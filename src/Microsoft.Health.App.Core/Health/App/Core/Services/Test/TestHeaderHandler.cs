// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Test.TestHeaderHandler
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Test
{
  public sealed class TestHeaderHandler : DelegatingHandler
  {
    private readonly ITestConfigurationService testService;

    public TestHeaderHandler(HttpMessageHandler innerHandler, ITestConfigurationService testService)
      : base(innerHandler)
    {
      Assert.ParamIsNotNull((object) testService, nameof (testService));
      this.testService = testService;
    }

    protected override Task<HttpResponseMessage> SendAsync(
      HttpRequestMessage request,
      CancellationToken cancellationToken)
    {
      string testHeaderName = this.testService.TestHeaderName;
      string testHeaderValue = this.testService.TestHeaderValue;
      if (!string.IsNullOrWhiteSpace(testHeaderName) && !string.IsNullOrWhiteSpace(testHeaderValue) && !request.Headers.Contains(testHeaderName) && this.testService.IsEnabled)
        request.Headers.Add(testHeaderName, testHeaderValue);
      return base.SendAsync(request, cancellationToken);
    }
  }
}
