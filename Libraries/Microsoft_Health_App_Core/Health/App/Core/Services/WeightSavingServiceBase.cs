// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.WeightSavingServiceBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.Cloud.Client;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public abstract class WeightSavingServiceBase : IWeightSavingService
  {
    private readonly IMessageSender messageSender;
    private readonly IDateTimeService dateTimeService;
    private readonly IUserProfileService userProfileService;
    private readonly IWeightProvider weightProvider;

    protected WeightSavingServiceBase(
      IMessageSender messageSender,
      IDateTimeService dateTimeService,
      IUserProfileService userProfileService,
      IWeightProvider weightProvider)
    {
      this.messageSender = messageSender;
      this.dateTimeService = dateTimeService;
      this.userProfileService = userProfileService;
      this.weightProvider = weightProvider;
    }

    public bool IsMetric => this.userProfileService.MassUnitType == MassUnitType.Metric;

    public abstract Task OpenSaveWeightAsync(Weight currentWeight);

    public async Task SaveWeightAsync(Weight weight)
    {
      Microsoft.Health.App.Core.Providers.WeightSensor newWeight = new Microsoft.Health.App.Core.Providers.WeightSensor(this.dateTimeService.Now, weight);
      await this.weightProvider.PostWeightAsync(newWeight, CancellationToken.None);
      this.messageSender.Send<WeightChangedMessage>(new WeightChangedMessage(newWeight));
      ApplicationTelemetry.LogWeightAdded(true);
    }
  }
}
