// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.ComponentContextReader
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.Threading;

namespace Microsoft.ApplicationInsights.Extensibility
{
  internal class ComponentContextReader : IComponentContextReader
  {
    internal const string UnknownComponentVersion = "Unknown";
    private static IComponentContextReader instance;

    public void Initialize()
    {
    }

    public string GetVersion() => string.Empty;

    internal ComponentContextReader()
    {
    }

    public static IComponentContextReader Instance
    {
      get
      {
        if (ComponentContextReader.instance != null)
          return ComponentContextReader.instance;
        Interlocked.CompareExchange<IComponentContextReader>(ref ComponentContextReader.instance, (IComponentContextReader) new ComponentContextReader(), (IComponentContextReader) null);
        ComponentContextReader.instance.Initialize();
        return ComponentContextReader.instance;
      }
      internal set => ComponentContextReader.instance = value;
    }
  }
}
