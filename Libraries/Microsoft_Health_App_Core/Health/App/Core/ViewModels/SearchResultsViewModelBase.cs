// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.SearchResultsViewModelBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public abstract class SearchResultsViewModelBase : PageViewModelBase
  {
    private string searchTitle;
    private string resultsTitle;
    private bool isFilteringEnabled;
    private bool isFiltered;

    public string SearchTitle
    {
      get => this.searchTitle;
      protected set => this.SetProperty<string>(ref this.searchTitle, value, nameof (SearchTitle));
    }

    public string ResultsTitle
    {
      get => this.resultsTitle;
      protected set => this.SetProperty<string>(ref this.resultsTitle, value, nameof (ResultsTitle));
    }

    public bool IsFilteringEnabled
    {
      get => this.isFilteringEnabled;
      protected set => this.SetProperty<bool>(ref this.isFilteringEnabled, value, nameof (IsFilteringEnabled));
    }

    public bool IsFiltered
    {
      get => this.isFiltered;
      protected set => this.SetProperty<bool>(ref this.isFiltered, value, nameof (IsFiltered));
    }

    public virtual bool ShowSearch => false;

    public abstract ICommand OpenFiltersCommand { get; }

    protected SearchResultsViewModelBase(INetworkService networkService)
      : base(networkService)
    {
    }
  }
}
