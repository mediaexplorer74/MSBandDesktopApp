// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.ComponentContextInitializer
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.DataContracts;
using System;

namespace Microsoft.ApplicationInsights.Extensibility
{
  public class ComponentContextInitializer : IContextInitializer
  {
    public void Initialize(TelemetryContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      context.Component.Version = ComponentContextReader.Instance.GetVersion();
    }
  }
}
