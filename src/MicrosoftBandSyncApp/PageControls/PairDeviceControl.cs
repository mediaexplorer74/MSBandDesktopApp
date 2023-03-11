// PairDeviceControl.cs
// Type: DesktopSyncApp.PairDeviceControl
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

namespace DesktopSyncApp
{
  public partial class PairDeviceControl : SyncAppPageControl
  {

    public PairDeviceControl(AppMainWindow parent)
      : base(parent, false)
    {
      this.InitializeComponent();
    }
            
  }
}
