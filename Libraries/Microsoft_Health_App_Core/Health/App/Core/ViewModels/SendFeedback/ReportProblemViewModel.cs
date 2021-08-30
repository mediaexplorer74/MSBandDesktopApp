// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.SendFeedback.ReportProblemViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Models.Diagnostics;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.SendFeedback
{
  [PageTaxonomy(new string[] {"App", "Feedback", "Description"})]
  public class ReportProblemViewModel : PageViewModelBase
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\SendFeedback\\ReportProblemViewModel.cs");
    protected const int MaxFeedbackImages = 5;
    private readonly IReportProblemStore reportProblemStore;
    private readonly ISmoothNavService smoothNavService;
    private readonly IErrorHandlingService errorHandlingService;
    private readonly IEnvironmentService environmentService;
    private readonly IMessageBoxService messageBoxService;
    private readonly IFilePickerService filePickerService;
    private readonly IPermissionService permissionService;
    private string description;
    private ICommand backCommand;
    private ICommand editDescriptionCommand;
    private ICommand pickImagesCommand;
    private HealthCommand<IFile> removeImageCommand;
    private ICommand nextCommand;

    public string Title => this.reportProblemStore.Title ?? AppResources.ReportProblemPageTitle;

    public IList<IFile> Images { get; private set; }

    public string Description
    {
      get => !string.IsNullOrEmpty(this.description) ? this.description : AppResources.ReportProblemDescriptionTextPlaceholder;
      set => this.SetProperty<string>(ref this.description, value, nameof (Description));
    }

    public ReportProblemViewModel(
      INetworkService networkService,
      IReportProblemStore reportProblemStore,
      ISmoothNavService smoothNavService,
      IErrorHandlingService errorHandlingService,
      IEnvironmentService environmentService,
      IMessageBoxService messageBoxService,
      IFilePickerService filePickerService,
      IPermissionService permissionService)
      : base(networkService)
    {
      this.reportProblemStore = reportProblemStore;
      this.smoothNavService = smoothNavService;
      this.errorHandlingService = errorHandlingService;
      this.environmentService = environmentService;
      this.messageBoxService = messageBoxService;
      this.filePickerService = filePickerService;
      this.permissionService = permissionService;
      if (this.reportProblemStore.UserFeedback == null)
        this.reportProblemStore.UserFeedback = new DiagnosticsUserFeedback();
      this.Images = (IList<IFile>) new ObservableCollection<IFile>((IEnumerable<IFile>) this.reportProblemStore.ImageFiles);
    }

    protected override void OnNavigatedTo() => this.Description = this.reportProblemStore?.UserFeedback?.Text ?? string.Empty;

    protected override void OnNavigatedFrom()
    {
      if (this.reportProblemStore?.UserFeedback == null)
        return;
      this.reportProblemStore.UserFeedback.Text = this.description;
    }

    public ICommand BackCommand => this.backCommand ?? (this.backCommand = (ICommand) new HealthCommand((Action) (() => this.smoothNavService.GoBack())));

    public ICommand EditDescriptionCommand => this.editDescriptionCommand ?? (this.editDescriptionCommand = (ICommand) new HealthCommand((Action) (() => this.smoothNavService.Navigate(typeof (ReportProblemEditDescriptionViewModel)))));

    public ICommand PickImagesCommand => this.pickImagesCommand ?? (this.pickImagesCommand = (ICommand) AsyncHealthCommand.Create((Func<Task>) (async () =>
    {
      try
      {
        await this.permissionService.RequestPermissionsAsync(FeaturePermissions.Feedback);
        await this.AddImagesAsync((IEnumerable<IFile>) await this.filePickerService.PickImagesAsync());
      }
      catch (Exception ex)
      {
        ReportProblemViewModel.Logger.Error((object) "Could not pick images.", ex);
        await this.errorHandlingService.HandleExceptionAsync(ex);
      }
    })));

    public ICommand RemoveImageCommand => (ICommand) this.removeImageCommand ?? (ICommand) (this.removeImageCommand = new HealthCommand<IFile>((Action<IFile>) (async imageFile =>
    {
      this.Images.Remove(imageFile);
      this.reportProblemStore.ImageFiles.Remove(imageFile);
      try
      {
        await imageFile.DeleteAsync();
      }
      catch (Exception ex)
      {
        ReportProblemViewModel.Logger.Warn((object) "Could not delete user added image.", ex);
      }
    })));

    public ICommand NextCommand => this.nextCommand ?? (this.nextCommand = (ICommand) AsyncHealthCommand.Create((Func<Task>) (async () =>
    {
      try
      {
        await this.permissionService.RequestPermissionsAsync(FeaturePermissions.Feedback);
        this.reportProblemStore.ImageFiles.Clear();
        foreach (IFile image in (IEnumerable<IFile>) this.Images)
          this.reportProblemStore.ImageFiles.Add(image);
        this.smoothNavService.Navigate(typeof (ReportProblemConfirmViewModel));
      }
      catch (PermissionDeniedException ex)
      {
        await this.errorHandlingService.HandleExceptionAsync((Exception) ex);
      }
    })));

    private async Task AddImagesAsync(IEnumerable<IFile> imageFiles)
    {
      foreach (IFile imageFile in imageFiles)
      {
        IFile file = imageFile;
        if (this.Images.Count >= 5)
        {
          int num = (int) await this.messageBoxService.ShowAsync(AppResources.ReportProblemMaxImageAmountExceededMessage, AppResources.ReportProblemMaxImageAmountExceededMessageHeader, PortableMessageBoxButton.OK);
          break;
        }
        this.Images.Add(file);
        file = (IFile) null;
      }
    }
  }
}
