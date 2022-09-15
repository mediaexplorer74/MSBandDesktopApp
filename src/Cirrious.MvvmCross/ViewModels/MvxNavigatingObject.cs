// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxNavigatingObject
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Views;
using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.ViewModels
{
  public abstract class MvxNavigatingObject : MvxNotifyPropertyChanged
  {
    protected IMvxViewDispatcher ViewDispatcher => (IMvxViewDispatcher) this.Dispatcher;

    protected bool Close(IMvxViewModel viewModel) => this.ChangePresentation((MvxPresentationHint) new MvxClosePresentationHint(viewModel));

    protected bool ChangePresentation(MvxPresentationHint hint)
    {
      MvxTrace.Trace("Requesting presentation change");
      IMvxViewDispatcher viewDispatcher = this.ViewDispatcher;
      return viewDispatcher != null && viewDispatcher.ChangePresentation(hint);
    }

    protected bool ShowViewModel<TViewModel>(
      object parameterValuesObject,
      IMvxBundle presentationBundle = null,
      MvxRequestedBy requestedBy = null)
      where TViewModel : IMvxViewModel
    {
      return this.ShowViewModel(typeof (TViewModel), parameterValuesObject.ToSimplePropertyDictionary(), presentationBundle, requestedBy);
    }

    protected bool ShowViewModel<TViewModel>(
      IDictionary<string, string> parameterValues,
      IMvxBundle presentationBundle = null,
      MvxRequestedBy requestedBy = null)
      where TViewModel : IMvxViewModel
    {
      return this.ShowViewModel(typeof (TViewModel), (IMvxBundle) new MvxBundle(parameterValues.ToSimplePropertyDictionary()), presentationBundle, requestedBy);
    }

    protected bool ShowViewModel<TViewModel>(
      IMvxBundle parameterBundle = null,
      IMvxBundle presentationBundle = null,
      MvxRequestedBy requestedBy = null)
      where TViewModel : IMvxViewModel
    {
      return this.ShowViewModel(typeof (TViewModel), parameterBundle, presentationBundle, requestedBy);
    }

    protected bool ShowViewModel(
      Type viewModelType,
      object parameterValuesObject,
      IMvxBundle presentationBundle = null,
      MvxRequestedBy requestedBy = null)
    {
      return this.ShowViewModel(viewModelType, (IMvxBundle) new MvxBundle(parameterValuesObject.ToSimplePropertyDictionary()), presentationBundle, requestedBy);
    }

    protected bool ShowViewModel(
      Type viewModelType,
      IDictionary<string, string> parameterValues,
      IMvxBundle presentationBundle = null,
      MvxRequestedBy requestedBy = null)
    {
      return this.ShowViewModel(viewModelType, (IMvxBundle) new MvxBundle(parameterValues), presentationBundle, requestedBy);
    }

    protected bool ShowViewModel(
      Type viewModelType,
      IMvxBundle parameterBundle = null,
      IMvxBundle presentationBundle = null,
      MvxRequestedBy requestedBy = null)
    {
      return this.ShowViewModelImpl(viewModelType, parameterBundle, presentationBundle, requestedBy);
    }

    private bool ShowViewModelImpl(
      Type viewModelType,
      IMvxBundle parameterBundle,
      IMvxBundle presentationBundle,
      MvxRequestedBy requestedBy)
    {
      MvxTrace.Trace("Showing ViewModel {0}", (object) viewModelType.Name);
      IMvxViewDispatcher viewDispatcher = this.ViewDispatcher;
      return viewDispatcher != null && viewDispatcher.ShowViewModel(new MvxViewModelRequest(viewModelType, parameterBundle, presentationBundle, requestedBy));
    }
  }
}
