// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.ServiceInfo
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

namespace Microsoft.Band.Admin
{
  public class ServiceInfo
  {
    private string fileUpdateServiceAddress = "http://fileupdatequeryservice.cloudapp.net";
    private string discoveryServiceAddress = "";
    private string podAddress = "";
    private string userAgent;

    public string DiscoveryServiceAddress
    {
      get => this.discoveryServiceAddress;
      set => this.discoveryServiceAddress = this.NormalizeAddress(value);
    }

    public string DiscoveryServiceAccessToken { get; set; }

    public string UserId { get; set; }

    public string PodAddress
    {
      get => this.podAddress;
      set => this.podAddress = this.NormalizeAddress(value);
    }

    public string AccessToken { get; set; }

    public string FileUpdateServiceAddress
    {
      get => this.fileUpdateServiceAddress;
      set => this.fileUpdateServiceAddress = this.NormalizeAddress(value);
    }

    public string UserAgent
    {
      get => this.userAgent;
      set => this.userAgent = value;
    }

    private string NormalizeAddress(string address)
    {
      string str = address.Trim();
      while (str.EndsWith("/"))
        str = str.Remove(str.Length - 1);
      return str;
    }
  }
}
