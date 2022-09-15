// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.WeightProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  internal class WeightProvider : IWeightProvider
  {
    private readonly IHealthCloudClient healthCloudClient;

    public WeightProvider(IHealthCloudClient healthCloudClient) => this.healthCloudClient = healthCloudClient;

    public async Task<IList<WeightSensor>> GetAllWeightsAsync(
      CancellationToken cancellationToken)
    {
      return this.GetSortedWeights(await this.healthCloudClient.GetAllWeightsAsync(cancellationToken));
    }

    public async Task<IList<WeightSensor>> GetTopWeightsAsync(
      int count,
      CancellationToken cancellationToken)
    {
      return this.GetSortedWeights(await this.healthCloudClient.GetTopWeightsAsync(count, cancellationToken));
    }

    public async Task PostWeightAsync(
      WeightSensor weightSensor,
      CancellationToken cancellationToken)
    {
      await this.healthCloudClient.PostWeightsAsync(this.CreateUserWeight(weightSensor), cancellationToken);
    }

    public async Task DeleteWeightAsync(
      WeightSensor weightSensor,
      CancellationToken cancellationToken)
    {
      await this.healthCloudClient.DeleteWeightsAsync(this.CreateUserWeight(weightSensor), cancellationToken);
    }

    private IList<WeightSensor> GetSortedWeights(UserWeight userWeight)
    {
      List<WeightSensor> weightSensorList = new List<WeightSensor>();
      if (userWeight != null && userWeight.Weights != null)
        weightSensorList = userWeight.Weights.Select<Microsoft.Health.Cloud.Client.WeightSensor, WeightSensor>(new Func<Microsoft.Health.Cloud.Client.WeightSensor, WeightSensor>(WeightSensor.FromCloudModel)).OrderByDescending<WeightSensor, DateTimeOffset>((Func<WeightSensor, DateTimeOffset>) (p => p.Timestamp)).ToList<WeightSensor>();
      return (IList<WeightSensor>) weightSensorList;
    }

    private UserWeight CreateUserWeight(WeightSensor weightSensor)
    {
      Microsoft.Health.Cloud.Client.WeightSensor cloudModel = WeightSensor.ToCloudModel(weightSensor);
      IList<Microsoft.Health.Cloud.Client.WeightSensor> weightSensorList = (IList<Microsoft.Health.Cloud.Client.WeightSensor>) new List<Microsoft.Health.Cloud.Client.WeightSensor>();
      weightSensorList.Add(cloudModel);
      return new UserWeight() { Weights = weightSensorList };
    }
  }
}
