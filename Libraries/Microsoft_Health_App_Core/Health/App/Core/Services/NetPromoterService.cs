// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.NetPromoterService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.App.Core.Utilities;
using System;

namespace Microsoft.Health.App.Core.Services
{
  public class NetPromoterService : INetPromoterService
  {
    private const string NetPromoterServiceCategory = "NetPromoterService";
    private readonly IConfig config;
    private readonly IConfigProvider configProvider;
    private readonly IConfigurationService configurationService;
    private readonly INetPromoterSettingsService netPromoterSettingsService;
    private bool promptForNps;
    private static readonly string LastKnownNetPromoterSurveyNumber = ConfigurationValue.CreateKey(nameof (NetPromoterService), nameof (LastKnownNetPromoterSurveyNumber));
    private static readonly string LastKnownNetPromoterReset = ConfigurationValue.CreateKey(nameof (NetPromoterService), nameof (LastKnownNetPromoterReset));
    private static readonly ConfigurationValue<string> LastDayUsed;
    private static readonly ConfigurationValue<bool> UserHasBeenPromptedForNps;
    private static readonly ConfigurationValue<int> ApplicationUseCount;
    private static readonly ConfigurationValue<int> UniqueDaysOfUseCount;

    public bool PromptUserForNpsSurvey
    {
      get => this.promptForNps;
      private set => this.promptForNps = value;
    }

    public bool UserHasFilledNpsSurvey => this.configurationService.GetValue<bool>(NetPromoterService.UserHasBeenPromptedForNps);

    public NetPromoterService(
      IConfig config,
      IConfigProvider configProvider,
      IConfigurationService configurationService,
      IUserProfileService userProfileService,
      IApplicationLifecycleService applicationLifecycleService,
      INetPromoterSettingsService netPromoterSettingsService)
    {
      this.config = config;
      this.configProvider = configProvider;
      this.configurationService = configurationService;
      this.netPromoterSettingsService = netPromoterSettingsService;
      applicationLifecycleService.Resuming += new EventHandler<object>(this.OnResuming);
      this.Initialize();
    }

    private void Initialize()
    {
      this.ResetPromptFlagForNewSurvey();
      this.ResetAppUseCountsBasedOnResetFlag();
      this.UpdateNpsPromptFlag();
    }

    private void OnResuming(object sender, object e) => this.IncrementApplicationUseCount();

    private void ResetPromptFlagForNewSurvey()
    {
      if (!(this.netPromoterSettingsService.NetPromoterSurveyNumber != this.configProvider.Get<string>(NetPromoterService.LastKnownNetPromoterSurveyNumber, (string) null)))
        return;
      if (this.netPromoterSettingsService.NetPromoterPromptAllUsersForSurvey)
        this.configurationService.SetValue<bool>((GenericConfigurationValue<bool>) NetPromoterService.UserHasBeenPromptedForNps, false);
      this.configProvider.Set<string>(NetPromoterService.LastKnownNetPromoterSurveyNumber, this.netPromoterSettingsService.NetPromoterSurveyNumber);
    }

    private void ResetAppUseCountsBasedOnResetFlag()
    {
      if (!(this.netPromoterSettingsService.NetPromoterResetTag != this.configProvider.Get<string>(NetPromoterService.LastKnownNetPromoterReset, (string) null)))
        return;
      this.ResetAppUseCount();
      this.configProvider.Set<string>(NetPromoterService.LastKnownNetPromoterReset, this.netPromoterSettingsService.NetPromoterResetTag);
    }

    public void ResetAppUseCount()
    {
      this.configurationService.SetValue<int>((GenericConfigurationValue<int>) NetPromoterService.ApplicationUseCount, 0);
      this.configurationService.SetValue<int>((GenericConfigurationValue<int>) NetPromoterService.UniqueDaysOfUseCount, 0);
      IConfigurationService configurationService = this.configurationService;
      ConfigurationValue<string> lastDayUsed = NetPromoterService.LastDayUsed;
      DateTime dateTime = DateTime.Today;
      dateTime = dateTime.AddDays(-1.0);
      string newValue = dateTime.ToString();
      configurationService.SetValue<string>((GenericConfigurationValue<string>) lastDayUsed, newValue);
    }

