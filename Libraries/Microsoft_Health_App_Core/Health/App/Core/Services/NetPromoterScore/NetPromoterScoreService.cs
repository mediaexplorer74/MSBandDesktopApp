// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.NetPromoterScore.NetPromoterScoreService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Diagnostics;
using System;
using System.Threading;

namespace Microsoft.Health.App.Core.Services.NetPromoterScore
{
  public class NetPromoterScoreService : INetPromoterScoreService
  {
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IBandHardwareService bandHardwareService;
    private readonly IUserProfileService userProfileService;

    public NetPromoterScoreService(
      IBandConnectionFactory cargoConnectionFactory,
      IBandHardwareService bandHardwareService,
      IUserProfileService userProfileService)
    {
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.bandHardwareService = bandHardwareService;
      this.userProfileService = userProfileService;
    }

    public async void PostNetPromoterScoreAsync(
      int appRating,
      int bandRating,
      string feedback,
      SourceOfNps sourceOfNps,
      CancellationToken cancellationToken)
    {
      Assert.ParamIsNotNull((object) sourceOfNps, nameof (sourceOfNps));
      bool wasManualEntry = sourceOfNps == SourceOfNps.Settings;
      string firmware = (string) null;
      BandClass? bandType = new BandClass?();
      if (this.userProfileService.IsRegisteredBandPaired)
      {
        try
        {
          bandType = new BandClass?(BandClass.Unknown);
          bandType = new BandClass?(await this.bandHardwareService.GetDeviceTypeAsync(cancellationToken));
          using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
            firmware = (await cargoConnection.GetBandFirmwareVersionAsync(cancellationToken)).ApplicationVersion.Version.ToString();
        }
        catch (Exception ex)
        {
          Logger.LogException(LogLevel.Error, ex, "Unable to retrieve Band Info for NPS Feedback.");
        }
      }
      if (appRating > 0)
        ApplicationTelemetry.LogNetPromoterScoreEntry(wasManualEntry, true, appRating, feedback, bandType, firmware);
      if (bandRating <= 0)
        return;
      ApplicationTelemetry.LogNetPromoterScoreEntry(wasManualEntry, false, bandRating, feedback, bandType, firmware);
    }

    public void PostNetPromoterScorePromptDismissal() => ApplicationTelemetry.LogNetPromoterScoreDismissed();
  }
}
