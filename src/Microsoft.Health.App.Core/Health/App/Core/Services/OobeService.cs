// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.OobeService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Authentication;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Caching;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services.Configuration.Dynamic;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.AddBand;
using Microsoft.Health.App.Core.ViewModels.Oobe;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class OobeService : IOobeService
  {
    private const int OobeMinimumAge = 14;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\OobeService.cs");
    private readonly IHealthDiscoveryClient discoveryClient;
    private readonly IConfig config;
    private readonly IDynamicConfigurationService dynamicConfigurationService;
    private readonly ISmoothNavService smoothNavService;
    private readonly IUserProfileService userProfileService;
    private readonly IBackgroundTaskService backgroundTaskUtils;
    private readonly IConnectionInfoStore connectionInfoStore;
    private readonly IOAuthMsaTokenStore oAuthMsaTokenStore;
    private readonly IDebuggableHttpCacheService cacheService;
    private readonly IMessageBoxService messageBoxService;
    private readonly ITileManagementService tileManagementService;
    private readonly IRegionService regionService;
    private readonly ICultureService cultureService;
    private readonly IMessageSender messageSender;
    private readonly IWhatsNewService whatsNewService;
    private OobeStep currentStep;
    private BandUserProfile cloudUserProfile;
    private ITimedTelemetryEvent oobeTimedEvent;

    public OobeService(
      IHealthDiscoveryClient discoveryClient,
      IConfig config,
      ISmoothNavService smoothNavService,
      IUserProfileService userProfileService,
      IBackgroundTaskService backgroundTaskUtils,
      IConnectionInfoStore connectionInfoStore,
      IOAuthMsaTokenStore oAuthMsaTokenStore,
      IDebuggableHttpCacheService kCacheService,
      IMessageBoxService messageBoxService,
      ITileManagementService tileManagementService,
      IRegionService regionService,
      ICultureService cultureService,
      IMessageSender messageSender,
      IDynamicConfigurationService dynamicConfigurationService,
      IWhatsNewService whatsNewService)
    {
      this.discoveryClient = discoveryClient;
      this.config = config;
      this.smoothNavService = smoothNavService;
      this.userProfileService = userProfileService;
      this.backgroundTaskUtils = backgroundTaskUtils;
      this.connectionInfoStore = connectionInfoStore;
      this.oAuthMsaTokenStore = oAuthMsaTokenStore;
      this.cacheService = kCacheService;
      this.messageBoxService = messageBoxService;
      this.tileManagementService = tileManagementService;
      this.regionService = regionService;
      this.cultureService = cultureService;
      this.messageSender = messageSender;
      this.dynamicConfigurationService = dynamicConfigurationService;
      this.whatsNewService = whatsNewService;
    }

    public OobePhoneMotionTrackingData PhoneMotionTracking { get; set; }

    public async Task StartAsync(CancellationToken token)
    {
      this.smoothNavService.DisableNavPanel(typeof (IOobeService));
      this.PhoneMotionTracking = (OobePhoneMotionTrackingData) null;
      this.cloudUserProfile = (BandUserProfile) null;
      MsaUserProfile userProfileAsync = await this.discoveryClient.GetMsaUserProfileAsync(token);
      if (!userProfileAsync.BirthDate.HasValue || !this.IsMeetingMinimumAgeRequirement(userProfileAsync.BirthDate.Value))
      {
        this.currentStep = OobeStep.MinimumAge;
        this.smoothNavService.Navigate(typeof (OobeMinimumAgeRequirementViewModel));
      }
      else
      {
        this.currentStep = OobeStep.LegalTerms;
        this.smoothNavService.Navigate(typeof (OobeLegalTermsViewModel));
      }
      this.messageSender.Register<BackButtonPressedMessage>((object) this, new Action<BackButtonPressedMessage>(this.OnBackKeyPressed));
    }

    public async Task CompleteStepAsync(OobeStep step, CancellationToken token)
    {
      if (step != this.currentStep)
      {
        OobeService.Logger.Warn((object) ("Cannot complete OOBE step " + (object) step + ", we are currently on step " + (object) this.currentStep));
      }
      else
      {
        OobeService.Logger.Info((object) ("Completing OOBE step " + (object) step));
        switch (this.currentStep)
        {
          case OobeStep.NotInOobe:
            throw new InvalidOperationException("Cannot call CompleteStepAsync when not in OOBE");
          case OobeStep.MinimumAge:
            this.currentStep = OobeStep.LegalTerms;
            this.Navigate(typeof (OobeLegalTermsViewModel));
            break;
          case OobeStep.LegalTerms:
            this.currentStep = OobeStep.Profile;
            this.Navigate(typeof (OobeProfileViewModel));
            break;
          case OobeStep.Profile:
            this.currentStep = OobeStep.Done;
            this.Navigate(typeof (OobeDoneViewModel));
            break;
          case OobeStep.Done:
            await this.CompleteAsync(token);
            this.smoothNavService.Navigate(typeof (AddBandStartViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
            {
              {
                "IsOobe",
                bool.TrueString
              }
            });
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    private async Task CompleteAsync(CancellationToken cancellationToken)
    {
      await this.backgroundTaskUtils.RegisterBackgroundTasksAsync(cancellationToken, new bool?(false));
      await this.SaveUserProfileAsync();
      this.tileManagementService.EnabledTiles = (IList<AppBandTile>) new List<AppBandTile>();
      this.config.OobeStatus = OobeStatus.Shown;
      this.currentStep = OobeStep.NotInOobe;
      this.whatsNewService.UpdateViewStatus();
      ApplicationTelemetry.LogOobeComplete(this.userProfileService.CurrentUserProfile.Gender == Gender.Male, this.userProfileService.CurrentUserProfile.Birthdate, this.userProfileService.IsRegisteredBandPaired, this.PhoneMotionTracking);
      this.config.LastCompletedFullOobe = DateTimeOffset.Now;
      this.StopOobeTimer();
      this.messageSender.Unregister<BackButtonPressedMessage>((object) this, new Action<BackButtonPressedMessage>(this.OnBackKeyPressed));
      this.smoothNavService.DisableNavPanel(typeof (IOobeService));
    }

    public void SetCloudUserProfile(BandUserProfile profile, bool forceSetRegionInfo = false)
    {
      this.cloudUserProfile = profile;
      if (forceSetRegionInfo)
      {
        this.cloudUserProfile.PreferredLocale = this.cultureService.CurrentSupportedUICulture.Name;
        this.cloudUserProfile.PreferredRegion = this.regionService.CurrentRegion.TwoLetterISORegionName;
      }
      else
      {
        if (string.IsNullOrEmpty(this.cloudUserProfile.PreferredLocale))
          this.cloudUserProfile.PreferredLocale = this.cultureService.CurrentSupportedUICulture.Name;
        if (!string.IsNullOrEmpty(this.cloudUserProfile.PreferredRegion))
          return;
        this.cloudUserProfile.PreferredRegion = this.regionService.CurrentRegion.TwoLetterISORegionName;
      }
    }

    private async Task SaveUserProfileAsync()
    {
      int num = 0;
      if (num != 0 && this.cloudUserProfile == null)
        throw new InvalidOperationException("cloudUserProfile must be populated before calling SaveUserProfileAsync.");
      try
      {
        CancellationToken token = new CancellationTokenSource(TimeSpan.FromMinutes(5.0)).Token;
        this.cloudUserProfile.IsOobeCompleted = true;
        this.cloudUserProfile.TelemetryEnabled = true;
        await this.userProfileService.SaveCloudUserProfileAsync(this.cloudUserProfile, token);
      }
      catch (Exception ex)
      {
        OobeService.Logger.Error(ex, "Exception saving band settings");
        throw;
      }
    }

    private void StopOobeTimer()
    {
      if (this.oobeTimedEvent == null)
        return;
      this.oobeTimedEvent.End();
      this.oobeTimedEvent.Dispose();
      this.oobeTimedEvent = (ITimedTelemetryEvent) null;
    }

    private void Navigate(System.Type viewModelType) => this.smoothNavService.Navigate(viewModelType, action: NavigationStackAction.RemovePrevious);

    public async Task<bool> ResetOobeStatusAsync(
      bool resetProfileFlag = true,
      bool startOobe = true,
      bool unlinkBand = true)
    {
      try
      {
        if (resetProfileFlag)
        {
          BandUserProfile userProfileAsync = await this.userProfileService.GetCloudUserProfileAsync(CancellationToken.None);
          userProfileAsync.IsOobeCompleted = false;
          await this.userProfileService.SaveCloudUserProfileAsync(userProfileAsync, CancellationToken.None);
        }
        if (unlinkBand)
          await OobeService.TryUnlinkBandFromProfileAsync(this.userProfileService);
        this.config.OobeStatus = OobeStatus.NotShown;
        await this.userProfileService.SetUserProfileAsync((BandUserProfile) null, CancellationToken.None);
        if (startOobe)
          await this.StartAsync(CancellationToken.None);
        await this.connectionInfoStore.ClearAsync();
        await this.oAuthMsaTokenStore.ClearAsync();
        await this.cacheService.RemoveAllAsync();
        return true;
      }
      catch (Exception ex)
      {
        OobeService.Logger.Error(ex, "reset oobe");
      }
      int num = (int) await this.messageBoxService.ShowAsync(AppResources.ResetOobeMessage, AppResources.ResetOobeMessageHeader, PortableMessageBoxButton.OK);
      return false;
    }

    private static async Task TryUnlinkBandFromProfileAsync(
      IUserProfileService userProfileService)
    {
      OobeService.Logger.Debug((object) "<START> Unlinking band from profile");
      try
      {
        await userProfileService.UnlinkBandFromProfileAsync(CancellationToken.None).ConfigureAwait(false);
        OobeService.Logger.Debug((object) "<END> Unlinking band from profile");
      }
      catch (Exception ex)
      {
        OobeService.Logger.Error(ex, "<FAIL> Unlinking band from profile");
      }
    }

    private void OnBackKeyPressed(BackButtonPressedMessage message) => message.CancelAction();

    private int GetMinimumRequirementAge()
    {
      int minimumAge = this.dynamicConfigurationService.Configuration.Oobe.Defaults.MinimumAge;
      return minimumAge > 0 ? minimumAge : 14;
    }

    private bool IsMeetingMinimumAgeRequirement(DateTime birthDate) => AgeUtilities.GetAge(birthDate) >= this.GetMinimumRequirementAge();
  }
}
