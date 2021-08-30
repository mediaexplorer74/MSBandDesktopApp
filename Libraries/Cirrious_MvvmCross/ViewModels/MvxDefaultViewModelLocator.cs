// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxDefaultViewModelLocator
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using System;

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxDefaultViewModelLocator : IMvxViewModelLocator
  {
    public virtual IMvxViewModel Load(
      Type viewModelType,
      IMvxBundle parameterValues,
      IMvxBundle savedState)
    {
      IMvxViewModel viewModel;
      try
      {
        viewModel = (IMvxViewModel) Mvx.IocConstruct(viewModelType);
      }
      catch (Exception ex)
      {
        throw ex.MvxWrap("Problem creating viewModel of type {0}", (object) viewModelType.Name);
      }
      try
      {
        this.CallCustomInitMethods(viewModel, parameterValues);
        if (savedState != null)
          this.CallReloadStateMethods(viewModel, savedState);
        viewModel.Start();
      }
      catch (Exception ex)
      {
        throw ex.MvxWrap("Problem initialising viewModel of type {0}", (object) viewModelType.Name);
      }
      return viewModel;
    }

    protected virtual void CallCustomInitMethods(
      IMvxViewModel viewModel,
      IMvxBundle parameterValues)
    {
      viewModel.CallBundleMethods("Init", parameterValues);
    }

    protected virtual void CallReloadStateMethods(IMvxViewModel viewModel, IMvxBundle savedState) => viewModel.CallBundleMethods("ReloadState", savedState);
  }
}
