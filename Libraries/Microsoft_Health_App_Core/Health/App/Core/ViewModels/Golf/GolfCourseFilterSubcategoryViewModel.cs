// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfCourseFilterSubcategoryViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

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
  public sealed class GolfCourseFilterSubcategoryViewModel : PageViewModelBase
  {
    private readonly ISmoothNavService smoothNavService;
    private readonly IGolfCourseFilterService golfCourseFilterService;
    private GolfCourseFilterCategory category;
    private ICommand confirmCommand;
    private ICommand cancelCommand;
    private IEnumerable<GolfCourseFilterItemViewModel> subfilters;

    public GolfCourseFilterSubcategoryViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IGolfCourseFilterService golfCourseFilterService)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.golfCourseFilterService = golfCourseFilterService;
    }

    public IEnumerable<GolfCourseFilterItemViewModel> Subfilters
    {
      get => this.subfilters;
      private set => this.SetProperty<IEnumerable<GolfCourseFilterItemViewModel>>(ref this.subfilters, value, nameof (Subfilters));
    }

    public GolfCourseFilterCategory Category
    {
      get => this.category;
      private set => this.SetProperty<GolfCourseFilterCategory>(ref this.category, value, nameof (Category));
    }

    public ICommand ConfirmCommand => this.confirmCommand ?? (this.confirmCommand = (ICommand) new HealthCommand((Action) (() =>
    {
      this.golfCourseFilterService.PendingFilters.First<GolfCourseFilter>((Func<GolfCourseFilter, bool>) (p => p.Category == this.Category)).Items = (IList<GolfCourseFilterItem>) new List<GolfCourseFilterItem>(this.Subfilters.Select<GolfCourseFilterItemViewModel, GolfCourseFilterItem>((Func<GolfCourseFilterItemViewModel, GolfCourseFilterItem>) (p => new GolfCourseFilterItem()
      {
        Subcategory = p.Subcategory,
        IsSelected = p.IsSelected
      })));
      this.smoothNavService.GoBack();
    })));

    public ICommand CancelCommand => this.cancelCommand ?? (this.cancelCommand = (ICommand) new HealthCommand((Action) (() => this.smoothNavService.GoBack())));

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.Category = this.GetEnumParameter<GolfCourseFilterCategory>("FilterCategory");
      this.Subfilters = (IEnumerable<GolfCourseFilterItemViewModel>) new List<GolfCourseFilterItemViewModel>(this.golfCourseFilterService.PendingFilters.First<GolfCourseFilter>((Func<GolfCourseFilter, bool>) (p => p.Category == this.Category)).Items.Select<GolfCourseFilterItem, GolfCourseFilterItemViewModel>((Func<GolfCourseFilterItem, GolfCourseFilterItemViewModel>) (s => new GolfCourseFilterItemViewModel(s.Subcategory, s.IsSelected))));
      this.ShowAppBar();
      return (Task) Task.FromResult<bool>(true);
    }

    private void ShowAppBar() => this.AppBar = new Microsoft.Health.App.Core.Models.AppBar.AppBar(new AppBarButton[2]
    {
      new AppBarButton(AppResources.LabelConfirmLocked, AppBarIcon.Accept, this.ConfirmCommand),
      new AppBarButton(AppResources.LabelCancelLocked, AppBarIcon.Cancel, this.CancelCommand)
    });
  }
}
