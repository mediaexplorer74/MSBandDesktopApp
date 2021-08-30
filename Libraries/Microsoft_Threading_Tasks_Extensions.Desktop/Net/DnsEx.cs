// Decompiled with JetBrains decompiler
// Type: System.Net.DnsEx
// Assembly: Microsoft.Threading.Tasks.Extensions.Desktop, Version=1.0.168.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5E74C701-7BC8-493E-B2E6-765635FA6670
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Threading_Tasks_Extensions.Desktop.dll

using System.Threading.Tasks;

namespace System.Net
{
  public static class DnsEx
  {
    public static Task<IPAddress[]> GetHostAddressesAsync(string hostNameOrAddress) => Task<IPAddress[]>.Factory.FromAsync<string>(new Func<string, AsyncCallback, object, IAsyncResult>(Dns.BeginGetHostAddresses), new Func<IAsyncResult, IPAddress[]>(Dns.EndGetHostAddresses), hostNameOrAddress, (object) null);

    public static Task<IPHostEntry> GetHostEntryAsync(IPAddress address) => Task<IPHostEntry>.Factory.FromAsync<IPAddress>(new Func<IPAddress, AsyncCallback, object, IAsyncResult>(Dns.BeginGetHostEntry), new Func<IAsyncResult, IPHostEntry>(Dns.EndGetHostEntry), address, (object) null);

    public static Task<IPHostEntry> GetHostEntryAsync(string hostNameOrAddress) => Task<IPHostEntry>.Factory.FromAsync<string>(new Func<string, AsyncCallback, object, IAsyncResult>(Dns.BeginGetHostEntry), new Func<IAsyncResult, IPHostEntry>(Dns.EndGetHostEntry), hostNameOrAddress, (object) null);
  }
}
