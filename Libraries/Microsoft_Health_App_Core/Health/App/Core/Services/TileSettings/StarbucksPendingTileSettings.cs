// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.TileSettings.StarbucksPendingTileSettings
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.TileSettings
{
  public class StarbucksPendingTileSettings : PendingTileSettings
  {
    private readonly IConfig config;
    private string cardNumber;

    public StarbucksPendingTileSettings(IConfig config) => this.config = config;

    public string CardNumber
    {
      get => this.cardNumber;
      set
      {
        this.cardNumber = value;
        this.IsChanged = true;
      }
    }

    public override Task LoadSettingsAsync(CancellationToken token)
    {
      this.cardNumber = this.config.StarbucksCardNumber ?? string.Empty;
      return (Task) Task.FromResult<bool>(true);
    }

    public override Task ApplyChangesAsync()
    {
      this.config.StarbucksCardNumber = this.CardNumber;
      return (Task) Task.FromResult<bool>(true);
    }
  }
}
