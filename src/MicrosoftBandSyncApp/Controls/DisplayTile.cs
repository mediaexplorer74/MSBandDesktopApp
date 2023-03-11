// DisplayTile.cs
// Type: DesktopSyncApp.DisplayTile
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
using System.Windows.Media.Imaging;

namespace DesktopSyncApp
{
  public partial class DisplayTile : UserControl
  {

    public DisplayTile(BitmapImage bi, Color background)
    {
      this.InitializeComponent();
      this.TileIcon.Source = (ImageSource) bi;
      this.OuterGrid.Background = (Brush) new SolidColorBrush(background);
    }

    public DisplayTile(WriteableBitmap bi, Color background)
    {
      this.InitializeComponent();
      this.TileIcon.Source = (ImageSource) bi;
      this.OuterGrid.Background = (Brush) new SolidColorBrush(background);
    }  
  }
}
