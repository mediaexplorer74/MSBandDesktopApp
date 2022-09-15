// Decompiled with JetBrains decompiler
// Type: Geolocator.Plugin.CrossGeolocator
// Assembly: Geolocator.Plugin, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: 06A9370A-1E61-4F86-9550-031E6F9093A2
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Geolocator_Plugin.dll

using Geolocator.Plugin.Abstractions;
using System;
using System.Threading;

namespace Geolocator.Plugin
{
  public class CrossGeolocator
  {
    private static Lazy<IGeolocator> Implementation = new Lazy<IGeolocator>((Func<IGeolocator>) (() => CrossGeolocator.CreateGeolocator()), LazyThreadSafetyMode.PublicationOnly);

    public static IGeolocator Current => CrossGeolocator.Implementation.Value ?? throw CrossGeolocator.NotImplementedInReferenceAssembly();

    private static IGeolocator CreateGeolocator() => (IGeolocator) null;

    internal static Exception NotImplementedInReferenceAssembly() => (Exception) new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
  }
}
