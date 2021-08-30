// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Platform.PlatformSingleton
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Platform
{
  internal static class PlatformSingleton
  {
    private static IPlatform current;

    public static IPlatform Current
    {
      get => PlatformSingleton.current ?? (PlatformSingleton.current = (IPlatform) new PlatformImplementation());
      set => PlatformSingleton.current = value;
    }
  }
}
