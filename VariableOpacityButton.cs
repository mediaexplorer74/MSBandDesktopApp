// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.VariableOpacityButton
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

namespace DesktopSyncApp
{
  public class VariableOpacityButton : Button, IComponentConnector
  {
    public static readonly DependencyProperty BaseOpacityProperty = DependencyProperty.Register(nameof (BaseOpacity), typeof (double), typeof (VariableOpacityButton), new PropertyMetadata(UIElement.OpacityProperty.DefaultMetadata.DefaultValue, new PropertyChangedCallback(VariableOpacityButton.BaseOpacityPropertyChanged)));
    public static readonly DependencyProperty MouseOverOpacityProperty = DependencyProperty.Register(nameof (MouseOverOpacity), typeof (double), typeof (VariableOpacityButton), new PropertyMetadata(UIElement.OpacityProperty.DefaultMetadata.DefaultValue, new PropertyChangedCallback(VariableOpacityButton.MouseOverOpacityPropertyChanged)));
    public static readonly DependencyProperty MouseDownOpacityProperty = DependencyProperty.Register(nameof (MouseDownOpacity), typeof (double), typeof (VariableOpacityButton), new PropertyMetadata(UIElement.OpacityProperty.DefaultMetadata.DefaultValue, new PropertyChangedCallback(VariableOpacityButton.MouseDownOpacityPropertyChanged)));
    public static readonly DependencyProperty BaseBackgroundProperty;
    public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.Register(nameof (MouseOverBackground), typeof (Brush), typeof (VariableOpacityButton), new PropertyMetadata(Control.BackgroundProperty.DefaultMetadata.DefaultValue, new PropertyChangedCallback(VariableOpacityButton.MouseOverBackgroundPropertyChanged)));
    public static readonly DependencyProperty MouseDownBackgroundProperty = DependencyProperty.Register(nameof (MouseDownBackground), typeof (Brush), typeof (VariableOpacityButton), new PropertyMetadata(Control.BackgroundProperty.DefaultMetadata.DefaultValue, new PropertyChangedCallback(VariableOpacityButton.MouseDownBackgroundPropertyChanged)));
    public static readonly DependencyProperty BaseForegroundProperty;
    public static readonly DependencyProperty MouseOverForegroundProperty = DependencyProperty.Register(nameof (MouseOverForeground), typeof (Brush), typeof (VariableOpacityButton), new PropertyMetadata(Control.ForegroundProperty.DefaultMetadata.DefaultValue, new PropertyChangedCallback(VariableOpacityButton.MouseOverForegroundPropertyChanged)));
    public static readonly DependencyProperty MouseDownForegroundProperty = DependencyProperty.Register(nameof (MouseDownForeground), typeof (Brush), typeof (VariableOpacityButton), new PropertyMetadata(Control.ForegroundProperty.DefaultMetadata.DefaultValue, new PropertyChangedCallback(VariableOpacityButton.MouseDownForegroundPropertyChanged)));
    private bool _contentLoaded;

    private static void BaseOpacityPropertyChanged(
      object sender,
      DependencyPropertyChangedEventArgs args)
    {
      ((VariableOpacityButton) sender).BaseOpacity = (double) args.NewValue;
    }

    private static void MouseOverOpacityPropertyChanged(
      object sender,
      DependencyPropertyChangedEventArgs args)
    {
      ((VariableOpacityButton) sender).MouseOverOpacity = (double) args.NewValue;
    }

    private static void MouseDownOpacityPropertyChanged(
      object sender,
      DependencyPropertyChangedEventArgs args)
    {
      ((VariableOpacityButton) sender).MouseDownOpacity = (double) args.NewValue;
    }

    private static void MouseOverBackgroundPropertyChanged(
      object sender,
      DependencyPropertyChangedEventArgs args)
    {
      ((VariableOpacityButton) sender).MouseOverBackground = (Brush) args.NewValue;
    }

    private static void MouseDownBackgroundPropertyChanged(
      object sender,
      DependencyPropertyChangedEventArgs args)
    {
      ((VariableOpacityButton) sender).MouseDownBackground = (Brush) args.NewValue;
    }

    private static void MouseOverForegroundPropertyChanged(
      object sender,
      DependencyPropertyChangedEventArgs args)
    {
      ((VariableOpacityButton) sender).MouseOverForeground = (Brush) args.NewValue;
    }

    private static void MouseDownForegroundPropertyChanged(
      object sender,
      DependencyPropertyChangedEventArgs args)
    {
      ((VariableOpacityButton) sender).MouseDownForeground = (Brush) args.NewValue;
    }

    [Browsable(true)]
    public double BaseOpacity
    {
      get => (double) this.GetValue(VariableOpacityButton.BaseOpacityProperty);
      set => this.SetValue(VariableOpacityButton.BaseOpacityProperty, (object) value);
    }

    [Browsable(true)]
    public double MouseOverOpacity
    {
      get => (double) this.GetValue(VariableOpacityButton.MouseOverOpacityProperty);
      set => this.SetValue(VariableOpacityButton.MouseOverOpacityProperty, (object) value);
    }

    [Browsable(true)]
    public double MouseDownOpacity
    {
      get => (double) this.GetValue(VariableOpacityButton.MouseDownOpacityProperty);
      set => this.SetValue(VariableOpacityButton.MouseDownOpacityProperty, (object) value);
    }

    [Browsable(true)]
    public Brush MouseOverBackground
    {
      get => (Brush) this.GetValue(VariableOpacityButton.MouseOverBackgroundProperty);
      set => this.SetValue(VariableOpacityButton.MouseOverBackgroundProperty, (object) value);
    }

    [Browsable(true)]
    public Brush MouseDownBackground
    {
      get => (Brush) this.GetValue(VariableOpacityButton.MouseDownBackgroundProperty);
      set => this.SetValue(VariableOpacityButton.MouseDownBackgroundProperty, (object) value);
    }

    [Browsable(true)]
    public Brush MouseOverForeground
    {
      get => (Brush) this.GetValue(VariableOpacityButton.MouseOverForegroundProperty);
      set => this.SetValue(VariableOpacityButton.MouseOverForegroundProperty, (object) value);
    }

    [Browsable(true)]
    public Brush MouseDownForeground
    {
      get => (Brush) this.GetValue(VariableOpacityButton.MouseDownForegroundProperty);
      set => this.SetValue(VariableOpacityButton.MouseDownForegroundProperty, (object) value);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft Band Sync;component/controls/variableopacitybutton.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
