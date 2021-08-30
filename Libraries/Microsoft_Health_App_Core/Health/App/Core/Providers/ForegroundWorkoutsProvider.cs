// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.ForegroundWorkoutsProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Bing.HealthAndFitness;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public class ForegroundWorkoutsProvider : WorkoutsProviderBase
  {
    private readonly IMessageBoxService messageBoxService;

    public ForegroundWorkoutsProvider(
      IConfig config,
      IBandConnectionFactory cargoConnectionFactory,
      IBingHealthAndFitnessClient healthAndFitnessClient,
      IHealthCloudClient healthCloudClient,
      IUserProfileService userProfileService,
      IEnvironmentService applicationEnvironmentService,
      IMessageBoxService messageBoxService)
      : base(config, cargoConnectionFactory, healthAndFitnessClient, healthCloudClient, userProfileService, applicationEnvironmentService)
    {
      this.messageBoxService = messageBoxService;
      this.ThrowOnUploadWorkoutBandFileNoRetry = true;
    }

    public override async Task<bool> CheckRetryAsync(
      string failureErrorBody,
      string failureErrorTitle)
    {
      return await this.messageBoxService.ShowAsync(failureErrorBody, failureErrorTitle, PortableMessageBoxButton.OKCancel) == PortableMessageBoxResult.OK;
    }
  }
}
