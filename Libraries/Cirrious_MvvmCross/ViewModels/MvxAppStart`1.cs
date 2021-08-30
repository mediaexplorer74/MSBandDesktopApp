// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxAppStart`1
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxAppStart<TViewModel> : MvxNavigatingObject, IMvxAppStart
    where TViewModel : IMvxViewModel
  {
    public void Start(object hint = null)
    {
      if (hint != null)
        MvxTrace.Trace("Hint ignored in default MvxAppStart");
      this.ShowViewModel<TViewModel>();
    }
  }
}
