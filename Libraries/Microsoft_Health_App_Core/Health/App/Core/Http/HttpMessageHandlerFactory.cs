// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Http.HttpMessageHandlerFactory
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Practices.ServiceLocation;
using ModernHttpClient;
using System;
using System.Net;
using System.Net.Http;

namespace Microsoft.Health.App.Core.Http
{
  public class HttpMessageHandlerFactory : IHttpMessageHandlerFactory
  {
    public static readonly ConfigurationValue<string> Proxy = ConfigurationValue.Create(nameof (HttpMessageHandlerFactory), nameof (Proxy), string.Empty);

    public static HttpMessageHandler CreateMessageHandler()
    {
      NativeMessageHandler nativeMessageHandler1 = new NativeMessageHandler();
      ((HttpClientHandler) nativeMessageHandler1).AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
      NativeMessageHandler nativeMessageHandler2 = nativeMessageHandler1;
      if (EnvironmentUtilities.IsDebugSettingEnabled)
      {
        string proxy = ServiceLocator.Current.GetInstance<IConfigurationService>().GetValue<string>(HttpMessageHandlerFactory.Proxy);
        if (!string.IsNullOrEmpty(proxy))
          ((HttpClientHandler) nativeMessageHandler2).Proxy = (IWebProxy) new HttpMessageHandlerFactory.DebugProxy(proxy);
      }
      return (HttpMessageHandler) nativeMessageHandler2;
    }

    public HttpMessageHandler CreateHandler() => HttpMessageHandlerFactory.CreateMessageHandler();

    private class DebugProxy : IWebProxy
    {
      private readonly string proxy;

      public DebugProxy(string proxy) => this.proxy = proxy;

      public ICredentials Credentials { get; set; }

      public Uri GetProxy(Uri destination) => new Uri(string.Format("http://{0}", new object[1]
      {
        (object) this.proxy
      }));

      public bool IsBypassed(Uri host) => false;
    }
  }
}
