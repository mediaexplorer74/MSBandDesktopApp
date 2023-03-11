// ProfileControl.cs
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
  public partial class ProfileControl : SyncAppPageControl
  { 
    private bool ChangingBinding;

    public ProfileControl(AppMainWindow parent)
      : base(parent, false)
    {
      this.InitializeComponent();
      this.spWeightEditStandard.SetBinding(Fader.FadedVisibilityProperty, 
          (BindingBase) this.CreateMultiBinding(
              (IMultiValueConverter) new WeightEditVisibilityMultiConverter(), 
              (object) (MassUnitType) 1, "LoginInfo.UserProfileEdit.Editing",
              "LoginInfo.UserProfileEdit.WeightDisplayUnits"));
      this.spWeightEditMetric.SetBinding(Fader.FadedVisibilityProperty, 
          (BindingBase) this.CreateMultiBinding(
              (IMultiValueConverter) new WeightEditVisibilityMultiConverter(), 
              (object) (MassUnitType) 2, "LoginInfo.UserProfileEdit.Editing", 
              "LoginInfo.UserProfileEdit.WeightDisplayUnits"));
      this.spHeightEditStandard.SetBinding(Fader.FadedVisibilityProperty, 
          (BindingBase) this.CreateMultiBinding(
              (IMultiValueConverter) new HeightEditVisibilityMultiConverter(), 
              (object) (DistanceUnitType) 1, "LoginInfo.UserProfileEdit.Editing",
              "LoginInfo.UserProfileEdit.HeightDisplayUnits"));
      this.spHeightEditMetric.SetBinding(Fader.FadedVisibilityProperty, 
          (BindingBase) this.CreateMultiBinding(
              (IMultiValueConverter) new HeightEditVisibilityMultiConverter(),
              (object) (DistanceUnitType) 2, "LoginInfo.UserProfileEdit.Editing", 
              "LoginInfo.UserProfileEdit.HeightDisplayUnits"));
      this.btForgetDevice.SetBinding(Fader.FadedVisibilityProperty, 
          (BindingBase) this.CreateMultiBinding(
              (IMultiValueConverter) new ForgetDeviceVisibilityMultiConverter(), 
              (object) null, "LoginInfo.UserProfile.PairedDeviceID", 
              "LoginInfo.UserProfileEdit.Editing"));
    }

    public override void OnHide()
    {
      this.model.EndEditUserProfile.Execute((object) false);
      base.OnHide();
    }

    private void NumericEditor_Loaded(object sender, RoutedEventArgs e) 
            => DataObject.AddPastingHandler((DependencyObject) sender, 
                new DataObjectPastingEventHandler(this.OnPastingToNumericTextBox));

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

    private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) 
            => ((TextBoxBase) sender).SelectAll();

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
      if (this.model.LoginInfo == null || this.model.LoginInfo.UserProfileEdit == null 
                || ((TextBox) sender).Text.Length <= 0)
        return;
      this.model.LoginInfo.UserProfileEdit.FirstNameError = false;
    }

    private void DeviceName_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.model.LoginInfo == null || this.model.LoginInfo.UserProfileEdit == null 
                || ((TextBox) sender).Text.Length <= 0)
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

    private void ChangingDisplayUnits_Button_Click(object sender, RoutedEventArgs e) 
            => this.ChangingBinding = true;

    
  }
}
