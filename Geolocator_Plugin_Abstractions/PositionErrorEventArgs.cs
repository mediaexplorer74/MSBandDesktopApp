// Decompiled with JetBrains decompiler
// Type: Geolocator.Plugin.Abstractions.PositionErrorEventArgs
// Assembly: Geolocator.Plugin.Abstractions, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 6DCD076A-1B75-4641-A574-7C3E2F83EFFF
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Geolocator_Plugin_Abstractions.dll

using System;

namespace Geolocator.Plugin.Abstractions
{
  public class PositionErrorEventArgs : EventArgs
  {
    public PositionErrorEventArgs(GeolocationError error) => this.Error = error;

    public GeolocationError Error { get; private set; }
  }
}
