// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.MessagingSettings
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
using System.Windows.Input;
using System.Windows.Markup;

namespace DesktopSyncApp
{
  public partial class MessagingSettings : SyncAppPageControl, IComponentConnector
  {
    private Guid tileId;
    //internal TextBlock Title;
    //internal TextBlock NotificationsText;
    //internal CheckBox NotificationsOn;
    //internal TextBlock QuickResponse;
    //internal TextBox Reply1;
    //internal TextBox Reply2;
    //internal TextBox Reply3;
    //internal TextBox Reply4;
    //private bool _contentLoaded;

    public MessagingSettings(AppMainWindow main, SyncAppPageControl parent, Guid id)
      : base(main, parent, true)
    {
      this.tileId = id;
      this.InitializeComponent();
    }

    private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) => ((TextBoxBase) sender).SelectAll();

    private void Save_Click(object sender, RoutedEventArgs e)
    {
      this.parent.Model.StrapManager.GetStrap(this.tileId).SaveReplies();
      ((TileManagementControl) this.container).ShowSettingsPage((SyncAppPageControl) null);
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
      this.parent.Model.StrapManager.GetStrap(this.tileId).CancelReplies();
      ((TileManagementControl) this.container).ShowSettingsPage((SyncAppPageControl) null);
    }

        /*
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft Band Sync;component/pagecontrols/tilesettingspagecontrols/messagingsettings.xaml", UriKind.Relative));
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
          this.NotificationsOn = (CheckBox) target;
          break;
        case 4:
          this.QuickResponse = (TextBlock) target;
          break;
        case 5:
          this.Reply1 = (TextBox) target;
          this.Reply1.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.TextBox_GotKeyboardFocus);
          break;
        case 6:
          this.Reply2 = (TextBox) target;
          this.Reply2.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.TextBox_GotKeyboardFocus);
          break;
        case 7:
          this.Reply3 = (TextBox) target;
          this.Reply3.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.TextBox_GotKeyboardFocus);
          break;
        case 8:
          this.Reply4 = (TextBox) target;
          this.Reply4.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.TextBox_GotKeyboardFocus);
          break;
        case 9:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.Save_Click);
          break;
        case 10:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.Cancel_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
        */
  }
}
