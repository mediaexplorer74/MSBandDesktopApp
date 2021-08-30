// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.Views.MvxViewExtensionMethods
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Reflection;

namespace Cirrious.MvvmCross.Views
{
  public static class MvxViewExtensionMethods
  {
    public static void OnViewCreate(this IMvxView view, Func<IMvxViewModel> viewModelLoader)
    {
      if (view.DataContext != null || view.ViewModel != null)
        return;
      IMvxViewModel mvxViewModel = viewModelLoader();
      if (mvxViewModel == null)
        MvxTrace.Warning("ViewModel not loaded for view {0}", (object) view.GetType().Name);
      else
        view.ViewModel = mvxViewModel;
    }

    public static void OnViewNewIntent(this IMvxView view, Func<IMvxViewModel> viewModelLoader)
    {
      MvxTrace.Warning("OnViewNewIntent isn't well understood or tested inside MvvmCross - it's not really a cross-platform concept.");
      throw new MvxException("OnViewNewIntent is not implemented");
    }

    public static void OnViewDestroy(this IMvxView view)
    {
    }

    public static Type FindAssociatedViewModelTypeOrNull(this IMvxView view)
    {
      if (view == null)
        return (Type) null;
      IMvxViewModelTypeFinder service;
      if (Mvx.TryResolve<IMvxViewModelTypeFinder>(out service))
        return service.FindTypeOrNull(view.GetType());
      MvxTrace.Trace("No view model type finder available - assuming we are looking for a splash screen - returning null");
      return typeof (MvxNullViewModel);
    }

    public static IMvxViewModel ReflectionGetViewModel(this IMvxView view)
    {
      if (view == null)
        return (IMvxViewModel) null;
      PropertyInfo property = ReflectionExtensions.GetProperty(view.GetType(), "ViewModel");
      return (object) property == null ? (IMvxViewModel) null : (IMvxViewModel) ReflectionExtensions.GetGetMethod(property).Invoke((object) view, new object[0]);
    }

    public static IMvxBundle CreateSaveStateBundle(this IMvxView view)
    {
      IMvxViewModel viewModel = view.ViewModel;
      return viewModel == null ? (IMvxBundle) new MvxBundle() : viewModel.SaveStateBundle();
    }
  }
}
