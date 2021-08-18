// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.KDKProgress`1
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.ComponentModel;

namespace DesktopSyncApp
{
  public abstract class KDKProgress<T> : IProgress<T>, INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    public abstract void Report(T progress);

    protected void OnPropertyChanged(string name) => this.OnPropertyChanged(name, this.PropertyChanged);
  }
}
