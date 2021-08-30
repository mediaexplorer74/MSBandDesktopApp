// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.FallbackDeviceContext
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;
using System.Xml.Linq;

namespace Microsoft.ApplicationInsights.Extensibility
{
  internal class FallbackDeviceContext : IFallbackContext
  {
    public string DeviceUniqueId { get; private set; }

    public void Initialize()
    {
      byte[] numArray = new byte[20];
      new Random().NextBytes(numArray);
      this.DeviceUniqueId = Convert.ToBase64String(numArray);
    }

    public void Serialize(XElement rootElement) => rootElement.Add((object) new XElement(XName.Get("DeviceUniqueId"), (object) this.DeviceUniqueId));

    public bool Deserialize(XElement rootElement)
    {
      if (rootElement == null)
        return false;
      XElement xelement = rootElement.Element(XName.Get("DeviceUniqueId"));
      if (xelement == null)
        return false;
      this.DeviceUniqueId = xelement.Value;
      return !this.DeviceUniqueId.IsNullOrWhiteSpace();
    }
  }
}
