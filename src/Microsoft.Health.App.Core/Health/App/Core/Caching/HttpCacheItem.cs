// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Caching.HttpCacheItem
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.Cloud.Client.Http;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Microsoft.Health.App.Core.Caching
{
  public class HttpCacheItem : 
    HealthObservableObject,
    IHttpCacheItem,
    INotifyPropertyChanged,
    IHttpResponseContent
  {
    private const string TagSeparator = "|";
    private string url;
    private byte[] responseBytes;
    private string cacheFileName;
    private string characterSet;
    private string contentEncoding;
    private string tagsStorage;
    private DateTimeOffset expiration;

    [PrimaryKey]
    public string Url
    {
      get => this.url;
      set => this.SetProperty<string>(ref this.url, value, nameof (Url));
    }

    [Ignore]
    public byte[] ResponseBytes
    {
      get => this.responseBytes;
      set => this.SetProperty<byte[]>(ref this.responseBytes, value, nameof (ResponseBytes));
    }

    public string CacheFileName
    {
      get => this.cacheFileName;
      set => this.SetProperty<string>(ref this.cacheFileName, value, nameof (CacheFileName));
    }

    public string CharacterSet
    {
      get => this.characterSet;
      set => this.SetProperty<string>(ref this.characterSet, value, nameof (CharacterSet));
    }

    public string ContentEncoding
    {
      get => this.contentEncoding;
      set => this.SetProperty<string>(ref this.contentEncoding, value, nameof (ContentEncoding));
    }

    public string TagsStorage
    {
      get => this.tagsStorage;
      set => this.SetProperty<string>(ref this.tagsStorage, value, nameof (TagsStorage));
    }

    [Ignore]
    public IList<string> Tags
    {
      get
      {
        if (string.IsNullOrEmpty(this.TagsStorage))
          return (IList<string>) new List<string>();
        return (IList<string>) new List<string>((IEnumerable<string>) this.TagsStorage.Split(new char[1]
        {
          "|"[0]
        }, StringSplitOptions.RemoveEmptyEntries));
      }
      set
      {
        if (value == null || value.Count == 0)
          this.TagsStorage = (string) null;
        else
          this.TagsStorage = "|" + string.Join("|", (IEnumerable<string>) value) + "|";
      }
    }

    public DateTimeOffset Expiration
    {
      get => this.expiration;
      set => this.SetProperty<DateTimeOffset>(ref this.expiration, value, nameof (Expiration));
    }
  }
}
