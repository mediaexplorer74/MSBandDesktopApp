// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.TileSettings.AppNotificationViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;

namespace Microsoft.Health.App.Core.ViewModels.TileSettings
{
  public class AppNotificationViewModel : HealthObservableObject
  {
    private readonly NotificationCenterSettingsViewModel parentViewModel;
    private bool enabled;

    public AppNotificationViewModel(
      NotificationCenterSettingsViewModel parentViewModel,
      string id,
      string displayName,
      bool enabled)
    {
      this.parentViewModel = parentViewModel;
      this.Id = id;
      this.DisplayName = displayName;
      this.enabled = enabled;
    }

    public string Id { get; }

    public string DisplayName { get; }

    public ArgbColor32 TextColor => !this.Enabled ? new ArgbColor32(4287137928U) : new ArgbColor32(uint.MaxValue);

    public bool Enabled
    {
      get => this.enabled;
      set
      {
        this.SetProperty<bool>(ref this.enabled, value, nameof (Enabled));
        this.RaisePropertyChanged("TextColor");
        this.parentViewModel.SaveToPendingSettings();
      }
    }
  }
}
