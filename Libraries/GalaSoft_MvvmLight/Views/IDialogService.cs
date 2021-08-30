// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Views.IDialogService
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

using System;
using System.Threading.Tasks;

namespace GalaSoft.MvvmLight.Views
{
  public interface IDialogService
  {
    Task ShowError(string message, string title, string buttonText, Action afterHideCallback);

    Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback);

    Task ShowMessage(string message, string title);

    Task ShowMessage(
      string message,
      string title,
      string buttonText,
      Action afterHideCallback);

    Task<bool> ShowMessage(
      string message,
      string title,
      string buttonConfirmText,
      string buttonCancelText,
      Action<bool> afterHideCallback);

    Task ShowMessageBox(string message, string title);
  }
}
