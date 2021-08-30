// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.CacheItemViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Caching;
using System;
using System.Text;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  public class CacheItemViewModel
  {
    private readonly IHttpCacheItem item;

    public CacheItemViewModel(IHttpCacheItem item) => this.item = item;

    public IHttpCacheItem Item => this.item;

    public string TimeLeftText
    {
      get
      {
        TimeSpan timeSpan = this.item.Expiration - DateTimeOffset.UtcNow;
        StringBuilder stringBuilder = new StringBuilder();
        if (timeSpan.TotalDays > 1.0)
        {
          stringBuilder.Append(Math.Floor(timeSpan.TotalDays));
          stringBuilder.Append("d ");
        }
        if (timeSpan.TotalHours > 1.0)
        {
          stringBuilder.Append(timeSpan.Hours);
          stringBuilder.Append("h ");
        }
        stringBuilder.Append(timeSpan.Minutes);
        stringBuilder.Append("m ");
        stringBuilder.Append(timeSpan.Seconds);
        stringBuilder.Append("s ");
        return stringBuilder.ToString().Trim();
      }
    }

    public int SizeBytes => this.item.ResponseBytes.Length;
  }
}
