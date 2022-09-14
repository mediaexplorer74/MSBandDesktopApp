// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.WeightTracking.WeightTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.Utilities.Logging;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.WeightTracking
{
  [PageTaxonomy(new string[] {"Weight"})]
  public class WeightTileViewModel : MetricTileViewModel
  {
    private static readonly IActivityManager ActivityManager = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\WeightTracking\\WeightTileViewModel.cs").CreateActivityManager();
    private readonly IMessageSender messageSender;
    private readonly IFormattingService formattingService;
    private readonly IWeightProvider weightProvider;
    private readonly IDispatchService dispatchService;
    private IList<Microsoft.Health.App.Core.Providers.WeightSensor> previousTwoWeights;

    public WeightTileViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IMessageSender messageSender,
      IFormattingService formattingService,
      IWeightProvider weightProvider,
      IDispatchService dispatchService,
      WeightOneMonthViewModel weightOneMonthViewModel,
      WeightThreeMonthViewModel weightThreeMonthViewModel,
      WeightAllViewModel weightAllViewModel,
      TileFirstTimeUseViewModel firtTimeUseViewModel)
      : base(networkService, smoothNavService, messageSender, firtTimeUseViewModel)
    {
      this.messageSender = messageSender;
      this.formattingService = formattingService;
      this.weightProvider = weightProvider;
      this.dispatchService = dispatchService;
      this.FirstTimeUse.IsSupported = false;
      this.TileIcon = "\uE203";
      this.ColorLevel = TileColorLevel.Medium;
      this.Pivots.Add(new PivotDefinition(AppResources.PivotOneMonth, (object) weightOneMonthViewModel));
      this.Pivots.Add(new PivotDefinition(AppResources.PivotThreeMonth, (object) weightThreeMonthViewModel));
      this.Pivots.Add(new PivotDefinition(AppResources.PivotAll, (object) weightAllViewModel));
      this.messageSender.Register<WeightChangedMessage>((object) this.messageSender, (Action<WeightChangedMessage>) (async message =>
      {
        List<Func<Task>> loadingFuncs = new List<Func<Task>>()
        {
          (Func<Task>) (() => this.UpdateHeaderAsync(message.WeightSensor))
        };
        if (this.IsOpen)
          loadingFuncs.AddRange(this.Pivots.Select<PivotDefinition, object>((Func<PivotDefinition, object>) (pivotDefinition => pivotDefinition.Content)).OfType<PanelViewModelBase>().Select<PanelViewModelBase, Func<Task>>((Func<PanelViewModelBase, Func<Task>>) (panelViewModel => (Func<Task>) (() => panelViewModel.LoadAsync((IDictionary<string, string>) null)))));
        await this.dispatchService.RunOnUIThreadAsync((Func<Task>) (async () => await Task.WhenAll(loadingFuncs.Select<Func<Task>, Task>((Func<Func<Task>, Task>) (p => p())))));
      }));
    }

    protected override Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null) => WeightTileViewModel.ActivityManager.RunAsActivityAsync<bool>(Level.Debug, (Func<string>) (() => "Load Weight Tile"), (Func<Task<bool>>) (async () => await this.GetWeightsAsync()), true);

    protected override async Task OnTransitionToLoadedStateAsync()
    {
      await base.OnTransitionToLoadedStateAsync();
      this.CanOpen = true;
      this.LoadHeader();
    }

    private async Task<bool> GetWeightsAsync()
    {
      using (CancellationTokenSource tcs = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
      {
        WeightTileViewModel weightTileViewModel = this;
        IList<Microsoft.Health.App.Core.Providers.WeightSensor> previousTwoWeights = weightTileViewModel.previousTwoWeights;
        IList<Microsoft.Health.App.Core.Providers.WeightSensor> topWeightsAsync = await this.weightProvider.GetTopWeightsAsync(2, tcs.Token);
        weightTileViewModel.previousTwoWeights = topWeightsAsync;
        weightTileViewModel = (WeightTileViewModel) null;
      }
      return this.previousTwoWeights != null;
    }

    private async Task UpdateHeaderAsync(Microsoft.Health.App.Core.Providers.WeightSensor newWeight)
    {
      if (newWeight != null && this.previousTwoWeights != null)
      {
        this.previousTwoWeights.Add(newWeight);
        this.previousTwoWeights = (IList<Microsoft.Health.App.Core.Providers.WeightSensor>) this.previousTwoWeights.OrderByDescending<Microsoft.Health.App.Core.Providers.WeightSensor, DateTimeOffset>((Func<Microsoft.Health.App.Core.Providers.WeightSensor, DateTimeOffset>) (w => w.Timestamp)).Take<Microsoft.Health.App.Core.Providers.WeightSensor>(2).ToList<Microsoft.Health.App.Core.Providers.WeightSensor>();
      }
      else
      {
        int num = await this.GetWeightsAsync() ? 1 : 0;
      }
      this.LoadHeader();
    }

    private void LoadHeader()
    {
      if (this.previousTwoWeights == null || this.previousTwoWeights.Count < 2)
      {
        this.Subheader = AppResources.WeightTileNoContent;
        this.Header = (StyledSpan) null;
        this.HeaderIcon = (string) null;
      }
      else
      {
        Microsoft.Health.App.Core.Providers.WeightSensor previousTwoWeight1 = this.previousTwoWeights[0];
        Microsoft.Health.App.Core.Providers.WeightSensor previousTwoWeight2 = this.previousTwoWeights[1];
        Weight weight = previousTwoWeight1.Weight - previousTwoWeight2.Weight;
        this.HeaderIcon = weight.TotalGrams > 0.0 ? "\uE201" : "\uE202";
        this.Header = this.formattingService.FormatWeight(new Weight?(weight));
        this.Subheader = this.formattingService.FormatTimeIntervalMonthDay(previousTwoWeight2.Timestamp, previousTwoWeight1.Timestamp);
      }
    }
  }
}
