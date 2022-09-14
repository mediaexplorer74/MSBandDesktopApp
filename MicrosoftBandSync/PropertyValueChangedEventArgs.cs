// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.PropertyValueChangedEventArgs
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System.ComponentModel;

namespace DesktopSyncApp
{
  public class PropertyValueChangedEventArgs : PropertyChangedEventArgs
  {
    public readonly object NewValue;
    public readonly object OldValue;

    public PropertyValueChangedEventArgs(string propertyName, object oldValue, object newValue)
      : base(propertyName)
    {
      this.OldValue = oldValue;
      this.NewValue = newValue;
    }
  }
}
