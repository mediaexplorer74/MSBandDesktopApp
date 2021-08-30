// Decompiled with JetBrains decompiler
// Type: ModernHttpClient.NativeCookieHandler
// Assembly: ModernHttpClient, Version=2.4.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6217D996-1B38-42C3-A52D-8A884E871EC8
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\ModernHttpClient.dll

using System;
using System.Collections.Generic;
using System.Net;

namespace ModernHttpClient
{
  public class NativeCookieHandler
  {
    private const string wrongVersion = "You're referencing the Portable version in your App - you need to reference the platform (iOS/Android) version";

    public void SetCookies(IEnumerable<Cookie> cookies) => throw new Exception("You're referencing the Portable version in your App - you need to reference the platform (iOS/Android) version");

    public List<Cookie> Cookies => throw new Exception("You're referencing the Portable version in your App - you need to reference the platform (iOS/Android) version");
  }
}
