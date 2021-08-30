// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.CacheItemsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Caching;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  public class CacheItemsViewModel : PageViewModelBase
  {
    private readonly IDebuggableHttpCacheService cacheService;
    private IList<CacheItemViewModel> cacheItems;
    private string headerText;
    private HealthCommand searchCommand;
    private string searchText;

    public CacheItemsViewModel(
      INetworkService networkService,
      IDebuggableHttpCacheService cacheService)
      : base(networkService)
    {
      this.cacheService = cacheService;
    }

    public string HeaderText
    {
      get => this.headerText;
      set => this.SetProperty<string>(ref this.headerText, value, nameof (HeaderText));
    }

    public IList<CacheItemViewModel> CacheItems => this.cacheItems;

    public string SearchText
    {
      get => this.searchText;
      set => this.SetProperty<string>(ref this.searchText, value, nameof (SearchText));
    }

    public ICommand SearchCommand => (ICommand) this.searchCommand ?? (ICommand) (this.searchCommand = new HealthCommand(new Action(((PanelViewModelBase) this).Refresh)));

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      IList<IHttpCacheItem> allAsync = await this.cacheService.GetAllAsync();
      if (string.IsNullOrWhiteSpace(this.SearchText))
      {
        this.cacheItems = (IList<CacheItemViewModel>) allAsync.Select<IHttpCacheItem, CacheItemViewModel>((Func<IHttpCacheItem, CacheItemViewModel>) (i => new CacheItemViewModel(i))).ToList<CacheItemViewModel>();
        this.HeaderText = this.cacheItems.Count.ToString() + " items in cache";
      }
      else
      {
        string lowerInvariant = this.SearchText.ToLowerInvariant();
        this.cacheItems = (IList<CacheItemViewModel>) new List<CacheItemViewModel>();
        foreach (IHttpCacheItem httpCacheItem in (IEnumerable<IHttpCacheItem>) allAsync)
        {
          if (httpCacheItem.Url.ToLowerInvariant().Contains(lowerInvariant))
            this.cacheItems.Add(new CacheItemViewModel(httpCacheItem));
        }
        this.HeaderText = string.Format("Showing {0}/{1} items", new object[2]
        {
          (object) this.cacheItems.Count,
          (object) allAsync.Count
        });
      }
      this.RaisePropertyChanged("CacheItems");
    }
  }
}
