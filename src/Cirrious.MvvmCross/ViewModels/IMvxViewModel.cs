// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.IMvxViewModel
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

namespace Cirrious.MvvmCross.ViewModels
{
  public interface IMvxViewModel
  {
    MvxRequestedBy RequestedBy { get; set; }

    void Init(IMvxBundle parameters);

    void ReloadState(IMvxBundle state);

    void Start();

    void SaveState(IMvxBundle state);
  }
}
