// Decompiled with JetBrains decompiler
// Type: Connectivity.Plugin.Abstractions.BaseConnectivity
// Assembly: Connectivity.Plugin.Abstractions, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AB69076D-CAA0-4B7C-9B1E-3DD73914B51F
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Connectivity_Plugin_Abstractions.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connectivity.Plugin.Abstractions
{
  public abstract class BaseConnectivity : IConnectivity, IDisposable
  {
    private bool disposed;

    public abstract bool IsConnected { get; }

    public abstract Task<bool> IsReachable(string host, int msTimeout = 5000);

    public abstract Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000);

    public abstract IEnumerable<ConnectionType> ConnectionTypes { get; }

    public abstract IEnumerable<ulong> Bandwidths { get; }

    protected virtual void OnConnectivityChanged(ConnectivityChangedEventArgs e)
    {
      if (this.ConnectivityChanged == null)
        return;
      this.ConnectivityChanged((object) this, e);
    }

    public event ConnectivityChangedEventHandler ConnectivityChanged;

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    ~BaseConnectivity() => this.Dispose(false);

    public virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      int num = disposing ? 1 : 0;
      this.disposed = true;
    }
  }
}
