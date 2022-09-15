// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.WebTileResource
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using Microsoft.Band.Admin.Desktop;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Microsoft.Band.Admin.WebTiles
{
  [DataContract]
  public class WebTileResource : IWebTileResource
  {
    private string url;
    private ResourceStyle style;
    private Dictionary<string, string> content;
    private WebTilePropertyValidator validator;
    private WebTileResourceCacheInfo cacheInfo;
    internal const int MaxFeed = 8;
    private static readonly Regex AllowedVariableName = new Regex("^([A-Za-z_]\\w*)$");
    private static IPlatformProvider platformProvider = (IPlatformProvider) new DesktopProvider();

    public IWebTileResourceCacheInfo CacheInfo
    {
      get => (IWebTileResourceCacheInfo) this.cacheInfo;
      set => this.cacheInfo = (WebTileResourceCacheInfo) value;
    }

    public WebTileResource()
    {
      this.style = ResourceStyle.Simple;
      this.validator = new WebTilePropertyValidator();
      this.cacheInfo = new WebTileResourceCacheInfo();
    }

    public bool AllowInvalidValues
    {
      get => this.validator.AllowInvalidValues;
      set => this.validator.AllowInvalidValues = value;
    }

    public Dictionary<string, string> PropertyErrors => this.validator.PropertyErrors;

    public HeaderNameValuePair[] RequestHeaders { get; set; }

    [DataMember(IsRequired = true, Name = "url")]
    public string Url
    {
      get => this.url;
      set => this.validator.SetProperty<string>(ref this.url, value, nameof (Url), value != null && Uri.IsWellFormedUriString(value, UriKind.Absolute), CommonSR.WTBadUrl);
    }

    [DataMember(Name = "style")]
    public ResourceStyle Style
    {
      get => this.style;
      set => this.style = value;
    }

    [DataMember(IsRequired = true, Name = "content")]
    public Dictionary<string, string> Content
    {
      get => this.content;
      set
      {
        this.validator.ClearPropertyError(nameof (Content));
        this.validator.CheckProperty(nameof (Content), value != null && value.Count > 0, CommonSR.WTMissingVariableDefinitions);
        if (value != null)
        {
          foreach (KeyValuePair<string, string> keyValuePair in value)
          {
            bool valid1 = this.ValidateVariableName(keyValuePair.Key);
            if (!valid1)
            {
              this.validator.CheckProperty(nameof (Content), valid1, string.Format(CommonSR.WTInvalidVariableName, (object) keyValuePair.Key));
              break;
            }
            bool valid2 = this.ValidateVariableExpression(keyValuePair.Value);
            if (!valid2)
            {
              this.validator.CheckProperty(nameof (Content), valid2, string.Format(CommonSR.WTInvalidVariableExpression, (object) keyValuePair.Key));
              break;
            }
          }
        }
        this.content = value;
      }
    }

    public string Username { get; set; }

    public string Password { get; set; }

    private string AuthenticationHeader => this.Username != null && this.Password != null ? Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", (object) this.Username, (object) this.Password))) : (string) null;

    private bool ValidateVariableName(string variableName) => !string.IsNullOrEmpty(variableName) && WebTileResource.AllowedVariableName.IsMatch(variableName);

    private bool ValidateVariableExpression(string variableExpression) => !string.IsNullOrWhiteSpace(variableExpression);

    public async Task<List<Dictionary<string, string>>> ResolveFeedContentMappingsAsync() => await this.DownloadResourceAsync(true) is XmlDocument data ? this.GetFeedContentMappings(data) : new List<Dictionary<string, string>>();

    internal async Task<object> DownloadResourceAsync(bool requiresXml = false)
    {
      string urlContent = (string) null;
      XmlDocument xdc = new XmlDocument();
      object result = (object) null;
      try
      {
        using (HttpResponseMessage responseMessage = await this.GetResourceResponseAsync())
        {
          if (responseMessage.Content != null)
          {
            if (responseMessage.Content.Headers != null && responseMessage.Content.Headers.ContentType != null && responseMessage.Content.Headers.ContentType.CharSet != null)
              responseMessage.Content.Headers.ContentType.CharSet = responseMessage.Content.Headers.ContentType.CharSet.Replace("\"", "");
            urlContent = await responseMessage.Content.ReadAsStringAsync();
          }
          try
          {
            if (responseMessage.StatusCode == HttpStatusCode.NotModified)
            {
              Logger.Log(Microsoft.Band.Admin.LogLevel.Info, "No new data for resource {0}", (object) this.Url);
              return (object) null;
            }
            responseMessage.EnsureSuccessStatusCode();
            if (responseMessage.Headers.ETag != null)
              this.cacheInfo.ETag = responseMessage.Headers.ETag.Tag;
            this.cacheInfo.LastModified = (string) null;
            if (responseMessage.Content.Headers.LastModified.HasValue)
              this.cacheInfo.LastModified = responseMessage.Content.Headers.LastModified.Value.ToString("r");
          }
          catch (Exception ex)
          {
            throw new BandHttpException(urlContent, CommonSR.WTFailedToFetchResourceData, ex);
          }
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(Microsoft.Band.Admin.LogLevel.Warning, ex);
        throw;
      }
      if (urlContent == null)
        return (object) null;
      try
      {
        xdc.LoadXml(urlContent);
        result = (object) xdc;
      }
      catch
      {
      }
      if (result == null)
      {
        if (!requiresXml)
        {
          try
          {
            result = (object) JToken.Parse(urlContent);
          }
          catch
          {
          }
        }
      }
      if (result == null)
      {
        Exception e = new Exception("Url content not recognized");
        Logger.LogException(Microsoft.Band.Admin.LogLevel.Info, e);
        throw e;
      }
      return result;
    }

    public bool IsSecure() => new Uri(this.Url).Scheme == "https";

    public async Task<bool> AuthenticateAsync()
    {
      using (HttpResponseMessage responseMessage = await this.GetResourceResponseAsync())
      {
        try
        {
          responseMessage.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
          if (responseMessage.StatusCode != HttpStatusCode.Unauthorized)
            throw new BandHttpException(await responseMessage.Content.ReadAsStringAsync(), CommonSR.WTFailedToFetchResourceData, ex);
          if (!this.IsSecure())
            throw new WebTileException(CommonSR.WTAuthenticationNeedsHttpsUri, ex);
          Logger.LogException(Microsoft.Band.Admin.LogLevel.Warning, ex, "Failed to authenticate");
          return false;
        }
      }
      return true;
    }

    private async Task<HttpResponseMessage> GetResourceResponseAsync()
    {
      HttpResponseMessage httpResponseMessage;
      using (HttpClient client = new HttpClient())
      {
        client.Timeout = TimeSpan.FromSeconds(30.0);
        using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, this.Url))
        {
          requestMessage.Headers.CacheControl = new CacheControlHeaderValue()
          {
            NoCache = true
          };
          requestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.2)");
          string authenticationHeader = this.AuthenticationHeader;
          if (authenticationHeader != null)
            requestMessage.Headers.Add("Authorization", string.Format("Basic {0}", (object) authenticationHeader));
          if (this.cacheInfo.ETag != null)
            requestMessage.Headers.TryAddWithoutValidation("If-None-Match", this.cacheInfo.ETag);
          if (this.cacheInfo.LastModified != null)
            requestMessage.Headers.TryAddWithoutValidation("If-Modified-Since", this.cacheInfo.LastModified);
          if (this.RequestHeaders != null)
          {
            foreach (HeaderNameValuePair requestHeader in this.RequestHeaders)
            {
              try
              {
                requestMessage.Headers.Add(requestHeader.name, requestHeader.value);
              }
              catch (Exception ex)
              {
                throw new BandHttpException(this.Url, string.Format(CommonSR.WTBadHTTPRequestHeader, (object) requestHeader.name, (object) requestHeader.value), ex);
              }
            }
          }
          httpResponseMessage = await client.SendAsync(requestMessage);
        }
      }
      return httpResponseMessage;
    }

    internal string GetUniqueItemId(XmlNamespaceHelper xmlnshelper, bool isRSS, XmlNode node)
    {
      string str = (string) null;
      string[] strArray1 = new string[2]
      {
        "guid",
        "pubDate"
      };
      string[] strArray2 = new string[3]
      {
        "id",
        "updated",
        "published"
      };
      foreach (string xpath in isRSS ? strArray1 : strArray2)
      {
        str = xmlnshelper.ResolveNodeWithNamespace(node, xpath);
        if (str != null)
          break;
      }
      if (str == null)
      {
        string s = xmlnshelper.ResolveNodeWithNamespace(node, isRSS ? "description" : "summary") ?? node.OuterXml;
        if (s != null)
        {
          byte[] bytes = Encoding.UTF8.GetBytes(s);
          str = BandBitConverter.ToString(WebTileResource.platformProvider.ComputeHashMd5(bytes));
        }
      }
      return str;
    }

    internal bool ItemIdIsInCache(string id)
    {
      bool flag = false;
      if (this.cacheInfo.FeedItemIds != null)
      {
        foreach (string feedItemId in this.cacheInfo.FeedItemIds)
        {
          if (string.Compare(id, feedItemId) == 0)
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    internal void AddItemIdsToCache(List<string> ids)
    {
      if (this.cacheInfo.FeedItemIds == null)
      {
        this.cacheInfo.FeedItemIds = ids;
      }
      else
      {
        foreach (string id in ids)
          this.cacheInfo.FeedItemIds.Add(id);
      }
      if (this.cacheInfo.FeedItemIds.Count <= 8)
        return;
      this.cacheInfo.FeedItemIds = this.cacheInfo.FeedItemIds.GetRange(this.cacheInfo.FeedItemIds.Count - 8, 8);
    }

    internal List<Dictionary<string, string>> GetFeedContentMappings(XmlDocument data)
    {
      List<Dictionary<string, string>> dictionaryList = new List<Dictionary<string, string>>();
      Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
      List<string> ids = new List<string>();
      XmlNamespaceHelper xmlnshelper = new XmlNamespaceHelper(data);
      xmlnshelper.RemoveDefaultNamespace(data);
      int num1 = 0;
      foreach (KeyValuePair<string, string> keyValuePair in this.Content)
      {
        if (keyValuePair.Value.StartsWith("/"))
        {
          ++num1;
          if (!dictionary1.ContainsKey(keyValuePair.Key))
          {
            string str = xmlnshelper.ResolveNodeWithNamespace((XmlNode) data.DocumentElement, keyValuePair.Value);
            if (str != null)
              dictionary1.Add(keyValuePair.Key, str);
          }
        }
      }
      if (num1 == this.Content.Count)
      {
        dictionaryList.Add(dictionary1);
        return dictionaryList;
      }
      XmlNodeList xmlNodeList;
      bool isRSS;
      if (data.DocumentElement.LocalName.ToLower() == "rss")
      {
        xmlNodeList = data.DocumentElement.SelectNodes("/rss/channel/item");
        isRSS = true;
      }
      else
      {
        xmlNodeList = data.DocumentElement.SelectNodes("/feed/entry");
        isRSS = false;
      }
      int num2 = Math.Min(xmlNodeList.Count, 8);
      for (int index = 0; index < num2; ++index)
      {
        string uniqueItemId = this.GetUniqueItemId(xmlnshelper, isRSS, xmlNodeList.Item(index));
        if (uniqueItemId != null)
        {
          if (!this.ItemIdIsInCache(uniqueItemId))
            ids.Add(uniqueItemId);
          else
            break;
        }
        Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
        foreach (KeyValuePair<string, string> keyValuePair in dictionary1)
          dictionary2.Add(keyValuePair.Key, keyValuePair.Value);
        foreach (KeyValuePair<string, string> keyValuePair in this.Content)
        {
          if (!dictionary2.ContainsKey(keyValuePair.Key))
          {
            string str = xmlnshelper.ResolveNodeWithNamespace(xmlNodeList.Item(index), keyValuePair.Value);
            if (str != null)
              dictionary2.Add(keyValuePair.Key, str);
          }
        }
        dictionaryList.Add(dictionary2);
      }
      this.AddItemIdsToCache(ids);
      return dictionaryList;
    }

    internal void ResolveContentMappings(Dictionary<string, string> mappings, XmlDocument document)
    {
      XmlNamespaceHelper xmlNamespaceHelper = new XmlNamespaceHelper(document);
      xmlNamespaceHelper.RemoveDefaultNamespace(document);
      foreach (KeyValuePair<string, string> keyValuePair in this.Content)
      {
        string str = xmlNamespaceHelper.ResolveNodeWithNamespace((XmlNode) document.DocumentElement, keyValuePair.Value);
        if (str != null)
          mappings[keyValuePair.Key] = str;
      }
    }

    internal void ResolveContentMappings(Dictionary<string, string> mappings, JToken j)
    {
      foreach (KeyValuePair<string, string> keyValuePair in this.Content)
      {
        string str = (string) j.SelectToken(keyValuePair.Value);
        if (str != null)
          mappings[keyValuePair.Key] = str;
      }
    }
  }
}
