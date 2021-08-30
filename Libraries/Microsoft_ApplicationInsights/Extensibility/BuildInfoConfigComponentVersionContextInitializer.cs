// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.BuildInfoConfigComponentVersionContextInitializer
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Microsoft.ApplicationInsights.Extensibility
{
  public class BuildInfoConfigComponentVersionContextInitializer : IContextInitializer
  {
    private string version;

    public void Initialize(TelemetryContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      if (!string.IsNullOrEmpty(context.Component.Version))
        return;
      string str = LazyInitializer.EnsureInitialized<string>(ref this.version, new Func<string>(this.GetVersion));
      context.Component.Version = str;
    }

    protected virtual XElement LoadBuildInfoConfig()
    {
      XElement xelement = (XElement) null;
      try
      {
        string str = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BuildInfo.config");
        if (File.Exists(str))
        {
          xelement = XDocument.Load(str).Root;
          CoreEventSource.Log.LogVerbose("[BuildInfoConfigComponentVersionContextInitializer] File loaded." + str);
        }
        else
          CoreEventSource.Log.LogVerbose("[BuildInfoConfigComponentVersionContextInitializer] No file." + str);
      }
      catch (XmlException ex)
      {
        CoreEventSource.Log.BuildInfoConfigBrokenXmlError(ex.Message);
      }
      return xelement;
    }

    private string GetVersion()
    {
      XElement xelement1 = this.LoadBuildInfoConfig();
      if (xelement1 == null)
        return "Unknown";
      XElement xelement2 = xelement1.Descendants().Where<XElement>((Func<XElement, bool>) (item => item.Name.LocalName == "Build")).Descendants<XElement>().Where<XElement>((Func<XElement, bool>) (item => item.Name.LocalName == "MSBuild")).Descendants<XElement>().SingleOrDefault<XElement>((Func<XElement, bool>) (item => item.Name.LocalName == "BuildLabel"));
      return xelement2 == null || string.IsNullOrEmpty(xelement2.Value) ? "Unknown" : xelement2.Value;
    }
  }
}
