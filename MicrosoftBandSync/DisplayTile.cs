// Decompiled with JetBrains decompiler
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
  public class DisplayTile : UserControl, IComponentConnector
  {
    internal Grid OuterGrid;
    internal Grid OpacityGrid;
    internal Image TileIcon;
    private bool _contentLoaded;

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

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft Band Sync;component/controls/displaytile.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.OuterGrid = (Grid) target;
          break;
        case 2:
          this.OpacityGrid = (Grid) target;
          break;
        case 3:
          this.TileIcon = (Image) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
