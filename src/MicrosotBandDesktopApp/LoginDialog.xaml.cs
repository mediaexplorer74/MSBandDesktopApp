// Decompiled with JetBrains decompiler
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
  public partial class LoginDialog : Window, INotifyPropertyChanged, IComponentConnector
  {
    private SecurityInfo securityInfo;
    private LoginDialog.Mode mode;
    private Exception error;
    //internal WebBrowser brLogin;
    //private bool _contentLoaded;

    public event PropertyChangedEventHandler PropertyChanged;

        //TODO
    public LoginDialog(Window owner, ViewModel1 model)
    {
      this.Owner = owner;
      this.DataContext = (object) this;
      //this.InitializeComponent(); // TODO
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

    public bool ShowLogin(SecurityInfo securityInfo) => this.ShowDialog(securityInfo, LoginDialog.Mode.LogoutLogin, Strings.Title_LoginDialog_Login);

    public bool ShowLogout(SecurityInfo securityInfo) => this.ShowDialog(securityInfo, LoginDialog.Mode.Logout, Strings.Title_LoginDialog_Logout);

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
      if (!(args.Uri != (Uri) null) || !(args.Uri.GetLeftPart(UriPartial.Path) == "https://login.live.com/oauth20_desktop.srf"))
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

        /*
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft Band Sync;component/logindialog.xaml", UriKind.Relative));
    }
        */

        /*
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);
        */

    /*
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        this.brLogin = (WebBrowser) target;
        this.brLogin.Navigated += new NavigatedEventHandler(this.brLogin_Navigated);
        this.brLogin.Navigating += new NavigatingCancelEventHandler(this.brLogin_Navigating);
      }
      else
        this._contentLoaded = true;
    }
    */

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
