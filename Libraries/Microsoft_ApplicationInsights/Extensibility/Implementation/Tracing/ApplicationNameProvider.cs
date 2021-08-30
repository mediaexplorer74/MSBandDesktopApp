// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.ApplicationNameProvider
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
  internal sealed class ApplicationNameProvider
  {
    public ApplicationNameProvider() => this.Name = this.GetApplicationName();

    public string Name { get; private set; }

    private string GetApplicationName()
    {
      try
      {
        return AppDomain.CurrentDomain.FriendlyName;
      }
      catch (Exception ex)
      {
        return "Undefined " + ex.Message ?? ex.ToString();
      }
    }
  }
}
