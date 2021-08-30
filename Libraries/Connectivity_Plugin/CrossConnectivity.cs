// Decompiled with JetBrains decompiler
// Type: Connectivity.Plugin.CrossConnectivity
// Assembly: Connectivity.Plugin, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 1426258C-DE07-4C13-AEA9-669E4B175A88
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Connectivity_Plugin.dll

using Connectivity.Plugin.Abstractions;
using System;
using System.Threading;

namespace Connectivity.Plugin
{
  public class CrossConnectivity
  {
    private static Lazy<IConnectivity> Implementation = new Lazy<IConnectivity>((Func<IConnectivity>) (() => CrossConnectivity.CreateConnectivity()), LazyThreadSafetyMode.PublicationOnly);

    public static IConnectivity Current => CrossConnectivity.Implementation.Value ?? throw CrossConnectivity.NotImplementedInReferenceAssembly();

    private static IConnectivity CreateConnectivity() => (IConnectivity) null;

    internal static Exception NotImplementedInReferenceAssembly() => (Exception) new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");

    public static void Dispose()
    {
      if (CrossConnectivity.Implementation == null || !CrossConnectivity.Implementation.IsValueCreated)
        return;
      CrossConnectivity.Implementation.Value.Dispose();
      CrossConnectivity.Implementation = new Lazy<IConnectivity>((Func<IConnectivity>) (() => CrossConnectivity.CreateConnectivity()), LazyThreadSafetyMode.PublicationOnly);
    }
  }
}
