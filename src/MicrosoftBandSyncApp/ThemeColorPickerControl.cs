// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.ThemeColorPickerControl
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
  public partial class ThemeColorPickerControl : SyncAppPageControl, IComponentConnector
  {
    internal PersonalizeDeviceEnvoy DeviceEnvoy;
    internal PersonalizeDeviceCargo DeviceCargo;
    internal Button PatternButton;  
        
    //private bool _contentLoaded;

    public ThemeColorPickerControl(AppMainWindow parent)
      : base(parent, false)
    {
      this.InitializeComponent();
      if ((int) this.model.ThemeManager.CurrentBandClass == 2)
      {
        this.PatternButton.Width = 137.0;
        this.PatternButton.Height = 58.0;
        this.DeviceEnvoy.Visibility = Visibility.Visible;
        this.DeviceCargo.Visibility = Visibility.Hidden;
      }
      else
      {
        this.DeviceEnvoy.Visibility = Visibility.Hidden;
        this.DeviceCargo.Visibility = Visibility.Visible;
      }
    }

       

        public void Connect(int connectionId, object target)
        {
            throw new NotImplementedException();
        }

        public void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private async void SaveCustomization_Click(object sender, RoutedEventArgs e)
    {
      using (new DisposableAction((Action) (() => this.model.ThemeManager.UpdatingDeviceTheme = true), (Action) (() => this.model.ThemeManager.UpdatingDeviceTheme = false)))
      {
        await this.model.DeviceManager.SaveThemeToBand(this.model.DeviceManager.CurrentDevice.CargoClient, this.model.DeviceManager.CurrentDevice, false);
        this.model.ShowSyncCommand.Execute((object) null);
      }
    }

    //[DebuggerNonUserCode]
    //[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    /*
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft Band Sync;component/pagecontrols/themecolorpickercontrol.xaml", UriKind.Relative));
    }
        */

    //[DebuggerNonUserCode]
    //[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    /*
        internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);
    */

    //[DebuggerNonUserCode]
    //[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    //[EditorBrowsable(EditorBrowsableState.Never)]
    /*
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.DeviceEnvoy = (PersonalizeDeviceEnvoy) target;
          break;
        case 2:
          this.DeviceCargo = (PersonalizeDeviceCargo) target;
          break;
        case 3:
          this.PatternButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
            
    }
    */
  }
}
