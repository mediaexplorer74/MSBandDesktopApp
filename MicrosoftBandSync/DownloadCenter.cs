// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.DownloadCenter
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopSyncApp
{
  public static class DownloadCenter
  {
    private static string JsonUrl = "https://go.microsoft.com/fwlink/?LinkId=512136";

    public static async Task<string> CheckUpdates(Version currentVersion) => await DownloadCenter.CheckUpdates(currentVersion, Environment.OSVersion.Version, CultureInfo.CurrentCulture.Name, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);

    public static async Task<string> CheckUpdates(
      Version currentVersion,
      Version windowsVersion,
      string cultureName,
      string languageISOCode)
    {
      DownloadCenter.MsiFileData msiFileData = DownloadCenter.LocateProperMsiFile(await DownloadCenter.GetMsiFiles(DownloadCenter.JsonUrl), currentVersion, windowsVersion, cultureName, languageISOCode);
      return msiFileData != null ? msiFileData.Url : "";
    }

    private static async Task<DownloadCenter.MsiFiles> GetMsiFiles(string url)
    {
      DownloadCenter.MsiFiles msi = (DownloadCenter.MsiFiles) null;
      using (MemoryStream stream = new MemoryStream())
      {
        long num = await DownloadCenter.DownloadFile(url, (Stream) stream, new TimeSpan(0, 0, 30));
        if (stream.Length > 0L)
        {
          DataContractJsonSerializer contractJsonSerializer = new DataContractJsonSerializer(typeof (DownloadCenter.MsiFiles));
          stream.Position = 0L;
          msi = (DownloadCenter.MsiFiles) contractJsonSerializer.ReadObject((Stream) stream);
        }
      }
      return msi;
    }

    private static async Task<long> DownloadFile(
      string downloadFileUrl,
      Stream updateStream,
      TimeSpan timeout)
    {
      string empty = string.Empty;
      long bytesdownloaded = 0;
      using (HttpClient client = new HttpClient())
      {
        client.Timeout = timeout;
        using (HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, downloadFileUrl))
        {
          requestMessage.Headers.Add("User-Agent", Globals.DefaultUserAgent);
          using (HttpResponseMessage responseMessage = await DownloadCenter.Send(client, requestMessage, CancellationToken.None))
          {
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
              await responseMessage.Content.CopyToAsync(updateStream);
              bytesdownloaded = responseMessage.Content.Headers.ContentLength.HasValue ? responseMessage.Content.Headers.ContentLength.Value : 0L;
            }
            else if (responseMessage.Content != null)
            {
              string str = await responseMessage.Content.ReadAsStringAsync();
            }
          }
        }
      }
      return bytesdownloaded;
    }

    private static async Task<HttpResponseMessage> Send(
      HttpClient client,
      HttpRequestMessage requestMessage,
      CancellationToken cancellationToken)
    {
      HttpResponseMessage httpResponseMessage;
      try
      {
        httpResponseMessage = await client.SendAsync(requestMessage, cancellationToken);
      }
      catch (AggregateException ex)
      {
        if (ex.InnerExceptions.Count == 1)
        {
          if (ex.InnerException is TaskCanceledException)
          {
            cancellationToken.ThrowIfCancellationRequested();
            throw new TimeoutException();
          }
          throw ex.InnerException;
        }
        throw;
      }
      return httpResponseMessage;
    }

    private static DownloadCenter.MsiFileData LocateProperMsiFile(
      DownloadCenter.MsiFiles MsiList,
      Version CurrentVersion,
      Version WindowsVersion,
      string CultureName,
      string LanguageISOCode)
    {
      DownloadCenter.MsiFileData msiFileData = (DownloadCenter.MsiFileData) null;
      if (MsiList != null)
        msiFileData = ((((DownloadCenter.LocateMatchingMsiFile(MsiList, CurrentVersion, WindowsVersion, CultureName) ?? DownloadCenter.LocateMatchingMsiFile(MsiList, CurrentVersion, WindowsVersion, LanguageISOCode)) ?? DownloadCenter.LocateMatchingMsiFile(MsiList, CurrentVersion, WindowsVersion, "")) ?? DownloadCenter.LocateMatchingMsiFile(MsiList, CurrentVersion, (Version) null, CultureName)) ?? DownloadCenter.LocateMatchingMsiFile(MsiList, CurrentVersion, (Version) null, LanguageISOCode)) ?? DownloadCenter.LocateMatchingMsiFile(MsiList, CurrentVersion, (Version) null, "");
      return msiFileData;
    }

    private static DownloadCenter.MsiFileData LocateMatchingMsiFile(
      DownloadCenter.MsiFiles MsiList,
      Version CurrentVersion,
      Version WindowsVersion,
      string CultureName)
    {
      DownloadCenter.MsiFileData msiFileData1 = (DownloadCenter.MsiFileData) null;
      foreach (DownloadCenter.MsiFileData msiFileData2 in MsiList.FileData)
      {
        if (msiFileData2.CultureName.ToLower() == CultureName.ToLower() && DownloadCenter.SameVersion(msiFileData2.WindowsVersion, WindowsVersion) && CurrentVersion < msiFileData2.Version)
        {
          if (msiFileData2.LiveDate.HasValue)
          {
            if (!(msiFileData2.LiveDate.Value < DateTime.UtcNow))
              break;
          }
          msiFileData1 = msiFileData2;
          break;
        }
      }
      return msiFileData1;
    }

    private static bool SameVersion(Version v1, Version v2)
    {
      if (v1 == v2)
        return true;
      return v1 == (Version) null ? v2.Major == 0 && v2.Minor == 0 : (v2 == (Version) null ? v1.Major == 0 && v1.Minor == 0 : v1.Major == v2.Major && v1.Minor == v2.Minor);
    }

    [DataContract]
    private sealed class MsiFiles
    {
      [DataMember]
      public List<DownloadCenter.MsiFileData> FileData { get; set; }
    }

    [DataContract]
    private sealed class MsiFileData
    {
      [DataMember]
      public string CultureName { get; set; }

      [DataMember]
      public Version Version { get; set; }

      [DataMember]
      public Version WindowsVersion { get; set; }

      [DataMember]
      public string Description { get; set; }

      [DataMember]
      public string Url { get; set; }

      [DataMember]
      public DateTime? LiveDate { get; set; }
    }
  }
}
