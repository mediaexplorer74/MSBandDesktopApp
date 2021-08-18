// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.SelectableTile
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band;
using Microsoft.Band.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DesktopSyncApp
{
  public class SelectableTile : INotifyPropertyChanged
  {
    private StartStripManager manager;
    private string[] originalResponses;
    private string[] savedResponses;
    public bool notificationsOn;
    private Color? backgroundColor;
    private AdminBandTile cargoStrap;

    public SelectableTile(StartStripManager startStripManager, AdminBandTile strapp)
    {
      this.manager = startStripManager;
      this.Strapp = strapp;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public BitmapImage Icon { get; set; }

    private bool BadgingChecked { get; set; }

    private bool CustomColor { get; set; }

    public List<string> Responses { get; set; }

    public string Title { get; set; }

    public string SettingsTitle { get; set; }

    public string DetailsTitle { get; set; }

    public string NotificationsText1 { get; set; }

    public string NotificationsText2 { get; set; }

    public string NotificationsText3 { get; set; }

    public string Reply1 { get; set; }

    public string Reply2 { get; set; }

    public string Reply3 { get; set; }

    public string Reply4 { get; set; }

    public bool HasNotifications { get; set; }

    public bool NotificationsOn
    {
      get => this.notificationsOn;
      set
      {
        if (this.notificationsOn == value)
          return;
        this.notificationsOn = value;
        this.manager.NeedToSave = true;
        this.OnPropertyChanged(nameof (NotificationsOn), this.PropertyChanged);
      }
    }

    public SyncAppPageControl SettingsPage { get; set; }

    public AdminBandTile Strapp
    {
      get => this.cargoStrap;
      set
      {
        if (this.cargoStrap == value)
          return;
        this.NotificationsOn = ((Enum) (object) value.SettingsMask).HasFlag((Enum) (object) (AdminTileSettings) 1);
        this.BadgingChecked = ((Enum) (object) value.SettingsMask).HasFlag((Enum) (object) (AdminTileSettings) 2);
        this.CustomColor = ((Enum) (object) value.SettingsMask).HasFlag((Enum) (object) (AdminTileSettings) 4);
        this.cargoStrap = value;
        this.OnPropertyChanged(nameof (Strapp), this.PropertyChanged);
      }
    }

    public AdminTileSettings TileSettings
    {
      get
      {
        AdminTileSettings adminTileSettings = this.cargoStrap.SettingsMask;
        if (this.HasSettingsPage)
          adminTileSettings = (AdminTileSettings) ((AdminTileSettings) ((AdminTileSettings) ((AdminTileSettings) 0 | (this.BadgingChecked ? 2 : 0)) | (this.NotificationsOn ? 1 : 0)) | (this.CustomColor ? 4 : 0));
        return adminTileSettings;
      }
    }

    public bool HasSettingsPage => this.SettingsPage != null;

    public object Image => this.Icon != null ? (object) this.Icon : (object) this.StrapImage;

    public WriteableBitmap StrapImage => this.Strapp.Image == null ? (WriteableBitmap) null : BandIconExtensions.ToWriteableBitmap(this.Strapp.Image);

    public string Name => this.Strapp.Name;

    public Color TileBackground
    {
      get
      {
        Color tileBackground = this.manager.TileBackground;
        if (this.backgroundColor.HasValue)
          tileBackground = this.backgroundColor.Value;
        return tileBackground;
      }
      set
      {
        Color? backgroundColor = this.backgroundColor;
        Color color = value;
        if ((backgroundColor.HasValue ? (backgroundColor.HasValue ? (backgroundColor.GetValueOrDefault() != color ? 1 : 0) : 0) : 1) == 0)
          return;
        this.backgroundColor = new Color?(value);
        this.OnPropertyChanged(nameof (TileBackground), this.PropertyChanged);
      }
    }

    public void SetReplies(string[] replies)
    {
      this.originalResponses = replies;
      this.savedResponses = replies;
      this.Reply1 = this.originalResponses.Length >= 1 ? this.originalResponses[0] : "";
      this.Reply2 = this.originalResponses.Length >= 2 ? this.originalResponses[1] : "";
      this.Reply3 = this.originalResponses.Length >= 3 ? this.originalResponses[2] : "";
      this.Reply4 = this.originalResponses.Length >= 4 ? this.originalResponses[3] : "";
      this.RaiseReplyPropertiesChanged();
    }

    public void SaveReplies()
    {
      this.savedResponses = new string[4]{ "", "", "", "" };
      this.savedResponses[0] = this.Reply1.Trim();
      this.savedResponses[1] = this.Reply2.Trim();
      this.savedResponses[2] = this.Reply3.Trim();
      this.savedResponses[3] = this.Reply4.Trim();
      this.manager.NeedToSave = true;
      this.RaiseReplyPropertiesChanged();
    }

    public void CancelReplies()
    {
      this.Reply1 = this.savedResponses.Length >= 1 ? this.savedResponses[0] : "";
      this.Reply2 = this.savedResponses.Length >= 2 ? this.savedResponses[1] : "";
      this.Reply3 = this.savedResponses.Length >= 3 ? this.savedResponses[2] : "";
      this.Reply4 = this.savedResponses.Length >= 4 ? this.savedResponses[3] : "";
      this.RaiseReplyPropertiesChanged();
    }

    private void RaiseReplyPropertiesChanged()
    {
      this.OnPropertyChanged("Reply1", this.PropertyChanged);
      this.OnPropertyChanged("Reply2", this.PropertyChanged);
      this.OnPropertyChanged("Reply3", this.PropertyChanged);
      this.OnPropertyChanged("Reply4", this.PropertyChanged);
    }

    public bool RepliesChanged
    {
      get
      {
        if (this.originalResponses == null || this.originalResponses.Length != 4 || this.Reply1 == null || this.Reply2 == null || this.Reply3 == null || this.Reply4 == null)
          return false;
        return this.Reply1.Trim() != this.originalResponses[0].Trim() || this.Reply2.Trim() != this.originalResponses[1].Trim() || this.Reply3.Trim() != this.originalResponses[2].Trim() || this.Reply4.Trim() != this.originalResponses[3].Trim();
      }
    }
  }
}
