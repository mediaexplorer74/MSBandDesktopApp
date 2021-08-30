// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfCourseFilterCategoryViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Messages;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.AppBar;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  [PageTaxonomy(new string[] {"Fitness", "Golf", "Filters"})]
  public sealed class GolfCourseFilterCategoryViewModel : PageViewModelBase
  {
    internal const string FilterCategoryKey = "FilterCategory";
    private readonly ISmoothNavService smoothNavService;
    private readonly IMessageSender messageSender;
    private readonly IGolfCourseFilterService golfCourseFilterService;
    private ICommand navigateToFilterCommand;
    private ICommand clearAllCommand;
    private ICommand confirmCommand;
    private ICommand cancelCommand;
    private IEnumerable<GolfCourseFilterViewModel> filters;
    private bool showClearAll;

    public IEnumerable<GolfCourseFilterViewModel> Filters
    {
      get => this.filters;
      private set => this.SetProperty<IEnumerable<GolfCourseFilterViewModel>>(ref this.filters, value, nameof (Filters));
    }

    public bool ShowClearAll
    {
      get => this.showClearAll;
      private set => this.SetProperty<bool>(ref this.showClearAll, value, nameof (ShowClearAll));
    }

    public ICommand NavigateToFilterCommand => this.navigateToFilterCommand ?? (this.navigateToFilterCommand = (ICommand) new HealthCommand<GolfCourseFilterViewModel>((Action<GolfCourseFilterViewModel>) (filter => this.smoothNavService.Navigate(typeof (GolfCourseFilterSubcategoryViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "FilterCategory",
        filter.Category.ToString()
      }
    }))));

    public ICommand ClearAllCommand => this.clearAllCommand ?? (this.clearAllCommand = (ICommand) new HealthCommand((Action) (() =>
    {
      this.golfCourseFilterService.ClearFilters();
      this.LoadFilters(this.golfCourseFilterService.PendingFilters);
    })));

    public ICommand ConfirmCommand => this.confirmCommand ?? (this.confirmCommand = (ICommand) new HealthCommand((Action) (() =>
    {
      this.golfCourseFilterService.EnabledFilters = (IEnumerable<GolfCourseFilter>) new List<GolfCourseFilter>(this.Filters.Select<GolfCourseFilterViewModel, GolfCourseFilter>((Func<GolfCourseFilterViewModel, GolfCourseFilter>) (p => new GolfCourseFilter()
      {
        Category = p.Category,
        Items = (IList<GolfCourseFilterItem>) p.GetRawItems().Select<GolfCourseFilterItemViewModel, GolfCourseFilterItem>((Func<GolfCourseFilterItemViewModel, GolfCourseFilterItem>) (s => new GolfCourseFilterItem()
        {
          Subcategory = s.Subcategory,
          IsSelected = s.IsSelected
        })).ToList<GolfCourseFilterItem>()
      })));
      this.golfCourseFilterService.ResetFilters();
      this.smoothNavService.GoBack();
    })));

    public ICommand CancelCommand => this.cancelCommand ?? (this.cancelCommand = (ICommand) new HealthCommand((Action) (() =>
    {
      this.golfCourseFilterService.ResetFilters();
      this.smoothNavService.GoBack();
    })));

    protected override void OnNavigatedTo()
    {
      base.OnNavigatedTo();
      this.messageSender.Register<BackButtonPressedMessage>((object) this, new Action<BackButtonPressedMessage>(this.OnBackKeyPressed));
    }

    protected override void OnBackNavigation()
    {
      base.OnBackNavigation();
      this.LoadFilters(this.golfCourseFilterService.PendingFilters);
      this.ShowAppBar();
    }

    private void OnBackKeyPressed(BackButtonPressedMessage message) => this.golfCourseFilterService.ResetFilters();

    public GolfCourseFilterCategoryViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IMessageSender messageSender,
      IGolfCourseFilterService golfCourseFilterService)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.messageSender = messageSender;
      this.golfCourseFilterService = golfCourseFilterService;
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.golfCourseFilterService.ResetFilters();
      this.LoadFilters(this.golfCourseFilterService.EnabledFilters);
      this.ShowAppBar();
      return (Task) Task.FromResult<bool>(true);
    }

    private void LoadFilters(IEnumerable<GolfCourseFilter> filterSelection)
    {
      this.Filters = (IEnumerable<GolfCourseFilterViewModel>) filterSelection.Select<GolfCourseFilter, GolfCourseFilterViewModel>((Func<GolfCourseFilter, GolfCourseFilterViewModel>) (golfCourseFilter => new GolfCourseFilterViewModel(golfCourseFilter.Category, (IList<GolfCourseFilterItemViewModel>) new List<GolfCourseFilterItemViewModel>(golfCourseFilter.Items.Select<GolfCourseFilterItem, GolfCourseFilterItemViewModel>((Func<GolfCourseFilterItem, GolfCourseFilterItemViewModel>) (s => new GolfCourseFilterItemViewModel(s.Subcategory, s.IsSelected))))))).ToList<GolfCourseFilterViewModel>();
      this.ShowClearAll = this.Filters.Any<GolfCourseFilterViewModel>((Func<GolfCourseFilterViewModel, bool>) (p => p.Items.Any<GolfCourseFilterItemViewModel>((Func<GolfCourseFilterItemViewModel, bool>) (s => s.IsSelected))));
    }

    private void ShowAppBar() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
    {
      new AppBarButton(AppResources.LabelConfirmLocked, AppBarIcon.Accept, this.ConfirmCommand),
      new AppBarButton(AppResources.LabelCancelLocked, AppBarIcon.Cancel, this.CancelCommand)
    });
  }
}
