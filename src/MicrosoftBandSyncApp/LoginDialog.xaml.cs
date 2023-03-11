// LoginDialog.cs
// Type: DesktopSyncApp.LoginDialog
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace DesktopSyncApp
{
  public partial class LoginDialog : Window, INotifyPropertyChanged 
  {
    private SecurityInfo securityInfo;
    private LoginDialog.Mode mode;
    private Exception error;
 
    public event PropertyChangedEventHandler PropertyChanged;

    public LoginDialog(Window owner, ViewModel model)
    {
      this.Owner = owner;
      this.DataContext = (object) this;
      this.InitializeComponent();
    }

    public Exception Error
    {
      get => this.error;
      private set
      {
        this.error = value;
        this.OnPropertyChanged(nameof (Error), this.PropertyChanged);
      }
    }

    public bool ShowLogin(SecurityInfo securityInfo)
    {
        return this.ShowDialog(securityInfo, LoginDialog.Mode.LogoutLogin, 
            LStrings.Title_LoginDialog_Login);
    }

    public bool ShowLogout(SecurityInfo securityInfo)
    {
        return this.ShowDialog(securityInfo, LoginDialog.Mode.Logout, LStrings.Title_LoginDialog_Logout);
    }

        private bool ShowDialog(SecurityInfo securityInfo, LoginDialog.Mode mode, string title)
    {
      this.securityInfo = securityInfo;
      this.mode = mode;
      this.Title = title;
      this.brLogin.Navigate(securityInfo.CreateLogoutUri());
      this.ShowDialog();
      return this.mode == LoginDialog.Mode.Complete;
    }

    private void brLogin_Navigating(object sender, NavigatingCancelEventArgs args)
    {
      if (!(args.Uri != (Uri) null) 
                || !(args.Uri.GetLeftPart(UriPartial.Path) == "https://login.live.com/oauth20_desktop.srf"))
        return;
      args.Cancel = true;
      switch (this.mode)
      {
        case LoginDialog.Mode.LogoutLogin:
          this.mode = LoginDialog.Mode.Login;
          this.brLogin.Navigate(this.securityInfo.CreateLoginUri());
          break;
        case LoginDialog.Mode.Login:
          try
          {
            LoginResponse response = LoginResponse.Create(args.Uri);
            if (response.Error != null)
              throw new Exception(response.Error);
            this.securityInfo.Update(response);
            this.mode = LoginDialog.Mode.Complete;
            this.Close();
            break;
          }
          catch (Exception ex)
          {
            this.error = ex;
            break;
          }
        case LoginDialog.Mode.Logout:
          this.mode = LoginDialog.Mode.Complete;
          this.Close();
          break;
      }
    }

    private void brLogin_Navigated(object sender, NavigationEventArgs args)
    {
      try
      {
        object document = this.brLogin.Document;
        if (((string) document.GetType().GetProperty("url").GetValue(document)).StartsWith("res:"))
        {
          this.brLogin.Visibility = Visibility.Hidden;
          this.Error = new Exception("Navigation failed");
          return;
        }
      }
      catch
      {
      }
      this.brLogin.Visibility = Visibility.Visible;
      this.Error = (Exception) null;
    }

    private void CloseButtonClick(object sender, RoutedEventArgs e) => this.Close();

      

    private enum Mode
    {
      Unused,
      LogoutLogin,
      Login,
      Logout,
      Complete,
    }
  }
}
