// Decompiled with JetBrains decompiler
// Type: Geolocator.Plugin.Abstractions.IGeolocator
// Assembly: Geolocator.Plugin.Abstractions, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 6DCD076A-1B75-4641-A574-7C3E2F83EFFF
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Geolocator_Plugin_Abstractions.dll

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Geolocator.Plugin.Abstractions
{
  public interface IGeolocator
  {
    event EventHandler<PositionErrorEventArgs> PositionError;

    event EventHandler<PositionEventArgs> PositionChanged;

    double DesiredAccuracy { get; set; }

    bool IsListening { get; }

    bool SupportsHeading { get; }

    bool IsGeolocationAvailable { get; }

    bool IsGeolocationEnabled { get; }

    Task<Position> GetPositionAsync(
      int timeout = -1,
      CancellationToken? token = null,
      bool includeHeading = false);

    void StartListening(int minTime, double minDistance, bool includeHeading = false);

    void StopListening();
  }
}
