// Decompiled with JetBrains decompiler
// Type: AsyncPlatformExtensions
// Assembly: Microsoft.Threading.Tasks.Extensions.Desktop, Version=1.0.168.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5E74C701-7BC8-493E-B2E6-765635FA6670
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Threading_Tasks_Extensions.Desktop.dll

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

public static class AsyncPlatformExtensions
{
  public static Task<string> DownloadStringTaskAsync(this WebClient webClient, string address) => webClient.DownloadStringTaskAsync(webClient.GetUri(address));

  public static Task<string> DownloadStringTaskAsync(this WebClient webClient, Uri address)
  {
    TaskCompletionSource<string> tcs = new TaskCompletionSource<string>((object) address);
    DownloadStringCompletedEventHandler completedHandler = (DownloadStringCompletedEventHandler) null;
    completedHandler = (DownloadStringCompletedEventHandler) ((sender, e) => TaskServices.HandleEapCompletion<string>(tcs, true, (AsyncCompletedEventArgs) e, (Func<string>) (() => e.Result), (Action) (() => webClient.DownloadStringCompleted -= completedHandler)));
    webClient.DownloadStringCompleted += completedHandler;
    try
    {
      webClient.DownloadStringAsync(address, (object) tcs);
    }
    catch
    {
      webClient.DownloadStringCompleted -= completedHandler;
      throw;
    }
    return tcs.Task;
  }

  public static Task<Stream> OpenReadTaskAsync(this WebClient webClient, string address) => webClient.OpenReadTaskAsync(webClient.GetUri(address));

  public static Task<Stream> OpenReadTaskAsync(this WebClient webClient, Uri address)
  {
    TaskCompletionSource<Stream> tcs = new TaskCompletionSource<Stream>((object) address);
    OpenReadCompletedEventHandler handler = (OpenReadCompletedEventHandler) null;
    handler = (OpenReadCompletedEventHandler) ((sender, e) => TaskServices.HandleEapCompletion<Stream>(tcs, true, (AsyncCompletedEventArgs) e, (Func<Stream>) (() => e.Result), (Action) (() => webClient.OpenReadCompleted -= handler)));
    webClient.OpenReadCompleted += handler;
    try
    {
      webClient.OpenReadAsync(address, (object) tcs);
    }
    catch
    {
      webClient.OpenReadCompleted -= handler;
      throw;
    }
    return tcs.Task;
  }

  public static Task<Stream> OpenWriteTaskAsync(this WebClient webClient, string address) => webClient.OpenWriteTaskAsync(webClient.GetUri(address), (string) null);

  public static Task<Stream> OpenWriteTaskAsync(this WebClient webClient, Uri address) => webClient.OpenWriteTaskAsync(address, (string) null);

  public static Task<Stream> OpenWriteTaskAsync(
    this WebClient webClient,
    string address,
    string method)
  {
    return webClient.OpenWriteTaskAsync(webClient.GetUri(address), method);
  }

  public static Task<Stream> OpenWriteTaskAsync(
    this WebClient webClient,
    Uri address,
    string method)
  {
    TaskCompletionSource<Stream> tcs = new TaskCompletionSource<Stream>((object) address);
    OpenWriteCompletedEventHandler handler = (OpenWriteCompletedEventHandler) null;
    handler = (OpenWriteCompletedEventHandler) ((sender, e) => TaskServices.HandleEapCompletion<Stream>(tcs, true, (AsyncCompletedEventArgs) e, (Func<Stream>) (() => e.Result), (Action) (() => webClient.OpenWriteCompleted -= handler)));
    webClient.OpenWriteCompleted += handler;
    try
    {
      webClient.OpenWriteAsync(address, method, (object) tcs);
    }
    catch
    {
      webClient.OpenWriteCompleted -= handler;
      throw;
    }
    return tcs.Task;
  }

  public static Task<string> UploadStringTaskAsync(
    this WebClient webClient,
    string address,
    string data)
  {
    return webClient.UploadStringTaskAsync(address, (string) null, data);
  }

  public static Task<string> UploadStringTaskAsync(
    this WebClient webClient,
    Uri address,
    string data)
  {
    return webClient.UploadStringTaskAsync(address, (string) null, data);
  }

  public static Task<string> UploadStringTaskAsync(
    this WebClient webClient,
    string address,
    string method,
    string data)
  {
    return webClient.UploadStringTaskAsync(webClient.GetUri(address), method, data);
  }

