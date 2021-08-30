// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Views.INavigationService
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

namespace GalaSoft.MvvmLight.Views
{
  public interface INavigationService
  {
    string CurrentPageKey { get; }

    void GoBack();

    void NavigateTo(string pageKey);

    void NavigateTo(string pageKey, object parameter);
  }
}
