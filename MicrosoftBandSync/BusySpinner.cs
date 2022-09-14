// Decompiled with JetBrains decompiler
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
  public class BusySpinner : UserControl, IComponentConnector
  {
    internal RotateTransform RotationTransform_01;
    internal RotateTransform RotationTransform_02;
    internal RotateTransform RotationTransform_03;
    internal RotateTransform RotationTransform_04;
    internal RotateTransform RotationTransform_05;
    private bool _contentLoaded;

    public BusySpinner() => this.InitializeComponent();

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

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft Band Sync;component/controls/busyspinner.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((UIElement) target).IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.UserControl_IsVisibleChanged);
          break;
        case 2:
          this.RotationTransform_01 = (RotateTransform) target;
          break;
        case 3:
          this.RotationTransform_02 = (RotateTransform) target;
          break;
        case 4:
          this.RotationTransform_03 = (RotateTransform) target;
          break;
        case 5:
          this.RotationTransform_04 = (RotateTransform) target;
          break;
        case 6:
          this.RotationTransform_05 = (RotateTransform) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
