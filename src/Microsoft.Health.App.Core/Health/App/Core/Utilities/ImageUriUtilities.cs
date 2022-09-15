// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.ImageUriUtilities
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class ImageUriUtilities
  {
    public static Uri GetGolfHoleImage(Uri baseUri, int screenWidth)
    {
      if (baseUri == (Uri) null || screenWidth <= 0)
        return (Uri) null;
      string uriString = baseUri.AbsoluteUri;
      int startIndex = uriString.LastIndexOf('.');
      if (startIndex >= 0 && startIndex < uriString.Length)
      {
        string empty = string.Empty;
        string str = screenWidth > 640 ? (screenWidth > 1080 ? "_lg" : "_md") : "_sm";
        uriString = uriString.Insert(startIndex, str);
      }
      Uri result = (Uri) null;
      Uri.TryCreate(uriString, UriKind.Absolute, out result);
      return result;
    }
  }
}
