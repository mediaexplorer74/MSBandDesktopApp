// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.WhatsNewService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.App.Core.Utilities;
using System;

namespace Microsoft.Health.App.Core.Services
{
  public class WhatsNewService : IWhatsNewService
  {
    private const string WhatsNewServiceCategory = "WhatsNewService";
    private const string WhatsNewRevision = "10";
    public static readonly string LastKnownWhatsNewRevisionKey = ConfigurationValue.CreateKey(nameof (WhatsNewService), "LastKnownWhatsNewRevision");
    public static readonly ConfigurationValue<bool> HasWhatsNewBeenViewed = ConfigurationValue.CreateBoolean(nameof (WhatsNewService), nameof (HasWhatsNewBeenViewed), (Func<bool>) (() => true));
    public static readonly ConfigurationValue<int> ApplicationSessions = (ConfigurationValue<int>) ConfigurationValue.CreateInteger(nameof (WhatsNewService), nameof (ApplicationSessions), Range.GetInclusive<int>(0, int.MaxValue), (Func<int>) (() => 0));
    private readonly IConfig config;
    private readonly IConfigProvider configProvider;
    private readonly IConfigurationService configurationService;

    public event EventHandler WasViewedChanged;

    public WhatsNewService(
      IConfig config,
      IConfigProvider configProvider,
      IConfigurationService configurationService,
      IApplicationLifecycleService applicationLifecycleService)
    {
      this.config = config;
      this.configProvider = configProvider;
      this.configurationService = configurationService;
      applicationLifecycleService.Resuming += new EventHandler<object>(this.OnResuming);
    }

    private void OnResuming(object sender, object e) => this.IncrementApplicationSessions();

    public void UpdateViewStatus()
    {
      if (this.config.OobeStatus != OobeStatus.NotShown && this.configProvider.Get<string>(WhatsNewService.LastKnownWhatsNewRevisionKey, (string) null) != "10")
        this.HasBeenViewed = false;
      this.configProvider.Set<string>(WhatsNewService.LastKnownWhatsNewRevisionKey, "10");
    }

    public bool HasBeenViewed
    {
      get => this.configurationService.GetValue<bool>(WhatsNewService.HasWhatsNewBeenViewed);
      set
      {
        this.configurationService.SetValue<bool>((GenericConfigurationValue<bool>) WhatsNewService.HasWhatsNewBeenViewed, value);
        if (value)
          this.configurationService.SetValue<int>((GenericConfigurationValue<int>) WhatsNewService.ApplicationSessions, 0);
        EventHandler wasViewedChanged = this.WasViewedChanged;
        if (wasViewedChanged == null)
          return;
        wasViewedChanged((object) this, EventArgs.Empty);
      }
    }

    public int GetApplicationSessions() => this.configurationService.GetValue<int>(WhatsNewService.ApplicationSessions);

    public void IncrementApplicationSessions()
    {
      if (this.configurationService.GetValue<bool>(WhatsNewService.HasWhatsNewBeenViewed))
        return;
      int num1 = this.configurationService.GetValue<int>(WhatsNewService.ApplicationSessions);
      if (num1 >= int.MaxValue)
        return;
      int num2;
      this.configurationService.SetValue<int>((GenericConfigurationValue<int>) WhatsNewService.ApplicationSessions, num2 = num1 + 1);
    }
  }
}
