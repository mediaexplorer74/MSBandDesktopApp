// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxViewModel
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

namespace Cirrious.MvvmCross.ViewModels
{
  public abstract class MvxViewModel : MvxNavigatingObject, IMvxViewModel
  {
    protected MvxViewModel() => this.RequestedBy = MvxRequestedBy.Unknown;

    public MvxRequestedBy RequestedBy { get; set; }

    public void Init(IMvxBundle parameters) => this.InitFromBundle(parameters);

    public void ReloadState(IMvxBundle state) => this.ReloadFromBundle(state);

    public virtual void Start()
    {
    }

    public void SaveState(IMvxBundle state) => this.SaveStateToBundle(state);

    protected virtual void InitFromBundle(IMvxBundle parameters)
    {
    }

    protected virtual void ReloadFromBundle(IMvxBundle state)
    {
    }

    protected virtual void SaveStateToBundle(IMvxBundle bundle)
    {
    }
  }
}
