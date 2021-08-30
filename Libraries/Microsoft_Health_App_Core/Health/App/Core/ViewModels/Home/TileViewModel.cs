// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Home.TileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Home
{
  public abstract class TileViewModel : HealthViewModelBase, ILoadableParameters
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Home\\TileViewModel.cs");
    private readonly Lazy<HealthCommand> tileCommand;
    private bool isTileCommandEnabled = true;
    private TileViewModel.TileLoadState loadState;
    private bool wasNavigatedFrom;

    protected TileViewModel(TileViewModel.TileType type)
    {
      this.Type = type;
      this.UseTransitions = true;
      this.tileCommand = new Lazy<HealthCommand>((Func<HealthCommand>) (() => new HealthCommand(new Action(this.OnExecuteTileCommand), new Func<bool>(this.OnCanExecuteTileCommand))));
    }

    public TileViewModel.TileType Type { get; private set; }

    public bool UseTransitions { get; set; }

    public ICommand TileCommand => (ICommand) this.tileCommand.Value;

    protected bool IsTileCommandEnabled
    {
      get => this.isTileCommandEnabled;
      set
      {
        this.SetProperty<bool>(ref this.isTileCommandEnabled, value, nameof (IsTileCommandEnabled));
        if (!this.tileCommand.IsValueCreated)
          return;
        this.tileCommand.Value.RaiseCanExecuteChanged();
      }
    }

    public TileViewModel.TileLoadState LoadState
    {
      get => this.loadState;
      protected set => this.SetProperty<TileViewModel.TileLoadState>(ref this.loadState, value, nameof (LoadState));
    }

    public Task ShowLoadingMessageAsync() => this.TransitionToLoadStateAsync(TileViewModel.TileLoadState.Loading);

    public async Task LoadAsync(IDictionary<string, string> parameters = null)
    {
      try
      {
        await this.TransitionToLoadStateAsync(TileViewModel.TileLoadState.Loading);
        if (await this.LoadDataAsync(parameters))
          await this.TransitionToLoadStateAsync(TileViewModel.TileLoadState.Loaded);
        else
          await this.TransitionToLoadStateAsync(TileViewModel.TileLoadState.NoData);
      }
      catch (Exception ex)
      {
        TileViewModel.Logger.Error(ex, "Error loading tile data.");
        await this.TransitionToLoadStateAsync(TileViewModel.TileLoadState.Error, ex);
      }
    }

    public void NavigateTo()
    {
      this.OnNavigatedTo();
      if (!this.wasNavigatedFrom)
        return;
      this.wasNavigatedFrom = false;
      this.OnNavigatedBack();
    }

    public void NavigateFrom()
    {
      this.OnNavigatedFrom();
      this.wasNavigatedFrom = true;
    }

    public virtual bool WillOpenOnTileCommand() => true;

    protected virtual void OnNavigatedBack()
    {
    }

    protected virtual void OnNavigatedFrom()
    {
    }

    protected virtual void OnNavigatedTo()
    {
    }

    protected virtual void OnTileCommand()
    {
    }

    protected virtual Task<bool> LoadDataAsync(IDictionary<string, string> parameters = null) => Task.FromResult<bool>(true);

    protected virtual Task OnTransitionToLoadingStateAsync() => (Task) Task.FromResult<bool>(true);

    protected virtual Task OnTransitionToLoadedStateAsync() => (Task) Task.FromResult<bool>(true);

    protected virtual Task OnTransitionToNoDataStateAsync() => (Task) Task.FromResult<bool>(true);

    protected virtual Task OnTransitionToErrorStateAsync(Exception ex) => (Task) Task.FromResult<bool>(true);

    private async Task TransitionToLoadStateAsync(
      TileViewModel.TileLoadState newLoadState,
      Exception ex = null)
    {
      if (this.LoadState == newLoadState)
        return;
      switch (newLoadState)
      {
        case TileViewModel.TileLoadState.Loading:
          await this.OnTransitionToLoadingStateAsync();
          break;
        case TileViewModel.TileLoadState.Loaded:
          await this.OnTransitionToLoadedStateAsync();
          break;
        case TileViewModel.TileLoadState.NoData:
          await this.OnTransitionToNoDataStateAsync();
          break;
        case TileViewModel.TileLoadState.Error:
          await this.OnTransitionToErrorStateAsync(ex);
          break;
      }
      this.LoadState = newLoadState;
    }

    private void OnExecuteTileCommand() => this.OnTileCommand();

    private bool OnCanExecuteTileCommand() => this.IsTileCommandEnabled;

    public enum TileLoadState
    {
      Unknown,
      Loading,
      Loaded,
      NoData,
      Error,
    }

    public enum TileType
    {
      Metric,
      Split,
    }
  }
}
