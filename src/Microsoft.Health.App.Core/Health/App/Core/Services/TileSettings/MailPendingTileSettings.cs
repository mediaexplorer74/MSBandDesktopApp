// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.TileSettings.MailPendingTileSettings
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.TileSettings
{
  public class MailPendingTileSettings : PendingTileSettings
  {
    private readonly IConfig config;
    private IList<EmailAddress> vips;
    private bool areVipsEnabled;

    public MailPendingTileSettings(IConfig config) => this.config = config;

    public IList<EmailAddress> Vips
    {
      get => this.vips;
      set
      {
        this.vips = value;
        this.IsChanged = true;
      }
    }

    public bool AreVipsEnabled
    {
      get => this.areVipsEnabled;
      set
      {
        this.areVipsEnabled = value;
        this.IsChanged = true;
      }
    }

    public override Task LoadSettingsAsync(CancellationToken token)
    {
      this.vips = this.config.EmailVips ?? (IList<EmailAddress>) new List<EmailAddress>();
      this.areVipsEnabled = this.config.AreEmailVipsEnabled;
      return (Task) Task.FromResult<bool>(true);
    }

    public override Task ApplyChangesAsync()
    {
      this.config.EmailVips = this.Vips;
      this.config.AreEmailVipsEnabled = this.AreVipsEnabled;
      ApplicationTelemetry.LogVipListChange(this.Vips.Count);
      return (Task) Task.FromResult<bool>(true);
    }
  }
}
