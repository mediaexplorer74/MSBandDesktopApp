// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.EditExerciseTagsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.TileSettings;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  public class EditExerciseTagsViewModel : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\TileSettings\\EditExerciseTagsViewModel.cs");
    private readonly ISmoothNavService smoothNavService;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly ITileManagementService tileManagementService;
    private HealthCommand cancelCommand;
    private HealthCommand confirmCommand;
    private ExercisePendingTileSettings pendingTileSettings;
    private IList<CustomTagViewModel> customExerciseTags;
    private IList<ExerciseTag> baseRawCustomTags;
    private CustomTagViewModel customTag1;
    private CustomTagViewModel customTag2;
    private CustomTagViewModel customTag3;
    private CustomTagViewModel customTag4;
    private CustomTagViewModel customTag5;

    public EditExerciseTagsViewModel(
      ISmoothNavService smoothNavService,
      INetworkService networkService,
      IErrorHandlingService errorHandlingService,
      ITileManagementService tileManagementService)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.errorHandlingService = errorHandlingService;
      this.tileManagementService = tileManagementService;
    }

    public CustomTagViewModel CustomTag1
    {
      get => this.customTag1;
      private set => this.SetProperty<CustomTagViewModel>(ref this.customTag1, value, nameof (CustomTag1));
    }

    public CustomTagViewModel CustomTag2
    {
      get => this.customTag2;
      private set => this.SetProperty<CustomTagViewModel>(ref this.customTag2, value, nameof (CustomTag2));
    }

    public CustomTagViewModel CustomTag3
    {
      get => this.customTag3;
      private set => this.SetProperty<CustomTagViewModel>(ref this.customTag3, value, nameof (CustomTag3));
    }

    public CustomTagViewModel CustomTag4
    {
      get => this.customTag4;
      private set => this.SetProperty<CustomTagViewModel>(ref this.customTag4, value, nameof (CustomTag4));
    }

    public CustomTagViewModel CustomTag5
    {
      get => this.customTag5;
      private set => this.SetProperty<CustomTagViewModel>(ref this.customTag5, value, nameof (CustomTag5));
    }

    public ICommand ConfirmCommand => (ICommand) this.confirmCommand ?? (ICommand) (this.confirmCommand = new HealthCommand(new Action(this.Confirm)));

    public ICommand CancelCommand => (ICommand) this.cancelCommand ?? (ICommand) (this.cancelCommand = new HealthCommand(new Action(this.Cancel)));

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      EditExerciseTagsViewModel.Logger.Debug((object) "<START> loading edit exercise tags page");
      try
      {
        EditExerciseTagsViewModel exerciseTagsViewModel = this;
        ExercisePendingTileSettings pendingTileSettings = exerciseTagsViewModel.pendingTileSettings;
        ExercisePendingTileSettings pendingSettingsAsync = await this.tileManagementService.GetPendingSettingsAsync<ExercisePendingTileSettings>();
        exerciseTagsViewModel.pendingTileSettings = pendingSettingsAsync;
        exerciseTagsViewModel = (EditExerciseTagsViewModel) null;
        this.baseRawCustomTags = (IList<ExerciseTag>) new List<ExerciseTag>(this.pendingTileSettings.ExerciseTags.Where<ExerciseTag>((Func<ExerciseTag, bool>) (tag => !tag.IsDefault)));
        List<CustomTagViewModel> list = this.baseRawCustomTags.Select<ExerciseTag, CustomTagViewModel>((Func<ExerciseTag, CustomTagViewModel>) (p => new CustomTagViewModel(p.Text, p))).ToList<CustomTagViewModel>();
        this.customExerciseTags = this.GetDefaultCustomTags();
        for (int index = 0; index < list.Count && index < 5; ++index)
          this.customExerciseTags[index] = list[index];
        this.CustomTag1 = this.customExerciseTags[0];
        this.CustomTag2 = this.customExerciseTags[1];
        this.CustomTag3 = this.customExerciseTags[2];
        this.CustomTag4 = this.customExerciseTags[3];
        this.CustomTag5 = this.customExerciseTags[4];
        this.ShowAppBar();
        EditExerciseTagsViewModel.Logger.Debug((object) "<END> loading edit exercise tags page");
      }
      catch (Exception ex)
      {
        EditExerciseTagsViewModel.Logger.Error(ex, "<FAILED> loading edit exercise tags page");
        await this.errorHandlingService.HandleExceptionAsync(ex);
        this.smoothNavService.GoBack();
      }
    }

    private void Confirm()
    {
      IList<ExerciseTag> exerciseTags = this.pendingTileSettings.ExerciseTags;
      foreach (ExerciseTag baseRawCustomTag in (IEnumerable<ExerciseTag>) this.baseRawCustomTags)
        exerciseTags.Remove(baseRawCustomTag);
      foreach (ExerciseTag exerciseTag in this.customExerciseTags.Select<CustomTagViewModel, ExerciseTag>((Func<CustomTagViewModel, ExerciseTag>) (p => p.RawExerciseTag)).Where<ExerciseTag>((Func<ExerciseTag, bool>) (tag => !string.IsNullOrEmpty(tag.Text) && !string.IsNullOrWhiteSpace(tag.Text))))
        exerciseTags.Add(exerciseTag);
      this.pendingTileSettings.ExerciseTags = exerciseTags;
      this.smoothNavService.GoBack();
    }

    private void Cancel()
    {
      this.baseRawCustomTags = (IList<ExerciseTag>) null;
      this.customExerciseTags = (IList<CustomTagViewModel>) null;
      this.smoothNavService.GoBack();
    }

    private void ShowAppBar() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
    {
      new AppBarButton(AppResources.LabelConfirm, AppBarIcon.Accept, this.ConfirmCommand),
      new AppBarButton(AppResources.LabelCancel, AppBarIcon.Cancel, this.CancelCommand)
    });

    public IList<CustomTagViewModel> GetDefaultCustomTags()
    {
      ObservableCollection<CustomTagViewModel> observableCollection = new ObservableCollection<CustomTagViewModel>();
      observableCollection.Add(EditExerciseTagsViewModel.CreateDefaultCustomTag());
      observableCollection.Add(EditExerciseTagsViewModel.CreateDefaultCustomTag());
      observableCollection.Add(EditExerciseTagsViewModel.CreateDefaultCustomTag());
      observableCollection.Add(EditExerciseTagsViewModel.CreateDefaultCustomTag());
      observableCollection.Add(EditExerciseTagsViewModel.CreateDefaultCustomTag());
      return (IList<CustomTagViewModel>) observableCollection;
    }

    private static CustomTagViewModel CreateDefaultCustomTag() => new CustomTagViewModel(string.Empty, new ExerciseTag()
    {
      ExerciseTypeId = Guid.Empty,
      Flags = 0U,
      Algorithm = HrAlgorithm.RunDefault,
      IsDefault = false,
      IsChecked = true
    });
  }
}
