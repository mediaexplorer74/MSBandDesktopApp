// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxViewModelLoader
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using System;

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxViewModelLoader : IMvxViewModelLoader
  {
    private IMvxViewModelLocatorCollection _locatorCollection;

    protected IMvxViewModelLocatorCollection LocatorCollection
    {
      get
      {
        this._locatorCollection = this._locatorCollection ?? Mvx.Resolve<IMvxViewModelLocatorCollection>();
        return this._locatorCollection;
      }
    }

    public IMvxViewModel LoadViewModel(
      MvxViewModelRequest request,
      IMvxBundle savedState)
    {
      if ((object) request.ViewModelType == (object) typeof (MvxNullViewModel))
        return (IMvxViewModel) new MvxNullViewModel();
      IMvxViewModelLocator viewModelLocator = this.FindViewModelLocator(request);
      return this.LoadViewModel(request, savedState, viewModelLocator);
    }

    private IMvxViewModel LoadViewModel(
      MvxViewModelRequest request,
      IMvxBundle savedState,
      IMvxViewModelLocator viewModelLocator)
    {
      MvxBundle mvxBundle = new MvxBundle(request.ParameterValues);
      IMvxViewModel mvxViewModel;
      try
      {
        mvxViewModel = viewModelLocator.Load(request.ViewModelType, (IMvxBundle) mvxBundle, savedState);
      }
      catch (Exception ex)
      {
        throw ex.MvxWrap("Failed to construct and initialize ViewModel for type {0} from locator {1} - check InnerException for more information", (object) request.ViewModelType, (object) viewModelLocator.GetType().Name);
      }
      mvxViewModel.RequestedBy = request.RequestedBy;
      return mvxViewModel;
    }

    private IMvxViewModelLocator FindViewModelLocator(
      MvxViewModelRequest request)
    {
      return this.LocatorCollection.FindViewModelLocator(request) ?? throw new MvxException("Sorry - somehow there's no viewmodel locator registered for {0}", new object[1]
      {
        (object) request.ViewModelType
      });
    }
  }
}
