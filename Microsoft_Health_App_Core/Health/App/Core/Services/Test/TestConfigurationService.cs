// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Test.TestConfigurationService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.Cloud.Client.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Test
{
  internal sealed class TestConfigurationService : ITestConfigurationService
  {
    private const string TestHeaderNameQueryName = "headerName";
    private const string TestHeaderValueQueryName = "headerValue";
    private readonly IEnvironmentService environmentService;
    private string testHeaderName;
    private string testHeaderValue;

    public TestConfigurationService(IEnvironmentService environmentService)
    {
      Assert.ParamIsNotNull((object) environmentService, nameof (environmentService));
      this.environmentService = environmentService;
    }

    public string TestHeaderName => this.testHeaderName;

    public string TestHeaderValue => this.testHeaderValue;

    public bool IsEnabled => !this.environmentService.IsPublicRelease;

    public Task InitializeAsync(Uri testUri, CancellationToken token)
    {
      if (this.IsEnabled)
      {
        IDictionary<string, string> query = testUri.ParseQuery();
        query.TryGetValue("headerName", out this.testHeaderName);
        query.TryGetValue("headerValue", out this.testHeaderValue);
      }
      return (Task) Task.FromResult<bool>(true);
    }
  }
}
