// Decompiled with JetBrains decompiler
// Type: Connectivity.Plugin.Abstractions.ConnectivityChangedEventArgs
// Assembly: Connectivity.Plugin.Abstractions, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AB69076D-CAA0-4B7C-9B1E-3DD73914B51F
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Connectivity_Plugin_Abstractions.dll

using System;

namespace Connectivity.Plugin.Abstractions
{
  public class ConnectivityChangedEventArgs : EventArgs
  {
    public bool IsConnected { get; set; }
  }
}
