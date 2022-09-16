// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.NotificationSettings
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace DesktopSyncApp
{
  public partial class NotificationSettings : SyncAppPageControl, IComponentConnector
  {
    //internal TextBlock Title;
    //internal TextBlock NotificationsText;
    //internal CheckBox NotificationsOn;
    //private bool _contentLoaded;

    public NotificationSettings(AppMainWindow main, SyncAppPageControl parent)
      : base(main, parent, true)
    {
      this.InitializeComponent();
    }

    private void Close_Click(object sender, RoutedEventArgs e) => ((TileManagementControl) this.container).ShowSettingsPage((SyncAppPageControl) null);

        /*
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft Band Sync;component/pagecontrols/tilesettingspagecontrols/notificationsettings.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

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
          this.NotificationsOn = (CheckBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
        */
  }
}