    private void UpdateNpsPromptFlag()
    {
      this.PromptUserForNpsSurvey = false;
      if (this.configurationService.GetValue<bool>(NetPromoterService.UserHasBeenPromptedForNps) || !this.netPromoterSettingsService.NetPromoterPromptNewUsers && !this.netPromoterSettingsService.NetPromoterPromptAllUsersForSurvey || this.configurationService.GetValue<int>(NetPromoterService.UniqueDaysOfUseCount) < this.netPromoterSettingsService.UniqueDaysOfUseThreshold || this.configurationService.GetValue<int>(NetPromoterService.ApplicationUseCount) < this.netPromoterSettingsService.ApplicationUseThreshold)
        return;
      this.PromptUserForNpsSurvey = true;
    }

    public void SetUserHasBeenPromptedForNpsFlag(bool userPrompted) => this.configurationService.SetValue<bool>((GenericConfigurationValue<bool>) NetPromoterService.UserHasBeenPromptedForNps, userPrompted);

    public void IncrementApplicationUseCount()
    {
      this.IncrementAppUseCount(NetPromoterService.ApplicationUseCount);
      this.IncrementDayUseCount(NetPromoterService.UniqueDaysOfUseCount, NetPromoterService.LastDayUsed);
      this.UpdateNpsPromptFlag();
    }

    private void IncrementAppUseCount(ConfigurationValue<int> key)
    {
      int num1 = this.configurationService.GetValue<int>(key);
      if (num1 >= int.MaxValue)
        return;
      int num2;
      this.configurationService.SetValue<int>((GenericConfigurationValue<int>) key, num2 = num1 + 1);
    }

    private void IncrementDayUseCount(
      ConfigurationValue<int> dayCountKey,
      ConfigurationValue<string> lastDayUsedDateKey)
    {
      if (DateTime.Today.ToString().Equals(this.configurationService.GetValue<string>(lastDayUsedDateKey)))
        return;
      int num1 = this.configurationService.GetValue<int>(dayCountKey);
      if (num1 < int.MaxValue)
      {
        int num2;
        this.configurationService.SetValue<int>((GenericConfigurationValue<int>) dayCountKey, num2 = num1 + 1);
      }
      this.configurationService.SetValue<string>((GenericConfigurationValue<string>) lastDayUsedDateKey, DateTime.Today.ToString());
    }

    public void SetNpsSettings(
      int appUseCount,
      DateTime lastDayUsed,
      int daysOfuseCount,
      bool isNpsDisplayed)
    {
      this.configurationService.SetValue<int>((GenericConfigurationValue<int>) NetPromoterService.ApplicationUseCount, appUseCount);
      this.configurationService.SetValue<string>((GenericConfigurationValue<string>) NetPromoterService.LastDayUsed, lastDayUsed.ToString());
      this.configurationService.SetValue<int>((GenericConfigurationValue<int>) NetPromoterService.UniqueDaysOfUseCount, daysOfuseCount);
      this.configurationService.SetValue<bool>((GenericConfigurationValue<bool>) NetPromoterService.UserHasBeenPromptedForNps, isNpsDisplayed);
    }

    static NetPromoterService()
    {
      DateTime dateTime = DateTime.Today;
      dateTime = dateTime.AddDays(-1.0);
      NetPromoterService.LastDayUsed = ConfigurationValue.Create(nameof (NetPromoterService), "LastUsedDate", dateTime.ToString());
      NetPromoterService.UserHasBeenPromptedForNps = ConfigurationValue.CreateBoolean(nameof (NetPromoterService), "NPSUserHasBeenPromptedForNPS", (Func<bool>) (() => false));
      NetPromoterService.ApplicationUseCount = (ConfigurationValue<int>) ConfigurationValue.CreateInteger(nameof (NetPromoterService), nameof (ApplicationUseCount), Range.GetInclusive<int>(0, int.MaxValue), (Func<int>) (() => 0));
      NetPromoterService.UniqueDaysOfUseCount = (ConfigurationValue<int>) ConfigurationValue.CreateInteger(nameof (NetPromoterService), "UniqueDaysCount", Range.GetInclusive<int>(0, int.MaxValue), (Func<int>) (() => 0));
    }
  }
}