  public static Task<string> UploadStringTaskAsync(
    this WebClient webClient,
    Uri address,
    string method,
    string data)
  {
    TaskCompletionSource<string> tcs = new TaskCompletionSource<string>((object) address);
    UploadStringCompletedEventHandler handler = (UploadStringCompletedEventHandler) null;
    handler = (UploadStringCompletedEventHandler) ((sender, e) => TaskServices.HandleEapCompletion<string>(tcs, true, (AsyncCompletedEventArgs) e, (Func<string>) (() => e.Result), (Action) (() => webClient.UploadStringCompleted -= handler)));
    webClient.UploadStringCompleted += handler;
    try
    {
      webClient.UploadStringAsync(address, method, data, (object) tcs);
    }
    catch
    {
      webClient.UploadStringCompleted -= handler;
      throw;
    }
    return tcs.Task;
  }

  private static Uri GetUri(this WebClient webClient, string path)
  {
    string baseAddress = webClient.BaseAddress;
    Uri result;
    if (!string.IsNullOrEmpty(baseAddress))
    {
      if (!Uri.TryCreate(new Uri(baseAddress), path, out result))
        return new Uri(path);
    }
    else if (!Uri.TryCreate(path, UriKind.Absolute, out result))
      return new Uri(path);
    return webClient.GetUri(result);
  }

  private static Uri GetUri(this WebClient webClient, Uri address)
  {
    Uri result = !(address == (Uri) null) ? address : throw new ArgumentNullException(nameof (address));
    string baseAddress = webClient.BaseAddress;
    if (!address.IsAbsoluteUri && !string.IsNullOrEmpty(baseAddress) && !Uri.TryCreate(webClient.GetUri(baseAddress), address, out result))
      return address;
    if (result.Query != null && !(result.Query == string.Empty))
      return result;
    StringBuilder stringBuilder = new StringBuilder();
    string str = string.Empty;
    NameValueCollection queryString = webClient.QueryString;
    for (int index = 0; index < queryString.Count; ++index)
    {
      stringBuilder.Append(str + queryString.AllKeys[index] + "=" + queryString[index]);
      str = "&";
    }
    return new UriBuilder(result)
    {
      Query = stringBuilder.ToString()
    }.Uri;
  }

  public static Task<byte[]> DownloadDataTaskAsync(this WebClient webClient, string address) => webClient.DownloadDataTaskAsync(webClient.GetUri(address));

  public static Task<byte[]> DownloadDataTaskAsync(this WebClient webClient, Uri address)
  {
    TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>((object) address);
    DownloadDataCompletedEventHandler completedHandler = (DownloadDataCompletedEventHandler) null;
    completedHandler = (DownloadDataCompletedEventHandler) ((sender, e) => TaskServices.HandleEapCompletion<byte[]>(tcs, true, (AsyncCompletedEventArgs) e, (Func<byte[]>) (() => e.Result), (Action) (() => webClient.DownloadDataCompleted -= completedHandler)));
    webClient.DownloadDataCompleted += completedHandler;
    try
    {
      webClient.DownloadDataAsync(address, (object) tcs);
    }
    catch
    {
      webClient.DownloadDataCompleted -= completedHandler;
      throw;
    }
    return tcs.Task;
  }

  public static Task DownloadFileTaskAsync(
    this WebClient webClient,
    string address,
    string fileName)
  {
    return webClient.DownloadFileTaskAsync(webClient.GetUri(address), fileName);
  }

  public static Task DownloadFileTaskAsync(
    this WebClient webClient,
    Uri address,
    string fileName)
  {
    TaskCompletionSource<object> tcs = new TaskCompletionSource<object>((object) address);
    AsyncCompletedEventHandler completedHandler = (AsyncCompletedEventHandler) null;
    completedHandler = (AsyncCompletedEventHandler) ((sender, e) => TaskServices.HandleEapCompletion<object>(tcs, true, e, (Func<object>) (() => (object) null), (Action) (() => webClient.DownloadFileCompleted -= completedHandler)));
    webClient.DownloadFileCompleted += completedHandler;
    try
    {
      webClient.DownloadFileAsync(address, fileName, (object) tcs);
    }
    catch
    {
      webClient.DownloadFileCompleted -= completedHandler;
      throw;
    }
    return (Task) tcs.Task;
  }

