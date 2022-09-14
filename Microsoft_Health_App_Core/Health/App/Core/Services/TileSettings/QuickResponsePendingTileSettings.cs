// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.TileSettings.QuickResponsePendingTileSettings
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.TileSettings
{
  public class QuickResponsePendingTileSettings : PendingTileSettings
  {
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\TileSettings\\QuickResponsePendingTileSettings.cs");
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private string[] messageResponses;
    private string[] phoneResponses;

    public QuickResponsePendingTileSettings(IBandConnectionFactory cargoConnectionFactory) => this.cargoConnectionFactory = cargoConnectionFactory;

    public string[] MessageResponses
    {
      get => this.messageResponses;
      set
      {
        this.MessageResponsesChanged = true;
        this.IsChanged = true;
        this.messageResponses = value;
      }
    }

    public string[] PhoneResponses
    {
      get => this.phoneResponses;
      set
      {
        this.PhoneResponsesChanged = true;
        this.IsChanged = true;
        this.phoneResponses = value;
      }
    }

    public bool MessageResponsesChanged { get; private set; }

    public bool PhoneResponsesChanged { get; private set; }

    public override async Task LoadSettingsAsync(CancellationToken token)
    {
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(token))
      {
        string[] smsResponsesAsync = await cargoConnection.GetSmsResponsesAsync(token);
        this.messageResponses = smsResponsesAsync == null ? QuickResponsePendingTileSettings.GetDefaultMessageResponses() : smsResponsesAsync;
        string[] callResponsesAsync = await cargoConnection.GetPhoneCallResponsesAsync(token);
        this.phoneResponses = callResponsesAsync == null ? QuickResponsePendingTileSettings.GetDefaultPhoneResponses() : callResponsesAsync;
      }
    }

    public override async Task ApplyChangesAsync()
    {
      if (this.MessageResponses != null && this.MessageResponsesChanged)
      {
        try
        {
          using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(CancellationToken.None))
          {
            await cargoConnection.SetSmsResponsesAsync(this.MessageResponses[0], this.MessageResponses[1], this.MessageResponses[2], this.MessageResponses[3], CancellationToken.None);
            this.MessageResponsesChanged = false;
            ApplicationTelemetry.LogMessagingCustomResponseChange();
          }
        }
        catch (Exception ex)
        {
          QuickResponsePendingTileSettings.Logger.Error(ex, "Failed to set SMS responses");
          throw;
        }
      }
      if (this.PhoneResponses == null)
        return;
      if (!this.PhoneResponsesChanged)
        return;
      try
      {
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(CancellationToken.None))
        {
          await cargoConnection.SetPhoneCallResponsesAsync(this.PhoneResponses[0], this.PhoneResponses[1], this.PhoneResponses[2], this.PhoneResponses[3], CancellationToken.None);
          this.PhoneResponsesChanged = false;
          ApplicationTelemetry.LogCallCustomResponseChange();
        }
      }
      catch (Exception ex)
      {
        QuickResponsePendingTileSettings.Logger.Error(ex, "Failed to set Phone responses");
        throw;
      }
    }

    public static string[] GetDefaultMessageResponses() => new string[4]
    {
      AppResources.DefaultMessageCannedResponses1,
      AppResources.DefaultMessageCannedResponses2,
      string.Empty,
      string.Empty
    };

    public static string[] GetDefaultPhoneResponses() => new string[4]
    {
      AppResources.DefaultPhoneCannedResponses1,
      AppResources.DefaultPhoneCannedResponses2,
      AppResources.DefaultPhoneCannedResponses3,
      string.Empty
    };
  }
}
