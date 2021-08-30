// Decompiled with JetBrains decompiler
// Type: ModernHttpClient.ProgressStreamContent
// Assembly: ModernHttpClient, Version=2.4.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6217D996-1B38-42C3-A52D-8A884E871EC8
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\ModernHttpClient.dll

using System;
using System.IO;
using System.Net.Http;

namespace ModernHttpClient
{
  public class ProgressStreamContent : StreamContent
  {
    private const string wrongVersion = "You're referencing the Portable version in your App - you need to reference the platform (iOS/Android) version";

    private ProgressStreamContent(Stream stream)
      : base(stream)
    {
      throw new Exception("You're referencing the Portable version in your App - you need to reference the platform (iOS/Android) version");
    }

    private ProgressStreamContent(Stream stream, int bufferSize)
      : base(stream, bufferSize)
    {
      throw new Exception("You're referencing the Portable version in your App - you need to reference the platform (iOS/Android) version");
    }

    public ProgressDelegate Progress
    {
      get => throw new Exception("You're referencing the Portable version in your App - you need to reference the platform (iOS/Android) version");
      set => throw new Exception("You're referencing the Portable version in your App - you need to reference the platform (iOS/Android) version");
    }
  }
}
