// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxPresentationHint
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using System.Collections.Generic;

namespace Cirrious.MvvmCross.ViewModels
{
  public abstract class MvxPresentationHint
  {
    protected MvxPresentationHint()
      : this(new MvxBundle())
    {
    }

    protected MvxPresentationHint(MvxBundle body) => this.Body = body;

    protected MvxPresentationHint(IDictionary<string, string> hints)
      : this(new MvxBundle(hints))
    {
    }

    public MvxBundle Body { get; private set; }
  }
}
