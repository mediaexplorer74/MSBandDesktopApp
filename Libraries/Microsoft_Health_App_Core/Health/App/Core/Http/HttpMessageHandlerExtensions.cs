// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Http.HttpMessageHandlerExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services.Test;
using Microsoft.Health.Cloud.Client.Authentication;
using Microsoft.Health.Cloud.Client.Http;
using Microsoft.Health.Cloud.Client.Services;
using System.Net.Http;

namespace Microsoft.Health.App.Core.Http
{
  public static class HttpMessageHandlerExtensions
  {
    public static HttpMessageHandler WithMsaAuthorization(
      this HttpMessageHandler innerHandler,
      IMsaTokenProvider tokenProvider,
      bool allowUI)
    {
      return (HttpMessageHandler) new MsaAuthorizationHandler(innerHandler, tokenProvider, allowUI);
    }

    public static HttpMessageHandler WithPodAuthorization(
      this HttpMessageHandler innerHandler,
      IConnectionInfoProvider connectionInfoProvider,
      bool allowUI)
    {
      return (HttpMessageHandler) new PodAuthorizationHandler(innerHandler, connectionInfoProvider, allowUI);
    }

    public static HttpMessageHandler WithRegion(
      this HttpMessageHandler innerHandler,
      IRegionService regionService)
    {
      return (HttpMessageHandler) new RegionHandler(innerHandler, regionService);
    }

    public static HttpMessageHandler WithCulture(
      this HttpMessageHandler innerHandler,
      ICultureService cultureService)
    {
      return (HttpMessageHandler) new CultureHandler(innerHandler, cultureService);
    }

    public static HttpMessageHandler WithUserAgent(
      this HttpMessageHandler innerHandler,
      IUserAgentService userAgentService)
    {
      return (HttpMessageHandler) new UserAgentHandler(innerHandler, userAgentService);
    }

    public static HttpMessageHandler WithHttpLogging(
      this HttpMessageHandler innerHandler)
    {
      return (HttpMessageHandler) new HttpLoggingHandler(innerHandler);
    }

    public static HttpMessageHandler WithRequestId(
      this HttpMessageHandler innerHandler)
    {
      return (HttpMessageHandler) new RequestIdHandler(innerHandler);
    }

    public static HttpMessageHandler WithTestHeader(
      this HttpMessageHandler innerHandler,
      ITestConfigurationService testService)
    {
      return (HttpMessageHandler) new TestHeaderHandler(innerHandler, testService);
    }
  }
}