  public static Task<byte[]> UploadDataTaskAsync(
    this WebClient webClient,
    string address,
    byte[] data)
  {
    return webClient.UploadDataTaskAsync(webClient.GetUri(address), (string) null, data);
  }

  public static Task<byte[]> UploadDataTaskAsync(
    this WebClient webClient,
    Uri address,
    byte[] data)
  {
    return webClient.UploadDataTaskAsync(address, (string) null, data);
  }

  public static Task<byte[]> UploadDataTaskAsync(
    this WebClient webClient,
    string address,
    string method,
    byte[] data)
  {
    return webClient.UploadDataTaskAsync(webClient.GetUri(address), method, data);
  }

  public static Task<byte[]> UploadDataTaskAsync(
    this WebClient webClient,
    Uri address,
    string method,
    byte[] data)
  {
    TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>((object) address);
    UploadDataCompletedEventHandler handler = (UploadDataCompletedEventHandler) null;
    handler = (UploadDataCompletedEventHandler) ((sender, e) => TaskServices.HandleEapCompletion<byte[]>(tcs, true, (AsyncCompletedEventArgs) e, (Func<byte[]>) (() => e.Result), (Action) (() => webClient.UploadDataCompleted -= handler)));
    webClient.UploadDataCompleted += handler;
    try
    {
      webClient.UploadDataAsync(address, method, data, (object) tcs);
    }
    catch
    {
      webClient.UploadDataCompleted -= handler;
      throw;
    }
    return tcs.Task;
  }

  public static Task<byte[]> UploadFileTaskAsync(
    this WebClient webClient,
    string address,
    string fileName)
  {
    return webClient.UploadFileTaskAsync(webClient.GetUri(address), (string) null, fileName);
  }

  public static Task<byte[]> UploadFileTaskAsync(
    this WebClient webClient,
    Uri address,
    string fileName)
  {
    return webClient.UploadFileTaskAsync(address, (string) null, fileName);
  }

  public static Task<byte[]> UploadFileTaskAsync(
    this WebClient webClient,
    string address,
    string method,
    string fileName)
  {
    return webClient.UploadFileTaskAsync(webClient.GetUri(address), method, fileName);
  }

  public static Task<byte[]> UploadFileTaskAsync(
    this WebClient webClient,
    Uri address,
    string method,
    string fileName)
  {
    TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>((object) address);
    UploadFileCompletedEventHandler handler = (UploadFileCompletedEventHandler) null;
    handler = (UploadFileCompletedEventHandler) ((sender, e) => TaskServices.HandleEapCompletion<byte[]>(tcs, true, (AsyncCompletedEventArgs) e, (Func<byte[]>) (() => e.Result), (Action) (() => webClient.UploadFileCompleted -= handler)));
    webClient.UploadFileCompleted += handler;
    try
    {
      webClient.UploadFileAsync(address, method, fileName, (object) tcs);
    }
    catch
    {
      webClient.UploadFileCompleted -= handler;
      throw;
    }
    return tcs.Task;
  }

  public static Task AnnounceOnlineTaskAsync(
    this AnnouncementClient source,
    EndpointDiscoveryMetadata discoveryMetadata)
  {
    return Task.Factory.FromAsync<EndpointDiscoveryMetadata>(new Func<EndpointDiscoveryMetadata, AsyncCallback, object, IAsyncResult>(source.BeginAnnounceOnline), new Action<IAsyncResult>(source.EndAnnounceOnline), discoveryMetadata, (object) null);
  }

  public static Task AnnounceOfflineTaskAsync(
    this AnnouncementClient source,
    EndpointDiscoveryMetadata discoveryMetadata)
  {
    return Task.Factory.FromAsync<EndpointDiscoveryMetadata>(new Func<EndpointDiscoveryMetadata, AsyncCallback, object, IAsyncResult>(source.BeginAnnounceOffline), new Action<IAsyncResult>(source.EndAnnounceOffline), discoveryMetadata, (object) null);
  }

