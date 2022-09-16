// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.Fader
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
using System.Windows.Media.Animation;

namespace DesktopSyncApp
{
  //[ContentProperty("Content")]
  public partial class Fader : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty FadedVisibilityProperty = DependencyProperty.Register(nameof (FadedVisibility), typeof (Visibility), typeof (Fader), new PropertyMetadata((object) Visibility.Visible, new PropertyChangedCallback(Fader.FadedVisibilityPropertyChanged)));
    public static readonly DependencyProperty FadedOpacityProperty = DependencyProperty.Register(nameof (FadedOpacity), typeof (double), typeof (Fader), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(Fader.FadedOpacityPropertyChanged)));
    private Storyboard fadeStoryboard;
    private DoubleAnimation fadeAnimation;
    //private bool _contentLoaded;

    private static void FadedVisibilityPropertyChanged(
      object sender,
      DependencyPropertyChangedEventArgs args)
    {
      ((Fader) sender).SetFadedVisibility((Visibility) args.NewValue);
    }

    private static void FadedOpacityPropertyChanged(
      object sender,
      DependencyPropertyChangedEventArgs args)
    {
      ((Fader) sender).SetFadedOpacity((double) args.NewValue);
    }

    public Fader()
    {
      this.FadeinTime = 400U;
      this.FadeoutTime = 400U;
      this.fadeStoryboard = new Storyboard();
      this.fadeAnimation = new DoubleAnimation();
      this.fadeStoryboard.Children.Add((Timeline) this.fadeAnimation);
      Storyboard.SetTarget((DependencyObject) this.fadeStoryboard, (DependencyObject) this);
      Storyboard.SetTargetProperty((DependencyObject) this.fadeStoryboard, new PropertyPath("Opacity", new object[0]));
      this.fadeStoryboard.FillBehavior = FillBehavior.Stop;
      this.fadeStoryboard.Completed += new EventHandler(this.FadeComplete);
    }

    [Browsable(true)]
    [DefaultValue(Visibility.Visible)]
    public Visibility FadedVisibility
    {
      get => (Visibility) this.GetValue(Fader.FadedVisibilityProperty);
      set => this.SetValue(Fader.FadedVisibilityProperty, (object) value);
    }

    [Browsable(true)]
    [DefaultValue(1.0)]
    public double FadedOpacity
    {
      get => (double) this.GetValue(Fader.FadedOpacityProperty);
      set => this.SetValue(Fader.FadedOpacityProperty, (object) value);
    }

    [Browsable(true)]
    [DefaultValue(400)]
    public uint FadeinTime { get; set; }

    [Browsable(true)]
    [DefaultValue(400)]
    public uint FadeoutTime { get; set; }

    private void SetFadedVisibility(Visibility value)
    {
      this.fadeStoryboard.Stop();
      if (value == Visibility.Visible)
      {
        if (this.FadeinTime > 0U && (this.Visibility != Visibility.Visible || this.Opacity < 1.0))
        {
          this.fadeAnimation.From = new double?(0.0);
          this.fadeAnimation.To = new double?(this.FadedOpacity);
          this.fadeAnimation.Duration = new Duration(TimeSpan.FromMilliseconds((double) this.FadeinTime));
          this.fadeStoryboard.Duration = new Duration(TimeSpan.FromMilliseconds((double) this.FadeinTime));
          this.fadeStoryboard.Begin();
          if (this.Visibility == Visibility.Visible)
            this.fadeStoryboard.Seek(TimeSpan.FromMilliseconds(this.fadeStoryboard.Duration.TimeSpan.TotalMilliseconds * (this.Opacity / this.FadedOpacity)));
        }
        else
          this.Opacity = this.FadedOpacity;
        this.Visibility = value;
      }
      else if (this.FadeoutTime > 0U && this.Visibility == Visibility.Visible && this.Opacity > 0.0)
      {
        this.fadeAnimation.From = new double?(this.FadedOpacity);
        this.fadeAnimation.To = new double?(0.0);
        this.fadeAnimation.Duration = new Duration(TimeSpan.FromMilliseconds((double) this.FadeoutTime));
        this.fadeStoryboard.Duration = new Duration(TimeSpan.FromMilliseconds((double) this.FadeoutTime));
        this.fadeStoryboard.Begin();
        this.fadeStoryboard.Seek(TimeSpan.FromMilliseconds(this.fadeStoryboard.Duration.TimeSpan.TotalMilliseconds * (Math.Max(this.FadedOpacity - this.Opacity, 0.0) / this.FadedOpacity)));
      }
      else
        this.Visibility = value;
    }

    private void SetFadedOpacity(double value) => this.Opacity = value;

    private void FadeComplete(object sender, EventArgs args)
    {
      if (this.FadedVisibility == Visibility.Visible)
      {
        this.Opacity = this.FadedOpacity;
      }
      else
      {
        this.Opacity = 0.0;
        this.Visibility = this.FadedVisibility;
      }
    }

    /*
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft Band Sync;component/controls/fader.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;
        */
  }
}
