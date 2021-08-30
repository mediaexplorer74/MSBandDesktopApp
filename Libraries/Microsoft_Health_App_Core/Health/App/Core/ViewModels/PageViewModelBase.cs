// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.PageViewModelBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels
{
  public abstract class PageViewModelBase : PanelViewModelBase
  {
    private PageNavigationState navigationState;
    private Microsoft.Health.App.Core.Models.AppBar.AppBar appBar;

    protected PageViewModelBase(INetworkService networkService)
      : base(networkService)
    {
      this.SupportsChanges = true;
    }

    public bool IsActive => this.navigationState == PageNavigationState.NavigatingTo || this.navigationState == PageNavigationState.Current || this.navigationState == PageNavigationState.NavigatingFrom;

    public PageNavigationState NavigationState
    {
      get => this.navigationState;
      set
      {
        switch (this.navigationState)
        {
          case PageNavigationState.Uninitialized:
            switch (value)
            {
              case PageNavigationState.Uninitialized:
              case PageNavigationState.NavigatingFrom:
              case PageNavigationState.Inactive:
                this.ThrowInvalidStateTransition(value);
                break;
              case PageNavigationState.NavigatingTo:
              case PageNavigationState.Current:
                this.navigationState = value;
                this.OnNavigatedTo();
                break;
              default:
                throw new ArgumentOutOfRangeException(nameof (value));
            }
            break;
          case PageNavigationState.NavigatingTo:
            if (value == PageNavigationState.Current)
            {
              this.navigationState = value;
              break;
            }
            this.ThrowInvalidStateTransition(value);
            break;
          case PageNavigationState.Current:
            if (value == PageNavigationState.NavigatingFrom)
            {
              this.navigationState = value;
              this.OnNavigateFromStarted();
              break;
            }
            this.ThrowInvalidStateTransition(value);
            break;
          case PageNavigationState.NavigatingFrom:
            if (value == PageNavigationState.Inactive)
            {
              this.navigationState = value;
              this.OnNavigatedFrom();
              break;
            }
            this.ThrowInvalidStateTransition(value);
            break;
          case PageNavigationState.Inactive:
            if (value == PageNavigationState.NavigatingTo)
            {
              this.navigationState = value;
              this.OnNavigatedTo();
              this.OnBackNavigation();
              break;
            }
            this.ThrowInvalidStateTransition(value);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
        this.RaisePropertyChanged(nameof (NavigationState));
      }
    }

    public virtual bool HeaderOpen => true;

    public Microsoft.Health.App.Core.Models.AppBar.AppBar AppBar
    {
      get => this.appBar;
      set => this.SetProperty<Microsoft.Health.App.Core.Models.AppBar.AppBar>(ref this.appBar, value, nameof (AppBar));
    }

    public bool SupportsChanges { get; protected set; }

    public virtual void OnResume()
    {
      if (this.LoadState != LoadState.Error && this.LoadState != LoadState.CustomError)
        return;
      this.Refresh();
    }

    protected virtual void OnNavigatedTo()
    {
    }

    protected virtual void OnNavigateFromStarted()
    {
    }

    protected virtual void OnNavigatedFrom()
    {
    }

    protected virtual void OnBackNavigation()
    {
    }

    public virtual Task ChangeAsync(IDictionary<string, string> parameters = null)
    {
      this.Parameters = parameters;
      return (Task) Task.FromResult<object>((object) null);
    }

    private void ThrowInvalidStateTransition(PageNavigationState newState)
    {
      if (!ServiceLocator.Current.GetInstance<IEnvironmentService>().IsPublicRelease)
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Invalid page transition: {0} to {1}", new object[2]
        {
          (object) this.navigationState,
          (object) newState
        }));
    }
  }
}
