// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Home.DesignNavViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Devices;
using Microsoft.Health.App.Core.ViewModels.Workouts;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Home
{
  public class DesignNavViewModel
  {
    public ICommand SyncCommand => (ICommand) null;

    public IList<DeviceSyncTime> LastSyncTimes
    {
      get => (IList<DeviceSyncTime>) new List<DeviceSyncTime>()
      {
        new DeviceSyncTime(DeviceType.Band, new DateTimeOffset?(new DateTimeOffset(2015, 3, 10, 12, 32, 48, TimeSpan.FromHours(7.0)))),
        new DeviceSyncTime(DeviceType.Phone, new DateTimeOffset?(new DateTimeOffset(2015, 3, 10, 12, 32, 8, TimeSpan.FromHours(7.0))))
      };
      set => throw new NotImplementedException();
    }

    public IList<object> NavChoices => (IList<object>) new List<object>()
    {
      (object) DesignNavViewModel.CreateNavChoice<TilesViewModel>(AppResources.NavigationHome, "\uE129"),
      (object) DesignNavViewModel.CreateNavChoice<HistoryViewModel>(AppResources.NavigationHistory, "\uE050"),
      (object) new NavSectionHeaderViewModel("Test"),
      (object) DesignNavViewModel.CreateNavChoice<SettingsProfileViewModel>(AppResources.NavigationProfile, "\uE173"),
      (object) DesignNavViewModel.CreateNavChoice<WorkoutPlanLandingViewModel>(AppResources.NavigationBrowseWorkouts, "\uE003")
    };

    private static NavChoiceViewModel CreateNavChoice<T>(string name, string icon) => DesignNavViewModel.CreateNavChoice(name, icon, typeof (T));

    private static NavChoiceViewModel CreateNavChoice(
      string name,
      string icon,
      Type viewModelType)
    {
      return new NavChoiceViewModel(ServiceLocator.Current.GetInstance<IMessageBoxService>(), ServiceLocator.Current.GetInstance<ILauncherService>(), ServiceLocator.Current.GetInstance<ISmoothNavService>(), ServiceLocator.Current.GetInstance<IMessageSender>())
      {
        Name = name,
        GlyphIcon = icon,
        Page = viewModelType,
        IsEnabled = true
      };
    }
  }
}
