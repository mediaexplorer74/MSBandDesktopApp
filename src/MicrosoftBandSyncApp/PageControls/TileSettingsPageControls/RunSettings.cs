// RunSettings.cs
// Type: DesktopSyncApp.RunSettings
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace DesktopSyncApp
{
  public partial class RunSettings : SyncAppPageControl
  {
    private RunSettingsManager manager;    

    public RunSettings(AppMainWindow main, SyncAppPageControl parent, RunSettingsManager manager)
      : base(main, parent, true)
    {
      this.InitializeComponent();
      this.manager = manager;
      this.DataContext = (object) manager;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
      if (!this.manager.ValidateRunMetrics())
        return;
      this.manager.SaveRunMetrics();
      ((TileManagementControl) this.container).ShowSettingsPage((SyncAppPageControl) null);
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
      this.manager.CancelRunMetrics();
      ((TileManagementControl) this.container).ShowSettingsPage((SyncAppPageControl) null);
    }
   
  }
}
