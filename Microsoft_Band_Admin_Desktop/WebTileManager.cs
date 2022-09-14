// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTileManager
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using Microsoft.Band.Admin.WebTiles;
using Microsoft.Band.Tiles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Microsoft.Band.Admin
{
  internal class WebTileManager : IWebTileManager
  {
    public const string WebTileTempFolder = "Temp_WebTile";
    public const string WebTileFolder = "WebTiles";
    public static readonly string WebTileInstalledFolder = Path.Combine("WebTiles", "Installed");
    private const string PackageFolderName = "Package";
    private const string DataFolderName = "Data";
    private const string ManifestFileName = "manifest.json";
    private const string ZipTempFolder = "Temp_Zip";
    private const string HeadersFileName = "headers.json";
    private IStorageProvider storageProvider;
    private IImageProvider imageProvider;
    private object debugLock = new object();

    internal WebTileManager(IStorageProvider storageProvider, IImageProvider imageProvider)
    {
      this.storageProvider = storageProvider;
      this.imageProvider = imageProvider;
    }

    public async Task<IWebTile> GetWebTilePackageAsync(Uri uri)
    {
      IWebTile webTile = (IWebTile) null;
      string url = this.GetWebTileUrlFromUri(uri);
      string zipPath = "";
      string str = zipPath;
      zipPath = await this.DownloadWebTileAsync(url);
      try
      {
        using (Stream zipStream = await Task.Run<Stream>((Func<Stream>) (() => this.storageProvider.OpenFileForRead(StorageProviderRoot.App, zipPath, -1))))
        {
          webTile = await this.GetWebTilePackageAsync(zipStream, url);
          webTile.RequestHeaders = WebTileAgentHelper.GetAgentHeadersForUrl(url, webTile.Organization);
        }
      }
      finally
      {
        this.TryDeleteFolder(StorageProviderRoot.App, "Temp_Zip", "the temporary webtile file");
      }
      return webTile;
    }

    public async Task<IWebTile> GetWebTilePackageAsync(
      Stream source,
      string sourceFileName)
    {
      IWebTile webTile = await Task.Run<IWebTile>((Func<IWebTile>) (() =>
      {
        this.storageProvider.DeleteFolder(StorageProviderRoot.App, "Temp_WebTile");
        ZipUtils.Unzip(this.storageProvider, StorageProviderRoot.App, source, "Temp_WebTile");
        try
        {
          IWebTile webTile1 = this.DeserializeWebTilePackageFromJson(Path.Combine("Temp_WebTile", "manifest.json"));
          webTile1.Validate();
          return webTile1;
        }
        catch
        {
          this.TryDeleteFolder(StorageProviderRoot.App, "Temp_WebTile", "temp folder after failed JSON deserialize");
          throw;
        }
      }));
      webTile.StorageProvider = this.storageProvider;
      webTile.ImageProvider = this.imageProvider;
      webTile.PackageFolderPath = "Temp_WebTile";
      await webTile.LoadIconsAsync();
      return webTile;
    }

    public Task InstallWebTileAsync(IWebTile webTile) => Task.Run((Action) (() => this.InstallWebTile(webTile)));

    private void InstallWebTile(IWebTile webTile)
    {
      if (webTile == null)
        throw new ArgumentNullException(nameof (webTile));
      webTile.TileId = Guid.NewGuid();
      string path1 = Path.Combine(WebTileManager.WebTileInstalledFolder, webTile.TileId.ToString());
      string relativeDestFolder = Path.Combine(path1, "Package");
      this.storageProvider.MoveFolder(StorageProviderRoot.App, "Temp_WebTile", StorageProviderRoot.App, relativeDestFolder);
      webTile.PackageFolderPath = relativeDestFolder;
      webTile.DataFolderPath = Path.Combine(path1, "Data");
      webTile.SaveResourceAuthentication();
      if (!this.storageProvider.DirectoryExists(StorageProviderRoot.App, webTile.DataFolderPath))
        this.storageProvider.CreateFolder(StorageProviderRoot.App, webTile.DataFolderPath);
      webTile.SaveUserSettings();
      if (webTile.RequestHeaders == null || ((IEnumerable<HeaderNameValuePair>) webTile.RequestHeaders).Count<HeaderNameValuePair>() <= 0)
        return;
      using (Stream outputStream = this.storageProvider.OpenFileForWrite(StorageProviderRoot.App, Path.Combine(webTile.DataFolderPath, "headers.json"), false))
        CargoClient.SerializeJson(outputStream, (object) webTile.RequestHeaders);
    }

    public Task UninstallWebTileAsync(Guid tileId) => Task.Run((Action) (() => this.UninstallWebTile(tileId)));

    private void UninstallWebTile(Guid tileId)
    {
      this.GetWebTile(tileId)?.DeleteStoredResourceCredentials();
      this.storageProvider.DeleteFolder(StorageProviderRoot.App, Path.Combine(WebTileManager.WebTileInstalledFolder, tileId.ToString()));
    }

    public Task<IList<IWebTile>> GetInstalledWebTilesAsync(
      bool loadTileDisplayIcons)
    {
      throw new NotImplementedException();
    }

    public async Task<AdminBandTile> CreateAdminBandTileAsync(
      IWebTile webTile,
      BandClass bandClass)
    {
      AdminTileSettings tileSettings = AdminTileSettings.None;
      List<BandIcon> bandIconList = new List<BandIcon>();
      bool flag = false;
      if (webTile.BadgeIcons != null)
        tileSettings |= AdminTileSettings.EnableBadging;
      AdminBandTile bandTile = new AdminBandTile(webTile.TileId, webTile.Name, tileSettings);
      bandTile.OwnerId = AdminBandTile.WebTileOwnerId;
      bandIconList.Add(webTile.TileBandIcon);
      if (webTile.BadgeBandIcon != null)
      {
        flag = true;
        bandIconList.Add(webTile.BadgeBandIcon);
      }
      else
        bandIconList.Add(webTile.TileBandIcon);
      if (webTile.AdditionalBandIcons != null)
        bandIconList.AddRange((IEnumerable<BandIcon>) webTile.AdditionalBandIcons);
      bandTile.SetImageList(webTile.TileId, (IList<BandIcon>) bandIconList, 0U, flag ? new uint?(1U) : new uint?());
      TileLayout[] layoutsAsync = await webTile.GetLayoutsAsync(bandClass);
      for (int index = 0; index < layoutsAsync.Length; ++index)
        bandTile.Layouts.Add((uint) index, layoutsAsync[index]);
      if (webTile.TileTheme != null)
        bandTile.Theme = this.GetBandTheme(webTile);
      return bandTile;
    }

    private BandTheme GetBandTheme(IWebTile webTile) => new BandTheme()
    {
      Base = this.GetBandColor(webTile.TileTheme.Base),
      Highlight = this.GetBandColor(webTile.TileTheme.Highlight),
      Lowlight = this.GetBandColor(webTile.TileTheme.Lowlight),
      SecondaryText = this.GetBandColor(webTile.TileTheme.SecondaryText),
      HighContrast = this.GetBandColor(webTile.TileTheme.HighContrast),
      Muted = this.GetBandColor(webTile.TileTheme.Muted)
    };

    private BandColor GetBandColor(string color) => color != null ? new BandColor(uint.Parse(color, NumberStyles.HexNumber)) : throw new ArgumentNullException(nameof (color));

    public IList<Guid> GetInstalledWebTileIds()
    {
      IList<Guid> guidList = (IList<Guid>) new List<Guid>();
      if (!this.storageProvider.DirectoryExists(StorageProviderRoot.App, "WebTiles") || !this.storageProvider.DirectoryExists(StorageProviderRoot.App, WebTileManager.WebTileInstalledFolder))
        return guidList;
      foreach (string folder in this.storageProvider.GetFolders(StorageProviderRoot.App, WebTileManager.WebTileInstalledFolder))
      {
        Guid result;
        if (Guid.TryParse(Path.GetFileName(folder), out result))
          guidList.Add(result);
      }
      return guidList;
    }

    public IWebTile GetWebTile(Guid tileId)
    {
      string str1 = Path.Combine(WebTileManager.WebTileInstalledFolder, tileId.ToString());
      if (!this.storageProvider.DirectoryExists(StorageProviderRoot.App, str1))
        return (IWebTile) null;
      string str2 = Path.Combine(str1, "Package");
      if (!this.storageProvider.DirectoryExists(StorageProviderRoot.App, str2))
      {
        this.TryDeleteFolder(StorageProviderRoot.App, str1, "folder for installed webtile with missing package path");
        return (IWebTile) null;
      }
      try
      {
        IWebTile webTile = this.DeserializeWebTilePackageFromJson(Path.Combine(str2, "manifest.json"));
        webTile.TileId = tileId;
        webTile.StorageProvider = this.storageProvider;
        webTile.PackageFolderPath = str2;
        webTile.DataFolderPath = Path.Combine(str1, "Data");
        webTile.LoadResourceAuthentication();
        webTile.LoadUserSettings();
        string relativePath = Path.Combine(webTile.DataFolderPath, "headers.json");
        if (this.storageProvider.FileExists(StorageProviderRoot.App, relativePath))
        {
          using (Stream inputStream = this.storageProvider.OpenFileForRead(StorageProviderRoot.App, relativePath))
            webTile.RequestHeaders = CargoClient.DeserializeJson<HeaderNameValuePair[]>(inputStream);
        }
        return webTile;
      }
      catch
      {
        this.TryDeleteFolder(StorageProviderRoot.App, str1, "folder for installed webtile after failed JSON deserialize");
        throw;
      }
    }

    public Task DeleteAllStoredResourceCredentialsAsync() => Task.Run((Action) (() => this.DeleteAllStoredResourceCredentials()));

    private void DeleteAllStoredResourceCredentials()
    {
    }

    private IWebTile DeserializeWebTilePackageFromJson(string filePath)
    {
      using (Stream stream = this.storageProvider.OpenFileForRead(StorageProviderRoot.App, filePath, -1))
        return this.DeserializeWebTilePackageFromJson(stream);
    }

    [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
    private IWebTile DeserializeWebTilePackageFromJson(Stream stream, int bufferSize = 8192)
    {
      using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8, false, bufferSize, true))
      {
        using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader) streamReader))
          return (IWebTile) JsonSerializer.Create().Deserialize<WebTile>((JsonReader) jsonTextReader);
      }
    }

    private Task<IWebTile> DeserializeWebTilePackageFromJsonAsync(Guid tileId) => throw new NotImplementedException();

    private Task SerializeWebTiletoJsonManifestFileAsync(IWebTile tile, string manifestFilePath) => throw new NotImplementedException();

    private bool TryDeleteFolder(
      StorageProviderRoot root,
      string folderRelativePath,
      string description)
    {
      try
      {
        this.storageProvider.DeleteFolder(root, folderRelativePath);
      }
      catch (Exception ex)
      {
        Logger.LogException(LogLevel.Warning, ex, string.Format("Error occurred while deleting {0}.", (object) description));
        return false;
      }
      return true;
    }

    public Task<string> DownloadWebTileAsync(string url) => this.DownloadWebTileAsync(url, CancellationToken.None);

    public async Task<string> DownloadWebTileAsync(
      string url,
      CancellationToken cancellationToken)
    {
      if (url == null)
        throw new ArgumentNullException(nameof (url));
      Stopwatch transferTimer = Stopwatch.StartNew();
      Stream updateStream = (Stream) null;
      string localWebTileTempFileRelativePath = Path.Combine("Temp_Zip", Guid.NewGuid().ToString() + ".webtile");
      await Task.Run((Action) (() => this.storageProvider.CreateFolder(StorageProviderRoot.App, "Temp_Zip")));
      try
      {
        updateStream = await Task.Run<Stream>((Func<Stream>) (() => this.storageProvider.OpenFileForWrite(StorageProviderRoot.App, localWebTileTempFileRelativePath, false)));
      }
      catch (Exception ex)
      {
        BandException bandException = new BandException(string.Format(CommonSR.WebTileDownloadTempFileOpenError, (object) localWebTileTempFileRelativePath), ex);
        Logger.LogException(LogLevel.Error, (Exception) bandException);
        throw bandException;
      }
      try
      {
        using (updateStream)
          await this.DownloadFileAsync(url, updateStream, TimeSpan.FromSeconds(60.0), cancellationToken);
      }
      catch
      {
        this.TryDeleteFolder(StorageProviderRoot.App, "Temp_Zip", "the temporary webtile file, after an error downloading it");
        throw;
      }
      transferTimer.Stop();
      Logger.Log(LogLevel.Info, "Time to get web tile: {0}", (object) transferTimer.Elapsed);
      return localWebTileTempFileRelativePath;
    }

    private async Task DownloadFileAsync(
      string downloadFileUrl,
      Stream updateStream,
      TimeSpan timeout,
      CancellationToken cancellationToken)
    {
      string responseContent = string.Empty;
      Logger.Log(LogLevel.Info, "Downloading file using the URL: {0}", (object) downloadFileUrl);
      using (HttpClient client = new HttpClient())
      {
        client.Timeout = timeout;
        using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, downloadFileUrl))
        {
          using (HttpResponseMessage responseMessage = await this.SendAsync(client, requestMessage, cancellationToken))
          {
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
              this.LogRequestAndResponseMessages(LogLevel.Verbose, requestMessage, responseMessage);
              await responseMessage.Content.CopyToAsync(updateStream);
            }
            else
            {
              if (responseMessage.Content != null)
                responseContent = responseMessage.Content.ReadAsStringAsync().Result;
              throw WebTileManager.CreateAppropriateException(responseMessage, responseContent, CommonSR.WebTileDownloadError);
            }
          }
        }
      }
    }

    private Task<HttpResponseMessage> SendAsync(
      HttpClient client,
      HttpRequestMessage requestMessage,
      CancellationToken cancellationToken)
    {
      try
      {
        return client.SendAsync(requestMessage, cancellationToken);
      }
      catch (AggregateException ex)
      {
        if (ex.InnerExceptions.Count == 1)
        {
          if (ex.InnerException is TaskCanceledException)
          {
            cancellationToken.ThrowIfCancellationRequested();
            TimeoutException timeoutException = new TimeoutException();
            Logger.LogException(LogLevel.Error, (Exception) timeoutException);
            throw timeoutException;
          }
          Logger.LogException(LogLevel.Error, ex.InnerException);
          throw ex.InnerException;
        }
        throw;
      }
    }

    private void LogRequestAndResponseMessages(
      LogLevel level,
      HttpRequestMessage requestMessage,
      HttpResponseMessage responseMessage)
    {
      lock (this.debugLock)
      {
        Logger.Log(level, "Request: {0} {1}", (object) requestMessage.Method, (object) requestMessage.RequestUri);
        Logger.Log(level, "Response StatusCode: {0} ({1})", (object) responseMessage.StatusCode, (object) (int) responseMessage.StatusCode);
      }
    }

    private void LogRequestAndResponseMessages(
      LogLevel level,
      HttpRequestMessage requestMessage,
      HttpResponseMessage responseMessage,
      string responseContent)
    {
      lock (this.debugLock)
        this.LogRequestAndResponseMessages(level, requestMessage, responseMessage);
    }

    private static BandHttpException CreateAppropriateException(
      HttpResponseMessage responseMessage,
      string responseContent,
      string message,
      Exception innerException = null)
    {
      return new BandHttpException(responseContent, WebTileManager.FormatMessage(responseMessage, responseContent, message), innerException);
    }

    private static string FormatMessage(
      HttpResponseMessage responseMessage,
      string responseContent,
      string message)
    {
      StringWriter stringWriter = new StringWriter();
      stringWriter.WriteLine(message);
      if (string.IsNullOrWhiteSpace(responseMessage.ReasonPhrase))
        stringWriter.WriteLine(" {0}: {1} {2}", (object) CommonSR.HttpExceptionStatusLineLabel, (object) (int) responseMessage.StatusCode, (object) responseMessage.StatusCode);
      else
        stringWriter.WriteLine(" {0}: {1} {2} {3}", (object) CommonSR.HttpExceptionStatusLineLabel, (object) (int) responseMessage.StatusCode, (object) responseMessage.StatusCode, (object) responseMessage.ReasonPhrase);
      stringWriter.Write(" {0}: {1} {2}", (object) CommonSR.HttpExceptionRequestLineLabel, (object) responseMessage.RequestMessage.Method, (object) responseMessage.RequestMessage.RequestUri);
      if (!string.IsNullOrWhiteSpace(responseContent) && responseContent.Trim() != string.Empty)
      {
        stringWriter.WriteLine();
        stringWriter.Write(" {0}: {1}", (object) CommonSR.HttpExceptionResponseContentLabel, (object) responseContent);
      }
      return stringWriter.ToString();
    }

    private string GetWebTileUrlFromUri(Uri uri)
    {
      NameValueCollection queryString = HttpUtility.ParseQueryString(uri.Query);
      string str1 = queryString.Get("action");
      if (str1 != null && str1.Equals("download-manifest", StringComparison.OrdinalIgnoreCase))
      {
        string str2 = queryString.Get("url");
        if (str2 != null)
          return str2;
      }
      throw new BandException(CommonSR.WTUnableToParseUrlFromUri);
    }
  }
}
