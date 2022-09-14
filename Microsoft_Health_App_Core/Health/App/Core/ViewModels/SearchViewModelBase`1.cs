// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.SearchViewModelBase`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Cirrious.MvvmCross.ViewModels;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public abstract class SearchViewModelBase<TResult> : PageViewModelBase
  {
    private readonly ISmoothNavService smoothNavService;
    private string query = string.Empty;
    private HealthCommand searchCommand;
    private HealthCommand backCommand;

    public event EventHandler NavigateFromStarted;

    protected SearchViewModelBase(
      ISmoothNavService smoothNavService,
      INetworkService networkService)
      : base(networkService)
    {
      Assert.ParamIsNotNull((object) smoothNavService, nameof (smoothNavService));
      this.smoothNavService = smoothNavService;
    }

    public bool ShowSearch => true;

    public string Query
    {
      get => this.query;
      set
      {
        this.SetProperty<string>(ref this.query, value, nameof (Query));
        ((MvxCommandBase) this.SearchCommand).RaiseCanExecuteChanged();
      }
    }

    private bool Searchable() => !string.IsNullOrWhiteSpace(this.Query);

    public ICommand SearchCommand => (ICommand) this.searchCommand ?? (ICommand) (this.searchCommand = new HealthCommand((Action) (() => this.smoothNavService.Navigate(typeof (TResult), this.GetSearchArguments(), NavigationStackAction.RemovePrevious)), new Func<bool>(this.Searchable)));

    public ICommand BackCommand => (ICommand) this.backCommand ?? (ICommand) (this.backCommand = new HealthCommand((Action) (() => this.smoothNavService.GoBack())));

    protected virtual IDictionary<string, string> GetSearchArguments() => (IDictionary<string, string>) new Dictionary<string, string>()
    {
      ["query"] = this.Query
    };

    protected override void OnNavigateFromStarted()
    {
      base.OnNavigateFromStarted();
      EventHandler navigateFromStarted = this.NavigateFromStarted;
      if (navigateFromStarted == null)
        return;
      navigateFromStarted((object) this, EventArgs.Empty);
    }
  }
}
