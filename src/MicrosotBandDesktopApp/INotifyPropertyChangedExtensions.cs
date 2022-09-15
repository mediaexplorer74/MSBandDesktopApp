// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.INotifyPropertyChangedExtensions
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System.ComponentModel;

namespace DesktopSyncApp
{
  public static class INotifyPropertyChangedExtensions
  {
    public static void OnPropertyChanged(
      this INotifyPropertyChanged sender,
      string name,
      PropertyChangedEventHandler eventHandler)
    {
      if (eventHandler == null)
        return;
      eventHandler((object) sender, new PropertyChangedEventArgs(name));
    }

    public static void OnPropertyChanged(
      this INotifyPropertyChanged sender,
      string name,
      PropertyChangedEventHandler eventHandler1,
      PropertyChangedEventHandler eventHandler2)
    {
      PropertyChangedEventArgs e1 = (PropertyChangedEventArgs) null;
      if (eventHandler1 != null)
      {
        e1 = e1 ?? new PropertyChangedEventArgs(name);
        eventHandler1((object) sender, e1);
      }
      if (eventHandler2 == null)
        return;
      PropertyChangedEventArgs e2 = e1 ?? new PropertyChangedEventArgs(name);
      eventHandler2((object) sender, e2);
    }

    public static void OnPropertyChanged(
      this INotifyPropertyChanged sender,
      string name,
      PropertyChangedEventHandler propertyChangedEventHandler,
      PropertyValueChangedEventHandler propertyValueChangedEventHandler,
      object oldValue,
      object newValue)
    {
      if (propertyChangedEventHandler != null)
        propertyChangedEventHandler((object) sender, new PropertyChangedEventArgs(name));
      if (propertyValueChangedEventHandler == null)
        return;
      propertyValueChangedEventHandler((object) sender, new PropertyValueChangedEventArgs(name, oldValue, newValue));
    }
  }
}
