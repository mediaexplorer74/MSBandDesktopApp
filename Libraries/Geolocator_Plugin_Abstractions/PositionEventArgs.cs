// Decompiled with JetBrains decompiler
// Type: Geolocator.Plugin.Abstractions.PositionEventArgs
// Assembly: Geolocator.Plugin.Abstractions, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 6DCD076A-1B75-4641-A574-7C3E2F83EFFF
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Geolocator_Plugin_Abstractions.dll

using System;

namespace Geolocator.Plugin.Abstractions
{
  public class PositionEventArgs : EventArgs
  {
    public PositionEventArgs(Position position) => this.Position = position != null ? position : throw new ArgumentNullException(nameof (position));

    public Position Position { get; private set; }
  }
}
