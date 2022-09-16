// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.ProfileControl
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using DesktopSyncApp.BindingConverters;
using Microsoft.Band.Admin;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace DesktopSyncApp
{
  public partial class ProfileControl : SyncAppPageControl, IComponentConnector
  {
    private bool ChangingBinding;
    //internal Fader spWeightEditStandard;
    //internal Fader spWeightEditMetric;
    //internal Fader spHeightEditStandard;
    //internal Fader spHeightEditMetric;
    //internal Fader btForgetDevice;
    
        //private bool _contentLoaded;

    public ProfileControl(AppMainWindow parent)
      : base(parent, false)
    {
      this.InitializeComponent();
      this.spWeightEditStandard.SetBinding(Fader.FadedVisibilityProperty, (BindingBase) this.CreateMultiBinding((IMultiValueConverter) new WeightEditVisibilityMultiConverter(), (object) (MassUnitType) 1, "LoginInfo.UserProfileEdit.Editing", "LoginInfo.UserProfileEdit.WeightDisplayUnits"));
      this.spWeightEditMetric.SetBinding(Fader.FadedVisibilityProperty, (BindingBase) this.CreateMultiBinding((IMultiValueConverter) new WeightEditVisibilityMultiConverter(), (object) (MassUnitType) 2, "LoginInfo.UserProfileEdit.Editing", "LoginInfo.UserProfileEdit.WeightDisplayUnits"));
      this.spHeightEditStandard.SetBinding(Fader.FadedVisibilityProperty, (BindingBase) this.CreateMultiBinding((IMultiValueConverter) new HeightEditVisibilityMultiConverter(), (object) (DistanceUnitType) 1, "LoginInfo.UserProfileEdit.Editing", "LoginInfo.UserProfileEdit.HeightDisplayUnits"));
      this.spHeightEditMetric.SetBinding(Fader.FadedVisibilityProperty, (BindingBase) this.CreateMultiBinding((IMultiValueConverter) new HeightEditVisibilityMultiConverter(), (object) (DistanceUnitType) 2, "LoginInfo.UserProfileEdit.Editing", "LoginInfo.UserProfileEdit.HeightDisplayUnits"));
      this.btForgetDevice.SetBinding(Fader.FadedVisibilityProperty, (BindingBase) this.CreateMultiBinding((IMultiValueConverter) new ForgetDeviceVisibilityMultiConverter(), (object) null, "LoginInfo.UserProfile.PairedDeviceID", "LoginInfo.UserProfileEdit.Editing"));
    }

    public override void OnHide()
    {
      this.model.EndEditUserProfile.Execute((object) false);
      base.OnHide();
    }

    private void NumericEditor_Loaded(object sender, RoutedEventArgs e) => DataObject.AddPastingHandler((DependencyObject) sender, new DataObjectPastingEventHandler(this.OnPastingToNumericTextBox));

    public MultiBinding CreateMultiBinding(
      IMultiValueConverter converter,
      object converterParameter,
      params string[] paths)
    {
      MultiBinding multiBinding = new MultiBinding();
      foreach (string path in paths)
        multiBinding.Bindings.Add((BindingBase) new Binding(path));
      multiBinding.Converter = converter;
      multiBinding.ConverterParameter = converterParameter;
      return multiBinding;
    }

    private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) => ((TextBoxBase) sender).SelectAll();

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      ComboBox comboBox = sender as ComboBox;
      comboBox.SelectedItem = comboBox.Items[comboBox.SelectedIndex];
    }

    private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs args)
    {
      if (args.Text.IsAllNumeric())
        return;
      args.Handled = true;
    }

    private void OnPastingToNumericTextBox(object sender, DataObjectPastingEventArgs args)
    {
      try
      {
        if (((string) args.DataObject.GetData(typeof (string))).IsAllNumeric())
          return;
        args.CancelCommand();
      }
      catch
      {
        args.CancelCommand();
      }
    }

    private void FirstName_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.model.LoginInfo == null || this.model.LoginInfo.UserProfileEdit == null || ((TextBox) sender).Text.Length <= 0)
        return;
      this.model.LoginInfo.UserProfileEdit.FirstNameError = false;
    }

    private void DeviceName_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.model.LoginInfo == null || this.model.LoginInfo.UserProfileEdit == null || ((TextBox) sender).Text.Length <= 0)
        return;
      this.model.LoginInfo.UserProfileEdit.DeviceNameError = false;
    }

    private void ChangingDisplayUnits_IsVisibleChanged(
      object sender,
      DependencyPropertyChangedEventArgs e)
    {
      if ((bool) e.NewValue)
      {
        if (this.ChangingBinding)
        {
          try
          {
            ((UIElement) sender).Focus();
          }
          catch
          {
          }
        }
      }
      this.ChangingBinding = false;
    }

    private void ChangingDisplayUnits_Button_Click(object sender, RoutedEventArgs e) => this.ChangingBinding = true;

        /*
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft Band Sync;component/pagecontrols/profilecontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((UIElement) target).GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.TextBox_GotKeyboardFocus);
          ((TextBoxBase) target).TextChanged += new TextChangedEventHandler(this.FirstName_TextChanged);
          break;
        case 2:
          ((UIElement) target).GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.TextBox_GotKeyboardFocus);
          ((TextBoxBase) target).TextChanged += new TextChangedEventHandler(this.DeviceName_TextChanged);
          break;
        case 3:
          this.spWeightEditStandard = (Fader) target;
          break;
        case 4:
          ((UIElement) target).GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.TextBox_GotKeyboardFocus);
          ((UIElement) target).IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.ChangingDisplayUnits_IsVisibleChanged);
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.NumericEditor_Loaded);
          ((UIElement) target).PreviewTextInput += new TextCompositionEventHandler(this.NumericTextBox_PreviewTextInput);
          break;
        case 5:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.ChangingDisplayUnits_Button_Click);
          break;
        case 6:
          this.spWeightEditMetric = (Fader) target;
          break;
        case 7:
          ((UIElement) target).GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.TextBox_GotKeyboardFocus);
          ((UIElement) target).IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.ChangingDisplayUnits_IsVisibleChanged);
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.NumericEditor_Loaded);
          ((UIElement) target).PreviewTextInput += new TextCompositionEventHandler(this.NumericTextBox_PreviewTextInput);
          break;
        case 8:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.ChangingDisplayUnits_Button_Click);
          break;
        case 9:
          this.spHeightEditStandard = (Fader) target;
          break;
        case 10:
          ((UIElement) target).IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.ChangingDisplayUnits_IsVisibleChanged);
          ((Selector) target).SelectionChanged += new SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
          break;
        case 11:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.ChangingDisplayUnits_Button_Click);
          break;
        case 12:
          ((Selector) target).SelectionChanged += new SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
          break;
        case 13:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.ChangingDisplayUnits_Button_Click);
          break;
        case 14:
          this.spHeightEditMetric = (Fader) target;
          break;
        case 15:
          ((UIElement) target).GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.TextBox_GotKeyboardFocus);
          ((UIElement) target).IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.ChangingDisplayUnits_IsVisibleChanged);
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.NumericEditor_Loaded);
          ((UIElement) target).PreviewTextInput += new TextCompositionEventHandler(this.NumericTextBox_PreviewTextInput);
          break;
        case 16:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.ChangingDisplayUnits_Button_Click);
          break;
        case 17:
          ((Selector) target).SelectionChanged += new SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
          break;
        case 18:
          ((UIElement) target).GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.TextBox_GotKeyboardFocus);
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.NumericEditor_Loaded);
          ((UIElement) target).PreviewTextInput += new TextCompositionEventHandler(this.NumericTextBox_PreviewTextInput);
          break;
        case 19:
          ((UIElement) target).GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.TextBox_GotKeyboardFocus);
          break;
        case 20:
          this.btForgetDevice = (Fader) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
        */
  }
}
