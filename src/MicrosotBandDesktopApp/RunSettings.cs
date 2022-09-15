// Decompiled with JetBrains decompiler
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
  public class RunSettings : SyncAppPageControl, IComponentConnector
  {
    private RunSettingsManager manager;
    internal TextBlock Title;
    internal TextBlock NotificationsText;
    internal TextBlock NotificationsText1;
    internal TextBlock MainDataGrouping;
    internal TextBlock Metric1Title;
    internal TextBlock Metric2Title;
    internal TextBlock Metric3Title;
    internal ComboBox Metric1Data;
    internal ComboBox Metric2Data;
    internal ComboBox Metric3Data;
    internal TextBlock Metric1Error;
    internal TextBlock Metric2Error;
    internal TextBlock Metric3Error;
    internal TextBlock DrawerDataGrouping;
    internal TextBlock Metric4Title;
    internal TextBlock Metric5Title;
    internal TextBlock Metric6Title;
    internal TextBlock Metric7Title;
    internal ComboBox Metric4Data;
    internal ComboBox Metric5Data;
    internal ComboBox Metric6Data;
    internal ComboBox Metric7Data;
    internal TextBlock Metric4Error;
    internal TextBlock Metric5Error;
    internal TextBlock Metric6Error;
    internal TextBlock Metric7Error;
    private bool _contentLoaded;

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

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft Band Sync;component/pagecontrols/tilesettingspagecontrols/runsettings.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.Title = (TextBlock) target;
          break;
        case 2:
          this.NotificationsText = (TextBlock) target;
          break;
        case 3:
          this.NotificationsText1 = (TextBlock) target;
          break;
        case 4:
          this.MainDataGrouping = (TextBlock) target;
          break;
        case 5:
          this.Metric1Title = (TextBlock) target;
          break;
        case 6:
          this.Metric2Title = (TextBlock) target;
          break;
        case 7:
          this.Metric3Title = (TextBlock) target;
          break;
        case 8:
          this.Metric1Data = (ComboBox) target;
          break;
        case 9:
          this.Metric2Data = (ComboBox) target;
          break;
        case 10:
          this.Metric3Data = (ComboBox) target;
          break;
        case 11:
          this.Metric1Error = (TextBlock) target;
          break;
        case 12:
          this.Metric2Error = (TextBlock) target;
          break;
        case 13:
          this.Metric3Error = (TextBlock) target;
          break;
        case 14:
          this.DrawerDataGrouping = (TextBlock) target;
          break;
        case 15:
          this.Metric4Title = (TextBlock) target;
          break;
        case 16:
          this.Metric5Title = (TextBlock) target;
          break;
        case 17:
          this.Metric6Title = (TextBlock) target;
          break;
        case 18:
          this.Metric7Title = (TextBlock) target;
          break;
        case 19:
          this.Metric4Data = (ComboBox) target;
          break;
        case 20:
          this.Metric5Data = (ComboBox) target;
          break;
        case 21:
          this.Metric6Data = (ComboBox) target;
          break;
        case 22:
          this.Metric7Data = (ComboBox) target;
          break;
        case 23:
          this.Metric4Error = (TextBlock) target;
          break;
        case 24:
          this.Metric5Error = (TextBlock) target;
          break;
        case 25:
          this.Metric6Error = (TextBlock) target;
          break;
        case 26:
          this.Metric7Error = (TextBlock) target;
          break;
        case 27:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.Save_Click);
          break;
        case 28:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.Cancel_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
