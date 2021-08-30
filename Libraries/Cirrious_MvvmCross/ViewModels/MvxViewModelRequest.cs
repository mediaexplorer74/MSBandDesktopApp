// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxViewModelRequest
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.MvvmCross.Platform;
using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxViewModelRequest
  {
    public MvxViewModelRequest()
    {
    }

    public MvxViewModelRequest(
      Type viewModelType,
      IMvxBundle parameterBundle,
      IMvxBundle presentationBundle,
      MvxRequestedBy requestedBy)
    {
      this.ViewModelType = viewModelType;
      this.ParameterValues = parameterBundle.SafeGetData();
      this.PresentationValues = presentationBundle.SafeGetData();
      this.RequestedBy = requestedBy;
    }

    public Type ViewModelType { get; set; }

    public IDictionary<string, string> ParameterValues { get; set; }

    public IDictionary<string, string> PresentationValues { get; set; }

    public MvxRequestedBy RequestedBy { get; set; }

    public static MvxViewModelRequest GetDefaultRequest(Type viewModelType) => new MvxViewModelRequest(viewModelType, (IMvxBundle) null, (IMvxBundle) null, MvxRequestedBy.Unknown);
  }
}
