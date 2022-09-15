// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxNavigationSerializer`1
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxNavigationSerializer<T> : MvxNavigationSerializer where T : class, IMvxTextSerializer
  {
    public MvxNavigationSerializer()
      : base((IMvxTextSerializer) Mvx.Resolve<T>())
    {
    }
  }
}