  public static Task<HttpListenerContext> GetContextAsync(
    this HttpListener source)
  {
    return Task<HttpListenerContext>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(source.BeginGetContext), new Func<IAsyncResult, HttpListenerContext>(source.EndGetContext), (object) null);
  }

  public static Task<X509Certificate2> GetClientCertificateAsync(
    this HttpListenerRequest source)
  {
    return Task<X509Certificate2>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(source.BeginGetClientCertificate), new Func<IAsyncResult, X509Certificate2>(source.EndGetClientCertificate), (object) null);
  }

  public static Task AuthenticateAsClientAsync(this NegotiateStream source) => Task.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(source.BeginAuthenticateAsClient), new Action<IAsyncResult>(source.EndAuthenticateAsClient), (object) null);

  public static Task AuthenticateAsClientAsync(
    this NegotiateStream source,
    NetworkCredential credential,
    string targetName)
  {
    return Task.Factory.FromAsync<NetworkCredential, string>(new Func<NetworkCredential, string, AsyncCallback, object, IAsyncResult>(source.BeginAuthenticateAsClient), new Action<IAsyncResult>(source.EndAuthenticateAsClient), credential, targetName, (object) null);
  }

  public static Task AuthenticateAsClientAsync(
    this NegotiateStream source,
    NetworkCredential credential,
    ChannelBinding binding,
    string targetName)
  {
    return Task.Factory.FromAsync<NetworkCredential, ChannelBinding, string>(new Func<NetworkCredential, ChannelBinding, string, AsyncCallback, object, IAsyncResult>(source.BeginAuthenticateAsClient), new Action<IAsyncResult>(source.EndAuthenticateAsClient), credential, binding, targetName, (object) null);
  }

  public static Task AuthenticateAsServerAsync(this NegotiateStream source) => Task.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(source.BeginAuthenticateAsServer), new Action<IAsyncResult>(source.EndAuthenticateAsServer), (object) null);

  public static Task AuthenticateAsServerAsync(
    this NegotiateStream source,
    ExtendedProtectionPolicy policy)
  {
    return Task.Factory.FromAsync<ExtendedProtectionPolicy>(new Func<ExtendedProtectionPolicy, AsyncCallback, object, IAsyncResult>(source.BeginAuthenticateAsServer), new Action<IAsyncResult>(source.EndAuthenticateAsServer), policy, (object) null);
  }

  public static Task AuthenticateAsServerAsync(
    this NegotiateStream source,
    NetworkCredential credential,
    ProtectionLevel requiredProtectionLevel,
    TokenImpersonationLevel requiredImpersonationLevel)
  {
    return Task.Factory.FromAsync<NetworkCredential, ProtectionLevel, TokenImpersonationLevel>(new Func<NetworkCredential, ProtectionLevel, TokenImpersonationLevel, AsyncCallback, object, IAsyncResult>(source.BeginAuthenticateAsServer), new Action<IAsyncResult>(source.EndAuthenticateAsServer), credential, requiredProtectionLevel, requiredImpersonationLevel, (object) null);
  }

  public static Task AuthenticateAsClientAsync(this SslStream source, string targetHost) => Task.Factory.FromAsync<string>(new Func<string, AsyncCallback, object, IAsyncResult>(source.BeginAuthenticateAsClient), new Action<IAsyncResult>(source.EndAuthenticateAsClient), targetHost, (object) null);

  public static Task AuthenticateAsServerAsync(
    this SslStream source,
    X509Certificate serverCertificate)
  {
    return Task.Factory.FromAsync<X509Certificate>(new Func<X509Certificate, AsyncCallback, object, IAsyncResult>(source.BeginAuthenticateAsServer), new Action<IAsyncResult>(source.EndAuthenticateAsServer), serverCertificate, (object) null);
  }

  public static Task ConnectAsync(this TcpClient source, string hostname, int port) => Task.Factory.FromAsync<string, int>(new Func<string, int, AsyncCallback, object, IAsyncResult>(source.BeginConnect), new Action<IAsyncResult>(source.EndConnect), hostname, port, (object) null);

  public static Task ConnectAsync(this TcpClient source, IPAddress address, int port) => Task.Factory.FromAsync<IPAddress, int>(new Func<IPAddress, int, AsyncCallback, object, IAsyncResult>(source.BeginConnect), new Action<IAsyncResult>(source.EndConnect), address, port, (object) null);

  public static Task ConnectAsync(this TcpClient source, IPAddress[] ipAddresses, int port) => Task.Factory.FromAsync<IPAddress[], int>(new Func<IPAddress[], int, AsyncCallback, object, IAsyncResult>(source.BeginConnect), new Action<IAsyncResult>(source.EndConnect), ipAddresses, port, (object) null);

  public static Task<Socket> AcceptSocketAsync(this TcpListener source) => Task<Socket>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(source.BeginAcceptSocket), new Func<IAsyncResult, Socket>(source.EndAcceptSocket), (object) null);

  public static Task<TcpClient> AcceptTcpClientAsync(this TcpListener source) => Task<TcpClient>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(source.BeginAcceptTcpClient), new Func<IAsyncResult, TcpClient>(source.EndAcceptTcpClient), (object) null);

  public static Task<int> SendAsync(
    this UdpClient source,
    byte[] datagram,
    int bytes,
    IPEndPoint endPoint)
  {
    return Task<int>.Factory.FromAsync<byte[], int, IPEndPoint>(new Func<byte[], int, IPEndPoint, AsyncCallback, object, IAsyncResult>(source.BeginSend), new Func<IAsyncResult, int>(source.EndSend), datagram, bytes, endPoint, (object) null);
  }

  public static Task<int> SendAsync(this UdpClient source, byte[] datagram, int bytes) => Task<int>.Factory.FromAsync<byte[], int>(new Func<byte[], int, AsyncCallback, object, IAsyncResult>(source.BeginSend), new Func<IAsyncResult, int>(source.EndSend), datagram, bytes, (object) null);

  public static Task<int> SendAsync(
    this UdpClient source,
    byte[] datagram,
    int bytes,
    string hostname,
    int port)
  {
    return Task<int>.Factory.FromAsync((Func<AsyncCallback, object, IAsyncResult>) ((callback, state) => source.BeginSend(datagram, bytes, hostname, port, callback, state)), new Func<IAsyncResult, int>(source.EndSend), (object) null);
  }

  public static Task<UnicastIPAddressInformationCollection> GetUnicastAddressesAsync(
    this IPGlobalProperties source)
  {
    return Task<UnicastIPAddressInformationCollection>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(source.BeginGetUnicastAddresses), new Func<IAsyncResult, UnicastIPAddressInformationCollection>(source.EndGetUnicastAddresses), (object) null);
  }

  public static Task OpenAsync(this SqlConnection source) => source.OpenAsync(CancellationToken.None);

  public static Task OpenAsync(this SqlConnection source, CancellationToken cancellationToken) => Task.Factory.StartNew((Action) (() => source.Open()), cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);

  public static Task<int> ExecuteNonQueryAsync(this SqlCommand source) => source.ExecuteNonQueryAsync(CancellationToken.None);

  public static Task<int> ExecuteNonQueryAsync(
    this SqlCommand source,
    CancellationToken cancellationToken)
  {
    return cancellationToken.IsCancellationRequested ? TaskServices.FromCancellation<int>(cancellationToken) : Task<int>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(source.BeginExecuteNonQuery), new Func<IAsyncResult, int>(source.EndExecuteNonQuery), (object) null);
  }

  public static Task<XmlReader> ExecuteXmlReaderAsync(this SqlCommand source) => source.ExecuteXmlReaderAsync(CancellationToken.None);

  public static Task<XmlReader> ExecuteXmlReaderAsync(
    this SqlCommand source,
    CancellationToken cancellationToken)
  {
    return cancellationToken.IsCancellationRequested ? TaskServices.FromCancellation<XmlReader>(cancellationToken) : Task<XmlReader>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(source.BeginExecuteXmlReader), new Func<IAsyncResult, XmlReader>(source.EndExecuteXmlReader), (object) null);
  }

  public static Task<SqlDataReader> ExecuteReaderAsync(this SqlCommand source) => source.ExecuteReaderAsync(CancellationToken.None);

  public static Task<SqlDataReader> ExecuteReaderAsync(
    this SqlCommand source,
    CancellationToken cancellationToken)
  {
    return cancellationToken.IsCancellationRequested ? TaskServices.FromCancellation<SqlDataReader>(cancellationToken) : Task<SqlDataReader>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(source.BeginExecuteReader), new Func<IAsyncResult, SqlDataReader>(source.EndExecuteReader), (object) null);
  }

  public static Task<MetadataSet> GetMetadataAsync(this MetadataExchangeClient source) => Task<MetadataSet>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(source.BeginGetMetadata), new Func<IAsyncResult, MetadataSet>(source.EndGetMetadata), (object) null);

  public static Task<MetadataSet> GetMetadataAsync(
    this MetadataExchangeClient source,
    Uri address,
    MetadataExchangeClientMode mode)
  {
    return Task<MetadataSet>.Factory.FromAsync<Uri, MetadataExchangeClientMode>(new Func<Uri, MetadataExchangeClientMode, AsyncCallback, object, IAsyncResult>(source.BeginGetMetadata), new Func<IAsyncResult, MetadataSet>(source.EndGetMetadata), address, mode, (object) null);
  }

  public static Task<MetadataSet> GetMetadataAsync(
    this MetadataExchangeClient source,
    EndpointAddress address)
  {
    return Task<MetadataSet>.Factory.FromAsync<EndpointAddress>(new Func<EndpointAddress, AsyncCallback, object, IAsyncResult>(source.BeginGetMetadata), new Func<IAsyncResult, MetadataSet>(source.EndGetMetadata), address, (object) null);
  }

  public static Task<FindResponse> FindTaskAsync(
    this DiscoveryClient discoveryClient,
    FindCriteria criteria)
  {
    TaskCompletionSource<FindResponse> tcs = discoveryClient != null ? new TaskCompletionSource<FindResponse>((object) discoveryClient) : throw new ArgumentNullException(nameof (discoveryClient));
    EventHandler<FindCompletedEventArgs> completedHandler = (EventHandler<FindCompletedEventArgs>) null;
    completedHandler = (EventHandler<FindCompletedEventArgs>) ((sender, e) => TaskServices.HandleEapCompletion<FindResponse>(tcs, true, (AsyncCompletedEventArgs) e, (Func<FindResponse>) (() => e.Result), (Action) (() => discoveryClient.FindCompleted -= completedHandler)));
    discoveryClient.FindCompleted += completedHandler;
    try
    {
      discoveryClient.FindAsync(criteria, (object) tcs);
    }
    catch
    {
      discoveryClient.FindCompleted -= completedHandler;
      throw;
    }
    return tcs.Task;
  }

  public static Task<ResolveResponse> ResolveTaskAsync(
    this DiscoveryClient discoveryClient,
    ResolveCriteria criteria)
  {
    TaskCompletionSource<ResolveResponse> tcs = discoveryClient != null ? new TaskCompletionSource<ResolveResponse>((object) discoveryClient) : throw new ArgumentNullException(nameof (discoveryClient));
    EventHandler<ResolveCompletedEventArgs> completedHandler = (EventHandler<ResolveCompletedEventArgs>) null;
    completedHandler = (EventHandler<ResolveCompletedEventArgs>) ((sender, e) => TaskServices.HandleEapCompletion<ResolveResponse>(tcs, true, (AsyncCompletedEventArgs) e, (Func<ResolveResponse>) (() => e.Result), (Action) (() => discoveryClient.ResolveCompleted -= completedHandler)));
    discoveryClient.ResolveCompleted += completedHandler;
    try
    {
      discoveryClient.ResolveAsync(criteria, (object) tcs);
    }
    catch
    {
      discoveryClient.ResolveCompleted -= completedHandler;
      throw;
    }
    return tcs.Task;
  }

  public static Task<PingReply> SendTaskAsync(this Ping ping, IPAddress address) => AsyncPlatformExtensions.SendTaskAsyncCore(ping, (object) address, (Action<TaskCompletionSource<PingReply>>) (tcs => ping.SendAsync(address, (object) tcs)));

  public static Task<PingReply> SendTaskAsync(this Ping ping, string hostNameOrAddress) => AsyncPlatformExtensions.SendTaskAsyncCore(ping, (object) hostNameOrAddress, (Action<TaskCompletionSource<PingReply>>) (tcs => ping.SendAsync(hostNameOrAddress, (object) tcs)));

  public static Task<PingReply> SendTaskAsync(
    this Ping ping,
    IPAddress address,
    int timeout)
  {
    return AsyncPlatformExtensions.SendTaskAsyncCore(ping, (object) address, (Action<TaskCompletionSource<PingReply>>) (tcs => ping.SendAsync(address, timeout, (object) tcs)));
  }

  public static Task<PingReply> SendTaskAsync(
    this Ping ping,
    string hostNameOrAddress,
    int timeout)
  {
    return AsyncPlatformExtensions.SendTaskAsyncCore(ping, (object) hostNameOrAddress, (Action<TaskCompletionSource<PingReply>>) (tcs => ping.SendAsync(hostNameOrAddress, timeout, (object) tcs)));
  }

  public static Task<PingReply> SendTaskAsync(
    this Ping ping,
    IPAddress address,
    int timeout,
    byte[] buffer)
  {
    return AsyncPlatformExtensions.SendTaskAsyncCore(ping, (object) address, (Action<TaskCompletionSource<PingReply>>) (tcs => ping.SendAsync(address, timeout, buffer, (object) tcs)));
  }

  public static Task<PingReply> SendTaskAsync(
    this Ping ping,
    string hostNameOrAddress,
    int timeout,
    byte[] buffer)
  {
    return AsyncPlatformExtensions.SendTaskAsyncCore(ping, (object) hostNameOrAddress, (Action<TaskCompletionSource<PingReply>>) (tcs => ping.SendAsync(hostNameOrAddress, timeout, buffer, (object) tcs)));
  }

  public static Task<PingReply> SendTaskAsync(
    this Ping ping,
    IPAddress address,
    int timeout,
    byte[] buffer,
    PingOptions options)
  {
    return AsyncPlatformExtensions.SendTaskAsyncCore(ping, (object) address, (Action<TaskCompletionSource<PingReply>>) (tcs => ping.SendAsync(address, timeout, buffer, options, (object) tcs)));
  }

  public static Task<PingReply> SendTaskAsync(
    this Ping ping,
    string hostNameOrAddress,
    int timeout,
    byte[] buffer,
    PingOptions options)
  {
    return AsyncPlatformExtensions.SendTaskAsyncCore(ping, (object) hostNameOrAddress, (Action<TaskCompletionSource<PingReply>>) (tcs => ping.SendAsync(hostNameOrAddress, timeout, buffer, options, (object) tcs)));
  }

  private static Task<PingReply> SendTaskAsyncCore(
    Ping ping,
    object userToken,
    Action<TaskCompletionSource<PingReply>> sendAsync)
  {
    if (ping == null)
      throw new ArgumentNullException(nameof (ping));
    TaskCompletionSource<PingReply> tcs = new TaskCompletionSource<PingReply>(userToken);
    PingCompletedEventHandler handler = (PingCompletedEventHandler) null;
    handler = (PingCompletedEventHandler) ((sender, e) => TaskServices.HandleEapCompletion<PingReply>(tcs, true, (AsyncCompletedEventArgs) e, (Func<PingReply>) (() => e.Reply), (Action) (() => ping.PingCompleted -= handler)));
    ping.PingCompleted += handler;
    try
    {
      sendAsync(tcs);
    }
    catch
    {
      ping.PingCompleted -= handler;
      throw;
    }
    return tcs.Task;
  }

  public static Task SendTaskAsync(
    this SmtpClient smtpClient,
    string from,
    string recipients,
    string subject,
    string body)
  {
    MailMessage message = new MailMessage(from, recipients, subject, body);
    return smtpClient.SendTaskAsync(message);
  }

  public static Task SendTaskAsync(this SmtpClient smtpClient, MailMessage message) => AsyncPlatformExtensions.SendTaskAsyncCore(smtpClient, (object) message, (Action<TaskCompletionSource<object>>) (tcs => smtpClient.SendAsync(message, (object) tcs)));

  private static Task SendTaskAsyncCore(
    SmtpClient smtpClient,
    object userToken,
    Action<TaskCompletionSource<object>> sendAsync)
  {
    if (smtpClient == null)
      throw new ArgumentNullException(nameof (smtpClient));
    TaskCompletionSource<object> tcs = new TaskCompletionSource<object>(userToken);
    SendCompletedEventHandler handler = (SendCompletedEventHandler) null;
    handler = (SendCompletedEventHandler) ((sender, e) => TaskServices.HandleEapCompletion<object>(tcs, true, e, (Func<object>) (() => (object) null), (Action) (() => smtpClient.SendCompleted -= handler)));
    smtpClient.SendCompleted += handler;
    try
    {
      sendAsync(tcs);
    }
    catch
    {
      smtpClient.SendCompleted -= handler;
      throw;
    }
    return (Task) tcs.Task;
  }
}
