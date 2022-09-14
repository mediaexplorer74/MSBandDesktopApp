// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.PanelViewModelBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public abstract class PanelViewModelBase : HealthViewModelBase, ILoadableParameters
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\PanelViewModelBase.cs");
    private static readonly IActivityManager ActivityManager = PanelViewModelBase.Logger.CreateActivityManager();
    private readonly INetworkService networkService;
    private string loadErrorMessage;
    private Exception loadError;
    private Task loadTask;
    private Task reloadTask;
    private HealthCommand refreshCommand;
    private bool loadAsyncInProgress;
    private bool reloadPending;
    private IDictionary<string, string> pendingReloadParameters;
    private LoadState loadState;

    protected PanelViewModelBase(INetworkService networkService)
    {
      this.networkService = networkService;
      this.ViewState = (IDictionary<string, object>) new Dictionary<string, object>();
    }

    public virtual bool UseLoadPanel => true;

    public IDictionary<string, object> ViewState { get; set; }

    protected IDictionary<string, string> Parameters { get; set; }

    public LoadState LoadState
    {
      get => this.loadState;
      set => this.SetProperty<LoadState>(ref this.loadState, value, nameof (LoadState));
    }

    public ICommand RefreshCommand => (ICommand) this.refreshCommand ?? (ICommand) (this.refreshCommand = new HealthCommand(new Action(this.Refresh)));

    public Exception LoadError
    {
      get => this.loadError;
      set => this.SetProperty<Exception>(ref this.loadError, value, nameof (LoadError));
    }

    public string CustomLoadErrorMessage
    {
      get
      {
        if (string.IsNullOrWhiteSpace(this.loadErrorMessage))
          this.loadErrorMessage = AppResources.NoDataMessage;
        return this.loadErrorMessage;
      }
      private set => this.SetProperty<string>(ref this.loadErrorMessage, value, nameof (CustomLoadErrorMessage));
    }

    public Task LoadAsync(IDictionary<string, string> parameters = null)
    {
      if (this.loadAsyncInProgress)
      {
        this.pendingReloadParameters = parameters;
        if (this.reloadPending)
        {
          if (this.reloadTask != null)
            return this.reloadTask;
          PanelViewModelBase.Logger.Warn((object) "Reload task could not be found. Returning immediately.");
          return (Task) Task.FromResult<object>((object) null);
        }
        this.reloadTask = this.ReloadInternalAsync();
        return this.reloadTask;
      }
      this.loadTask = this.LoadInternalAsync(parameters);
      return this.loadTask;
    }

    private async Task ReloadInternalAsync()
    {
      this.reloadPending = true;
      if (this.loadTask != null)
        await this.loadTask;
      else
        PanelViewModelBase.Logger.Warn((object) "Load task could not be found. Continuing with reload.");
      this.reloadPending = false;
      this.reloadTask = (Task) null;
      this.loadTask = this.LoadInternalAsync(this.pendingReloadParameters);
      this.pendingReloadParameters = (IDictionary<string, string>) null;
      await this.loadTask;
    }

    private Task LoadInternalAsync(IDictionary<string, string> parameters = null)
    {
      if (parameters == null)
        parameters = (IDictionary<string, string>) new Dictionary<string, string>();
      return PanelViewModelBase.ActivityManager.RunAsActivityAsync(Level.Info, (Func<string>) (() => string.Format("Load {0}", new object[1]
      {
        (object) this.GetType().Name
      })), (Func<IDictionary<string, object>>) (() => (IDictionary<string, object>) parameters.ToDictionary<KeyValuePair<string, string>, string, object>((Func<KeyValuePair<string, string>, string>) (pair => pair.Key), (Func<KeyValuePair<string, string>, object>) (pair => (object) pair.Value))), (Func<Task>) (async () =>
      {
        this.Parameters = parameters;
        this.loadAsyncInProgress = true;
        this.LoadState = LoadState.Loading;
        try
        {
          await this.LoadDataAsync(parameters);
          this.LoadState = LoadState.Loaded;
        }
        catch (NoDataException ex)
        {
          this.SetErrorMessage();
          this.LoadState = LoadState.CustomError;
        }
        catch (CustomErrorException ex)
        {
          this.CustomLoadErrorMessage = ex.Message;
          this.LoadState = LoadState.CustomError;
        }
        catch (Exception ex)
        {
          if (this.networkService.IsInternetAvailable)
          {
            PanelViewModelBase.Logger.Error(ex, "Error loading panel data.");
            this.LoadError = ex;
            this.LoadState = LoadState.Error;
          }
          else
          {
            PanelViewModelBase.Logger.Error(ex, "Could not load panel data, internet not available.");
            this.CustomLoadErrorMessage = AppResources.InternetRequiredMessage;
            this.LoadState = LoadState.CustomError;
          }
        }
        this.loadAsyncInProgress = false;
      }));
    }

    protected virtual Task LoadDataAsync(IDictionary<string, string> parameters = null) => (Task) Task.FromResult<object>((object) null);

    public virtual async Task RefreshAsync() => await this.LoadAsync(this.Parameters);

    public async void Refresh() => await this.RefreshAsync();

    protected void SetErrorMessage()
    {
      if (this.networkService.IsInternetAvailable)
        this.CustomLoadErrorMessage = AppResources.NoDataMessage;
      else
        this.CustomLoadErrorMessage = AppResources.InternetRequiredMessage;
    }

    protected string GetStringParameter(string parameterName)
    {
      if (this.Parameters == null)
        throw new ArgumentException("Parameters cannot be null.");
      return this.Parameters.ContainsKey(parameterName) ? this.Parameters[parameterName] : throw new ArgumentException(parameterName + " parameter is required.");
    }

    protected int GetIntParameter(string parameterName)
    {
      int result;
      if (!int.TryParse(this.GetStringParameter(parameterName), out result))
        throw new ArgumentException(parameterName + " must be an integer.");
      return result;
    }

    protected double GetDoubleParameter(string parameterName)
    {
      double result;
      if (!double.TryParse(this.GetStringParameter(parameterName), out result))
        throw new ArgumentException(parameterName + " must be a double.");
      return result;
    }

    protected T GetEnumParameter<T>(string parameterName) where T : struct
    {
      string stringParameter = this.GetStringParameter(parameterName);
      if (!typeof (T).GetTypeInfo().IsEnum)
        throw new ArgumentException(parameterName + " must be an enum.");
      T obj;
      ref T local = ref obj;
      if (!Enum.TryParse<T>(stringParameter, out local))
        throw new ArgumentException(parameterName + " must be an enum of the provided type.");
      return obj;
    }

    public static string GetPanelName(object panel) => PanelViewModelBase.GetPanelName(panel.GetType());

    public static string GetPanelName(Type type)
    {
      string str1 = type.Name;
      string str2 = "PanelViewModel";
      string str3 = "PageViewModel";
      string str4 = "ViewModel";
      if (str1.EndsWith(str2))
        str1 = str1.Substring(0, str1.Length - str2.Length);
      else if (str1.EndsWith(str3))
        str1 = str1.Substring(0, str1.Length - str3.Length);
      else if (str1.EndsWith(str4))
        str1 = str1.Substring(0, str1.Length - str4.Length);
      return str1;
    }
  }
}
