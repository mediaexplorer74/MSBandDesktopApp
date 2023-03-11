// MessagingSettings.cs
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
  public partial class MessagingSettings : SyncAppPageControl
  {
    private Guid tileId;
 
    public MessagingSettings(AppMainWindow main, SyncAppPageControl parent, Guid id)
      : base(main, parent, true)
    {
      this.tileId = id;
      this.InitializeComponent();
    }

    private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        ((TextBoxBase)sender).SelectAll();
    }

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
      
  }
}
