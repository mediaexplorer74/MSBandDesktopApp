// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.ErrorHandlingServiceBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels.AddBand;
using Microsoft.Health.Cloud.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public abstract class ErrorHandlingServiceBase : IErrorHandlingService
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\ErrorHandlingServiceBase.cs");
    private readonly IMessageBoxService messageBoxService;
    private readonly INetworkService networkService;
    private readonly ISmoothNavService smoothNavService;
    private readonly IPermissionService permissionService;

    protected ErrorHandlingServiceBase(
      IMessageBoxService messageBoxService,
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IPermissionService permissionService)
    {
      this.messageBoxService = messageBoxService;
      this.networkService = networkService;
      this.smoothNavService = smoothNavService;
      this.permissionService = permissionService;
    }

    public async void HandleExceptions(Action action)
    {
      Exception exception = (Exception) null;
      try
      {
        action();
      }
      catch (Exception ex)
      {
        exception = ex;
      }
      if (exception == null)
        return;
      await this.HandleExceptionAsync(exception);
    }

    public async Task HandleExceptionsAsync(Func<Task> func)
    {
      Exception e = (Exception) null;
      try
      {
        await func();
      }
      catch (Exception ex)
      {
        e = ex;
      }
      if (e == null)
        return;
      await this.HandleExceptionAsync(e);
    }

    public async Task HandleExceptionAsync(Exception exception)
    {
      if (ErrorHandlingServiceBase.ShouldIgnoreException(exception))
        return;
      exception = ErrorHandlingServiceBase.GetRealException(exception);
      ErrorHandlingServiceBase.MessageInfo messageInfo = this.GetMessageInfo(exception);
      if (messageInfo == null)
        return;
      string exceptionType = ((object) exception).GetType().Name;
      string message = ErrorHandlingServiceBase.AddStackTraceIfNeeded(messageInfo.Body, exception);
      ErrorHandlingServiceBase.Logger.Debug((object) ("<START> Showing exception dialog for " + exceptionType));
      if (messageInfo is ErrorHandlingServiceBase.ActionableMessageInfo)
      {
        ErrorHandlingServiceBase.ActionableMessageInfo actionableMessageInfo = (ErrorHandlingServiceBase.ActionableMessageInfo) messageInfo;
        PortableMessageBoxResult? nullable1 = await this.messageBoxService.TryShowAsync(message, messageInfo.Title, PortableMessageBoxButton.OKCancel).ConfigureAwait(false);
        PortableMessageBoxResult? nullable2 = nullable1;
        PortableMessageBoxResult messageBoxResult1 = PortableMessageBoxResult.OK;
        if ((nullable2.GetValueOrDefault() == messageBoxResult1 ? (nullable2.HasValue ? 1 : 0) : 0) != 0)
          actionableMessageInfo.Action();
        if (actionableMessageInfo.TelemetryAction != null)
        {
          Action<bool> telemetryAction = actionableMessageInfo.TelemetryAction;
          nullable2 = nullable1;
          PortableMessageBoxResult messageBoxResult2 = PortableMessageBoxResult.OK;
          int num = nullable2.GetValueOrDefault() == messageBoxResult2 ? (nullable2.HasValue ? 1 : 0) : 0;
          telemetryAction(num != 0);
        }
        actionableMessageInfo = (ErrorHandlingServiceBase.ActionableMessageInfo) null;
      }
      else
      {
        PortableMessageBoxResult? nullable = await this.messageBoxService.TryShowAsync(message, messageInfo.Title, PortableMessageBoxButton.OK).ConfigureAwait(false);
      }
      ErrorHandlingServiceBase.Logger.Debug((object) ("<END> Showing exception dialog for " + exceptionType));
      exceptionType = (string) null;
      message = (string) null;
    }

    public async Task<bool> HandleExceptionWithRetryAsync(Exception exception, bool showCancel = true)
    {
      if (ErrorHandlingServiceBase.ShouldIgnoreException(exception))
        return false;
      exception = ErrorHandlingServiceBase.GetRealException(exception);
      ErrorHandlingServiceBase.MessageInfo messageInfo = this.GetMessageInfo(exception);
      if (messageInfo == null)
        return false;
      string exceptionType = ((object) exception).GetType().Name;
      string message = ErrorHandlingServiceBase.AddStackTraceIfNeeded(messageInfo.Body, exception);
      ErrorHandlingServiceBase.Logger.Debug((object) ("<START> Showing exception dialog for " + exceptionType));
      PortableMessageBoxResult messageBoxResult;
      if (showCancel)
        messageBoxResult = await this.messageBoxService.ShowAsync(message, messageInfo.Title, PortableMessageBoxButton.OKCancel, AppResources.RetryButton, AppResources.Cancel).ConfigureAwait(false);
      else
        messageBoxResult = await this.messageBoxService.ShowAsync(message, messageInfo.Title, PortableMessageBoxButton.OK, AppResources.RetryButton).ConfigureAwait(false);
      ErrorHandlingServiceBase.Logger.Debug((object) ("<END> Showing exception dialog for " + exceptionType));
      return messageBoxResult == PortableMessageBoxResult.OK;
    }

    public Task ShowBandErrorMessageAsync() => (Task) this.messageBoxService.ShowAsync(AppResources.BandErrorBody, AppResources.BandErrorTitle, PortableMessageBoxButton.OK);

    protected abstract PlatformErrorType GetErrorType(Exception exception);

    private static PlatformErrorType GetCommonErrorType(Exception exception)
    {
      switch (exception)
      {
        case TimeoutException _:
          return PlatformErrorType.Timeout;
        case BluetoothException _:
          return PlatformErrorType.Bluetooth;
        case BandSyncException _:
          return PlatformErrorType.BandSync;
        case BluetoothOffException _:
          return PlatformErrorType.BluetoothOff;
        case SingleDevicePolicyException _:
          return PlatformErrorType.SingleDevicePolicy;
        case HealthCloudServerException _:
          return PlatformErrorType.HealthCloudServer;
        case HttpRequestException _:
          return PlatformErrorType.HttpRequest;
        case WebException _:
          return PlatformErrorType.Web;
        case InternetException _:
          return PlatformErrorType.Internet;
        case DeactivatedException _:
          return PlatformErrorType.Deactivated;
        case OperationCanceledException _:
          return PlatformErrorType.OperationCanceled;
        case FirmwareUpdateInterruptedException _:
          return PlatformErrorType.FirmwareUpdateInterrupted;
        case TwoUpModeException _:
          return PlatformErrorType.BandFirmwareCorrupt;
        case PermissionDeniedException _:
          return PlatformErrorType.PermissionDenied;
        default:
          return PlatformErrorType.Unspecified;
      }
    }

    private static Exception GetRealException(Exception exception) => !(exception is AggregateException aggregateException) || aggregateException.InnerExceptions == null || aggregateException.InnerExceptions.Count == 0 ? exception : aggregateException.Flatten().InnerExceptions[0];

    private static string AddStackTraceIfNeeded(string message, Exception exception)
    {
      if (!EnvironmentUtilities.IsDebugSettingEnabled)
        return message;
      return message + Environment.NewLine + Environment.NewLine + (object) exception;
    }

    private static bool ShouldIgnoreException(Exception exception) => exception is IIgnorableException ignorableException && ignorableException.Suppress;

    public virtual ErrorHandlingServiceBase.MessageInfo GetMessageInfo(
      Exception exception)
    {
      PlatformErrorType platformErrorType = this.GetErrorType(exception);
      if (platformErrorType == PlatformErrorType.Unspecified)
        platformErrorType = ErrorHandlingServiceBase.GetCommonErrorType(exception);
      switch (platformErrorType)
      {
        case PlatformErrorType.BluetoothOff:
          return new ErrorHandlingServiceBase.MessageInfo(AppResources.BluetoothOffErrorTitle, AppResources.BluetoothOffErrorBody);
        case PlatformErrorType.Timeout:
          return new ErrorHandlingServiceBase.MessageInfo(AppResources.TimeoutErrorHeader, AppResources.TimeoutErrorBody);
        case PlatformErrorType.Bluetooth:
          return this.GetBluetoothMessageInfo();
        case PlatformErrorType.BandOperation:
          return this.GetBluetoothMessageInfo();
        case PlatformErrorType.BandIo:
          return this.GetBluetoothMessageInfo();
        case PlatformErrorType.BandLowBattery:
          return new ErrorHandlingServiceBase.MessageInfo(AppResources.SramFwUpdateBatteryTooLowErrorHeader, AppResources.SramFwUpdateBatteryTooLowErrorBody);
        case PlatformErrorType.BandSync:
          return this.GetBluetoothMessageInfo();
        case PlatformErrorType.HealthCloudServer:
          return new ErrorHandlingServiceBase.MessageInfo(AppResources.SystemErrorTitle, AppResources.SystemErrorBody);
        case PlatformErrorType.HttpRequest:
          return this.GetNetworkMessageInfo();
        case PlatformErrorType.Web:
          return this.GetNetworkMessageInfo();
        case PlatformErrorType.Internet:
          return this.GetNetworkMessageInfo();
        case PlatformErrorType.BandCloud:
          return this.GetNetworkMessageInfo();
        case PlatformErrorType.SingleDevicePolicy:
          return this.GetSingleDevicePolicyMessageInfo((SingleDevicePolicyException) exception);
        case PlatformErrorType.Deactivated:
          ErrorHandlingServiceBase.Logger.Debug((object) "<FLAG> handling deactivated exception as ignored");
          return (ErrorHandlingServiceBase.MessageInfo) null;
        case PlatformErrorType.OperationCanceled:
          return new ErrorHandlingServiceBase.MessageInfo(AppResources.ErrorTitleText, AppResources.GenericErrorMessage);
        case PlatformErrorType.FirmwareUpdateInterrupted:
          return new ErrorHandlingServiceBase.MessageInfo(AppResources.FirmwareUpdateMessageBoxGeneralHeader, AppResources.FirmwareUpdateMessageBoxInterruptedErrorMessage);
        case PlatformErrorType.PermissionDenied:
          return new ErrorHandlingServiceBase.MessageInfo(AppResources.PermissionErrorTitle, this.permissionService.GetPermissionError(((PermissionDeniedException) exception).DeniedFeatures));
        case PlatformErrorType.WebTile:
          return new ErrorHandlingServiceBase.MessageInfo(AppResources.OopsCaption, AppResources.UnknownErrorBody);
        case PlatformErrorType.BandFirmwareCorrupt:
          return (ErrorHandlingServiceBase.MessageInfo) new ErrorHandlingServiceBase.ActionableMessageInfo(AppResources.FirmwareUpdateMessageBoxRequiredUpdateHeader, AppResources.FirmwareUpdateMessageBoxRequiredUpdateMessage, (Action) (() =>
          {
            this.smoothNavService.Navigate(typeof (FirmwareUpdateViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
            {
              {
                "Type",
                "Normal"
              }
            });
            this.smoothNavService.ClearBackStack();
          }));
        case PlatformErrorType.TileOpen:
          return new ErrorHandlingServiceBase.MessageInfo(AppResources.WorkoutUploadFailureAndRetryTitle, AppResources.WorkoutUploadTileOpenFailureAndRetry);
        default:
          return new ErrorHandlingServiceBase.MessageInfo(AppResources.OopsCaption, AppResources.UnknownErrorBody);
      }
    }

    private ErrorHandlingServiceBase.MessageInfo GetNetworkMessageInfo() => this.networkService.IsInternetAvailable ? new ErrorHandlingServiceBase.MessageInfo(AppResources.NetworkErrorTitle, AppResources.NetworkErrorBody) : new ErrorHandlingServiceBase.MessageInfo(AppResources.NetworkErrorTitle, AppResources.InternetRequiredMessage);

    private ErrorHandlingServiceBase.MessageInfo GetBluetoothMessageInfo() => new ErrorHandlingServiceBase.MessageInfo(AppResources.BandErrorTitle, AppResources.BandErrorBody);

    private ErrorHandlingServiceBase.MessageInfo GetSingleDevicePolicyMessageInfo(
      SingleDevicePolicyException exception)
    {
      switch (exception.Result)
      {
        case SingleDevicePolicyResult.NoPairedDevices:
        case SingleDevicePolicyResult.DeviceNotAvailable:
          return new ErrorHandlingServiceBase.MessageInfo(AppResources.BandErrorTitle, AppResources.BandErrorBody);
        case SingleDevicePolicyResult.MultiplePairedDevices:
          return new ErrorHandlingServiceBase.MessageInfo(AppResources.SingleDevicePolicyMultiplePairedTitle, AppResources.SingleDevicePolicyMultiplePaired);
        case SingleDevicePolicyResult.MismatchedDevices:
          return new ErrorHandlingServiceBase.MessageInfo(AppResources.SingleDevicePolicyMismatchedTitle, AppResources.SingleDevicePolicyMismatched);
        case SingleDevicePolicyResult.DeviceNotReset:
          return new ErrorHandlingServiceBase.MessageInfo(AppResources.BandErrorTitle, AppResources.SingleDevicePolicyNotReset);
        case SingleDevicePolicyResult.DeviceInOobe:
          return (ErrorHandlingServiceBase.MessageInfo) new ErrorHandlingServiceBase.ActionableMessageInfo(AppResources.ApplicationTitle, AppResources.ReplaceBandMessageBody, (Action) (() => this.smoothNavService.Navigate(typeof (AddBandStartViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
          {
            {
              "IsOobe",
              bool.FalseString
            }
          })), new Action<bool>(ApplicationTelemetry.LogNewBandDetected));
        default:
          DebugUtilities.Fail("A policy result was not specifically handled.");
          return new ErrorHandlingServiceBase.MessageInfo(AppResources.BandErrorTitle, AppResources.BandErrorBody);
      }
    }

    public class MessageInfo
    {
      public MessageInfo(string title, string body)
      {
        this.Title = title;
        this.Body = body;
      }

      public string Title { get; private set; }

      public string Body { get; private set; }
    }

    protected class ActionableMessageInfo : ErrorHandlingServiceBase.MessageInfo
    {
      public ActionableMessageInfo(
        string title,
        string body,
        Action action,
        Action<bool> telemetryAction = null)
        : base(title, body)
      {
        this.Action = action;
        this.TelemetryAction = telemetryAction;
      }

      public Action Action { get; private set; }

      public Action<bool> TelemetryAction { get; private set; }
    }
  }
}
