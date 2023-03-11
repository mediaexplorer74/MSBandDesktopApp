// BusySpinner.cs
// Type: DesktopSyncApp.BusySpinner
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
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DesktopSyncApp
{
  public partial class BusySpinner : UserControl  
  {

        public BusySpinner()
        {
            this.InitializeComponent();
        }

        public void StartAnimation() => ((Storyboard) this.Resources[(object) "BallAnimation01"]).Begin();

    public void StopAnimation() => ((Storyboard) this.Resources[(object) "BallAnimation01"]).Stop();

    private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      BusySpinner busySpinner = (BusySpinner) sender;
      if ((bool) e.NewValue)
        busySpinner.StartAnimation();
      else
        busySpinner.StopAnimation();
    }

   
  }
}
