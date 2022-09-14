// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.ExerciseSettingsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.TileSettings;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  public class ExerciseSettingsViewModel : SettingsViewModelBase<ExercisePendingTileSettings>
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\TileSettings\\ExerciseSettingsViewModel.cs");
    private readonly ISmoothNavService navService;
    private readonly IErrorHandlingService errorHandlingService;
    private IList<ExerciseTagViewModel> exerciseTags;

    public ExerciseSettingsViewModel(
      INetworkService networkService,
      ITileManagementService tileManager,
      IErrorHandlingService errorHandlingService,
      ISmoothNavService navService,
      IBandConnectionFactory cargoConnectionFactory)
      : base(networkService, tileManager, errorHandlingService, navService, cargoConnectionFactory)
    {
      this.errorHandlingService = errorHandlingService;
      this.navService = navService;
    }

    public override string TileGuid => "a708f02a-03cd-4da0-bb33-be904e6a2924";

    public IList<ExerciseTagViewModel> ExerciseTags
    {
      get => this.exerciseTags;
      set => this.SetProperty<IList<ExerciseTagViewModel>>(ref this.exerciseTags, value, nameof (ExerciseTags));
    }

    public void SaveTagsToPendingSettings() => this.PendingTileSettings.ExerciseTags = (IList<ExerciseTag>) this.ExerciseTags.Select<ExerciseTagViewModel, ExerciseTag>((Func<ExerciseTagViewModel, ExerciseTag>) (p => p.RawExerciseTag)).ToList<ExerciseTag>();

    protected override async Task LoadSettingsDataAsync(IDictionary<string, string> parameters = null)
    {
      ExerciseSettingsViewModel.Logger.Debug((object) "<START> loading exercise settings page");
      try
      {
        this.Header = AppResources.ExerciseSettingsHeader;
        this.Subheader = AppResources.ExerciseSettingsSubheader;
        this.ExerciseTags = (IList<ExerciseTagViewModel>) new List<ExerciseTagViewModel>();
        this.ExerciseTags = (IList<ExerciseTagViewModel>) this.PendingTileSettings.ExerciseTags.Select<ExerciseTag, ExerciseTagViewModel>((Func<ExerciseTag, ExerciseTagViewModel>) (p => new ExerciseTagViewModel(this, p.Text, p.IsChecked, p))).ToList<ExerciseTagViewModel>();
        ExerciseSettingsViewModel.Logger.Debug((object) "<END> loading exercise settings page");
      }
      catch (Exception ex)
      {
        ExerciseSettingsViewModel.Logger.Error(ex, "<FAILED> loading exercise settings page");
        await this.errorHandlingService.HandleExceptionAsync(ex);
        this.navService.GoBack();
      }
    }

    protected override void OnBackNavigation()
    {
      this.ExerciseTags = (IList<ExerciseTagViewModel>) new List<ExerciseTagViewModel>(this.PendingTileSettings.ExerciseTags.Select<ExerciseTag, ExerciseTagViewModel>((Func<ExerciseTag, ExerciseTagViewModel>) (p => new ExerciseTagViewModel(this, p.Text, p.IsChecked, p))));
      base.OnBackNavigation();
    }

    protected override void OnEdit() => this.navService.Navigate(typeof (EditExerciseTagsViewModel));
  }
}
