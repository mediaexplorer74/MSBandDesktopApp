// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.LaunchSoftwareUpdateControl
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
  public partial class LaunchSoftwareUpdateControl : SyncAppPageControl, IComponentConnector
  {
    //internal Image Logo;
    //private bool _contentLoaded;

    public LaunchSoftwareUpdateControl(AppMainWindow parent)
      : base(parent, false)
    {
      this.InitializeComponent();
    }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            this.pageControl.HideModalPage();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.pageControl.HideModalPage();
        }

        /*
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft Band Sync;component/pagecontrols/launchsoftwareupdate.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.Logo = (Image) target;
      else
        this._contentLoaded = true;
    }
        */
    }
}
