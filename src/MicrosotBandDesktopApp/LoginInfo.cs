// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.LoginInfo
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System.ComponentModel;

namespace DesktopSyncApp
{
  public class LoginInfo : INotifyPropertyChanged
  {
    private UserProfileSurrogate userProfile;
    private bool updatingUserProfile;
    private UserProfileEdit userProfileEdit;
    private ErrorInfo lastProfileUpdateError;

    public event PropertyChangedEventHandler PropertyChanged;

    public event PropertyValueChangedEventHandler UserProfileEditChange;

    public UserProfileSurrogate UserProfile
    {
      get => this.userProfile;
      set
      {
        if (this.userProfile == value)
          return;
        this.UserProfileEdit = (UserProfileEdit) null;
        this.userProfile = value;
        this.OnPropertyChanged(nameof (UserProfile), this.PropertyChanged);
      }
    }

    public bool UpdatingUserProfile
    {
      get => this.updatingUserProfile;
      set
      {
        if (this.updatingUserProfile == value)
          return;
        this.updatingUserProfile = value;
        this.OnPropertyChanged(nameof (UpdatingUserProfile), this.PropertyChanged);
      }
    }

    public ErrorInfo LastProfileUpdateError
    {
      get => this.lastProfileUpdateError;
      set
      {
        if (this.lastProfileUpdateError == value)
          return;
        this.lastProfileUpdateError = value;
        this.OnPropertyChanged(nameof (LastProfileUpdateError), this.PropertyChanged);
      }
    }

    public UserProfileEdit UserProfileEdit
    {
      get => this.userProfileEdit;
      set
      {
        UserProfileEdit userProfileEdit = this.userProfileEdit;
        if (userProfileEdit == value)
          return;
        this.userProfileEdit = value;
        this.OnPropertyChanged(nameof (UserProfileEdit), this.PropertyChanged, this.UserProfileEditChange, (object) userProfileEdit, (object) value);
      }
    }

    public ServiceInfo ServiceInfo { get; set; }
  }
}
