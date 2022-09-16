// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.TileManagementControl
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using DesktopSyncApp.BindingConverters;
using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Diagnostics;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DesktopSyncApp
{
  public class TileManagementControl : SyncAppPageControl, IComponentConnector
  {
    private ScrollViewer TilesScroller;
    private ScrollBar TilesScrollBar;
    private DispatcherTimer autoScrollingTimer;
    private bool allowAutoScroll = true;
    private SelectableTile CurrentTile;
    private Point StartingPoint;
    private Cursor CustomCursor;
    private int OriginalPosition = -1;
    private bool Dragging;
    private SelectableTile BlankTile;
    private BandClass deviceType;
    internal ListView Tiles;
    internal AnimatedPageControl SettingPageManager;
    private bool _contentLoaded;

    private ObservableCollection<SelectableTile> stripManager => (ObservableCollection<SelectableTile>) this.Tiles.ItemsSource;

    public TileManagementControl(AppMainWindow parent)
      : base(parent, false)
    {
      this.InitializeComponent();
      this.ResetWindow();
    }

    public void ResetWindow()
    {
      this.Dragging = false;
      this.autoScrollingTimer = new DispatcherTimer();
      this.autoScrollingTimer.Interval = new TimeSpan(0, 0, 0, 0, 250);
      this.autoScrollingTimer.Tick += new EventHandler(this.autoScrollingTimer_Tick);
    }

    public void SetPages()
    {
      this.ShowSettingsPage((SyncAppPageControl) null);
      this.deviceType = this.parent.Model.DeviceManager.CurrentDevice.DeviceBandClass;
      this.parent.Model.StrapManager.DeviceType = this.deviceType;
      foreach (SelectableTile tile in (Collection<SelectableTile>) this.parent.Model.StrapManager.DisplayStrip)
      {
        string lower = tile.Strapp.Id.ToString().ToLower();
        if (lower == "22b1c099-f2be-4bac-8ed8-2d6b0b3c25d1".ToLower())
        {
          string[] phoneCallResponses = this.parent.Model.DeviceManager.CurrentDevice.CargoClient.GetPhoneCallResponses();
          tile.SetReplies(phoneCallResponses);
          tile.SettingsPage = (SyncAppPageControl) this.SetMessageSettingsPage(tile, Strings.Settings_Calls_Title, Strings.Settings_Calls_SubHeading1, Strings.Settings_Calls_SubHeading2);
        }
        else if (lower == "b4edbc35-027b-4d10-a797-1099cd2ad98a".ToLower())
        {
          string[] smsResponses = this.parent.Model.DeviceManager.CurrentDevice.CargoClient.GetSmsResponses();
          tile.SetReplies(smsResponses);
          tile.SettingsPage = (SyncAppPageControl) this.SetMessageSettingsPage(tile, Strings.Settings_SMS_Title, Strings.Settings_SMS_SubHeading1, Strings.Settings_SMS_SubHeading2);
        }
        else if (lower == "ec149021-ce45-40e9-aeee-08f86e4746a7".ToLower())
          tile.SettingsPage = (SyncAppPageControl) this.SetNotificationSettingsPage(tile, Strings.Settings_Calendar_Title, Strings.Settings_Calendar_SubHeading1);
        else if (lower == "d7fb5ff5-906a-4f2c-8269-dde6a75138c4".ToLower())
          tile.SettingsPage = (SyncAppPageControl) this.SetNotificationSettingsPage(tile, Strings.Settings_Cortana_Title, Strings.Settings_Cortana_SubHeading1);
        else if (lower == "823ba55a-7c98-4261-ad5e-929031289c6e".ToLower())
          tile.SettingsPage = (SyncAppPageControl) this.SetNotificationSettingsPage(tile, Strings.Settings_Email_Title, Strings.Settings_Email_SubHeading1);
        else if (lower == "fd06b486-bbda-4da5-9014-124936386237".ToLower())
          tile.SettingsPage = (SyncAppPageControl) this.SetNotificationSettingsPage(tile, Strings.Settings_Facebook_Title, Strings.Settings_Facebook_SubHeading1);
        else if (lower == "76b08699-2f2e-9041-96c2-1f4bfc7eab10".ToLower())
          tile.SettingsPage = (SyncAppPageControl) this.SetNotificationSettingsPage(tile, Strings.Settings_FacebookMessenger_Title, Strings.Settings_FacebookMessenger_SubHeading1);
        else if (lower == "4076b009-0455-4af7-a705-6d4acd45a556".ToLower())
          tile.SettingsPage = (SyncAppPageControl) this.SetNotificationSettingsPage(tile, Strings.Settings_NotificationCenter_Title, Strings.Settings_NotificationCenter_SubHeading1);
        else if (lower == "2e76a806-f509-4110-9c03-43dd2359d2ad".ToLower())
          tile.SettingsPage = (SyncAppPageControl) this.SetNotificationSettingsPage(tile, Strings.Settings_Twitter_Title, Strings.Settings_Twitter_SubHeading1);
        else if (lower == "96430fcb-0060-41cb-9de2-e00cac97f85d".ToLower())
          tile.SettingsPage = (SyncAppPageControl) this.SetBikeSettingsPage(tile, this.parent.Model.DeviceManager.CurrentDevice.DeviceBandClass, this.parent.Model.DeviceManager.CurrentDevice.CargoClient.GetBikeDisplayMetrics(), this.parent.Model.DeviceManager.CurrentDevice.CargoClient.GetBikeSplitMultiplier());
        else if (lower == "65bd93db-4293-46af-9a28-bdd6513b4677".ToLower())
          tile.SettingsPage = (SyncAppPageControl) this.SetRunSettingsPage(tile, this.parent.Model.DeviceManager.CurrentDevice.DeviceBandClass, this.parent.Model.DeviceManager.CurrentDevice.CargoClient.GetRunDisplayMetrics());
        else if (lower == "64a29f65-70bb-4f32-99a2-0f250a05d427".ToLower())
          tile.Icon = (BitmapImage) Application.Current.Resources[(object) "tile-starbucks"];
      }
      if (this.TilesScroller != null)
        this.TilesScroller.ScrollToHome();
      this.BlankTile = new SelectableTile(this.model.StrapManager, new AdminBandTile(Guid.Empty, string.Empty, (AdminTileSettings) 0));
      this.BlankTile.TileBackground = (Color) Application.Current.Resources[(object) "BackgroundDark"];
    }

    private NotificationSettings SetNotificationSettingsPage(
      SelectableTile tile,
      string title,
      string subheading1)
    {
      NotificationSettings notificationSettings = new NotificationSettings(this.parent, (SyncAppPageControl) this);
      notificationSettings.Title.Text = title;
      notificationSettings.NotificationsText.Text = subheading1;
      notificationSettings.NotificationsOn.SetBinding(ToggleButton.IsCheckedProperty, (BindingBase) new Binding("NotificationsOn")
      {
        Source = (object) tile,
        Mode = BindingMode.TwoWay
      });
      return notificationSettings;
    }

    private MessagingSettings SetMessageSettingsPage(
      SelectableTile tile,
      string title,
      string subheading1,
      string subheading2)
    {
      MessagingSettings messagingSettings = new MessagingSettings(this.parent, (SyncAppPageControl) this, tile.Strapp.Id);
      messagingSettings.Title.Text = title;
      messagingSettings.NotificationsText.Text = subheading1;
      messagingSettings.QuickResponse.Text = subheading2;
      messagingSettings.NotificationsOn.SetBinding(ToggleButton.IsCheckedProperty, (BindingBase) new Binding("NotificationsOn")
      {
        Source = (object) tile,
        Mode = BindingMode.TwoWay
      });
      messagingSettings.Reply1.SetBinding(TextBox.TextProperty, (BindingBase) this.GetReplyBinding("Reply1", (object) tile));
      messagingSettings.Reply2.SetBinding(TextBox.TextProperty, (BindingBase) this.GetReplyBinding("Reply2", (object) tile));
      messagingSettings.Reply3.SetBinding(TextBox.TextProperty, (BindingBase) this.GetReplyBinding("Reply3", (object) tile));
      messagingSettings.Reply4.SetBinding(TextBox.TextProperty, (BindingBase) this.GetReplyBinding("Reply4", (object) tile));
      return messagingSettings;
    }

    public Binding GetReplyBinding(string field, object source) => new Binding(field)
    {
      Source = source,
      Mode = BindingMode.TwoWay,
      Converter = (IValueConverter) new CookedLimitedStringConverter(),
      ConverterParameter = (object) 160
    };

    public Binding GetBikeBinding(string field, object source) => new Binding(field)
    {
      Source = source,
      Mode = BindingMode.TwoWay,
      Converter = (IValueConverter) new BikeMetricComboBoxIndexConverter()
    };

    public Binding GetRunBinding(string field, object source) => new Binding(field)
    {
      Source = source,
      Mode = BindingMode.TwoWay,
      Converter = (IValueConverter) new RunMetricComboBoxIndexConverter()
    };

    private BikeSettings SetBikeSettingsPage(
      SelectableTile tile,
      BandClass deviceType,
      CargoBikeDisplayMetrics metrics,
      int split)
    {
      this.model.StrapManager.BikeManager = new BikeSettingsManager(this.model.StrapManager, deviceType, metrics, split, this.model.LoginInfo.UserProfile.HeightDisplayUnits);
      return new BikeSettings(this.parent, (SyncAppPageControl) this, this.model.StrapManager.BikeManager);
    }

    private RunSettings SetRunSettingsPage(
      SelectableTile tile,
      BandClass deviceType,
      CargoRunDisplayMetrics metrics)
    {
      this.model.StrapManager.RunManager = new RunSettingsManager(this.model.StrapManager, deviceType, metrics, this.model.LoginInfo.UserProfile.HeightDisplayUnits);
      return new RunSettings(this.parent, (SyncAppPageControl) this, this.model.StrapManager.RunManager);
    }

    private async void SaveOrder_Click(object sender, RoutedEventArgs e)
    {
      using (new DisposableAction((Action) (() => this.model.StrapManager.UpdatingStartStrip = true), (Action) (() => this.model.StrapManager.UpdatingStartStrip = false)))
      {
        StartStrip nwo = this.model.StrapManager.UpdateCurrentStartStrip();
        await this.model.DeviceManager.CurrentDevice.CargoClient.SetStartStripAsync(nwo);
        SelectableTile strap1 = this.model.StrapManager.GetStrap(new Guid("96430fcb-0060-41cb-9de2-e00cac97f85d"));
        SelectableTile strap2 = this.model.StrapManager.GetStrap(new Guid("65bd93db-4293-46af-9a28-bdd6513b4677"));
        SelectableTile strap3 = this.model.StrapManager.GetStrap(new Guid("b4edbc35-027b-4d10-a797-1099cd2ad98a"));
        SelectableTile strap4 = this.model.StrapManager.GetStrap(new Guid("22b1c099-f2be-4bac-8ed8-2d6b0b3c25d1"));
        List<string> stringList = new List<string>();
        foreach (AdminBandTile adminBandTile in nwo)
          stringList.Add(adminBandTile.Name);
        ApplicationTelemetry.LogTilesChanged(nwo.Count, true, (IEnumerable<string>) stringList);
        if (strap3 != null && strap3.RepliesChanged)
        {
          this.model.DeviceManager.CurrentDevice.CargoClient.SetSmsResponses(strap3.Reply1.Trim(), strap3.Reply2.Trim(), strap3.Reply3.Trim(), strap3.Reply4.Trim());
          ApplicationTelemetry.LogMessagingCustomResponseChange();
        }
        if (strap4 != null && strap4.RepliesChanged)
        {
          this.model.DeviceManager.CurrentDevice.CargoClient.SetPhoneCallResponses(strap4.Reply1.Trim(), strap4.Reply2.Trim(), strap4.Reply3.Trim(), strap4.Reply4.Trim());
          ApplicationTelemetry.LogCallCustomResponseChange();
        }
        if (strap1 != null)
        {
          if (this.model.StrapManager.BikeManager.BikeMetricsChanged)
            this.model.DeviceManager.CurrentDevice.CargoClient.SetBikeDisplayMetrics(this.model.StrapManager.BikeManager.BikeDisplayMetrics);
          if (this.model.StrapManager.BikeManager.SplitChanged)
            this.model.DeviceManager.CurrentDevice.CargoClient.SetBikeSplitMultiplier(this.model.StrapManager.BikeManager.Split);
          ApplicationTelemetry.LogBikeDataPointsChange();
        }
        if (strap2 != null && this.model.StrapManager.RunManager.RunMetricsChanged)
        {
          this.model.DeviceManager.CurrentDevice.CargoClient.SetRunDisplayMetrics(this.model.StrapManager.RunManager.RunDisplayMetrics);
          ApplicationTelemetry.LogRunDataPointsChange();
        }
        this.model.ShowSyncCommand.Execute((object) null);
        nwo = (StartStrip) null;
      }
    }

    public void ShowSettingsPage(SyncAppPageControl settingsPage)
    {
      if (settingsPage == null)
      {
        this.SettingPageManager.Visibility = Visibility.Hidden;
      }
      else
      {
        this.SettingPageManager.ShowPage(settingsPage, false);
        this.SettingPageManager.Visibility = Visibility.Visible;
      }
    }

    private void autoScrollingTimer_Tick(object sender, EventArgs e) => this.allowAutoScroll = true;

    private void ListView_DragEnter(object sender, DragEventArgs e) => e.Effects = DragDropEffects.Move;

    private void ListView_DragOver(object sender, DragEventArgs e)
    {
      e.Effects = DragDropEffects.Move;
      if (!this.allowAutoScroll)
        return;
      Point position = e.GetPosition((IInputElement) this.Tiles);
      if (TileManagementControl.GetObjectDataFromPoint(this.Tiles, position) is SelectableTile objectDataFromPoint)
      {
        int num = this.stripManager.IndexOf(this.BlankTile);
        if (num < 0)
          this.stripManager.Insert(num, this.BlankTile);
        else if (this.BlankTile != objectDataFromPoint)
          this.stripManager.Move(num, this.stripManager.IndexOf(objectDataFromPoint));
      }
      if (position.X > this.Tiles.ActualWidth * 0.95)
      {
        if (this.TilesScrollBar.Value == this.TilesScrollBar.Maximum)
          return;
        this.allowAutoScroll = false;
        this.autoScrollingTimer.Start();
        this.TilesScroller.LineRight();
      }
      else
      {
        if (position.X >= this.Tiles.ActualWidth * 0.05 || this.TilesScrollBar.Value == this.TilesScrollBar.Minimum)
          return;
        this.allowAutoScroll = false;
        this.autoScrollingTimer.Start();
        this.TilesScroller.LineLeft();
      }
    }

    private void ListView_Drop(object sender, DragEventArgs e)
    {
      SelectableTile data = e.Data.GetData(typeof (SelectableTile)) as SelectableTile;
      SelectableTile objectDataFromPoint = TileManagementControl.GetObjectDataFromPoint(this.Tiles, e.GetPosition((IInputElement) this.Tiles)) as SelectableTile;
      if (data != null)
      {
        if (objectDataFromPoint == null && this.Tiles.Items.Count > 1)
        {
          this.stripManager.Add(data);
          this.Tiles.SelectedItem = (object) data;
          this.model.StrapManager.NeedToSave = true;
        }
        if (objectDataFromPoint != null && data != objectDataFromPoint)
        {
          this.stripManager.Insert(this.stripManager.IndexOf(objectDataFromPoint), data);
          this.Tiles.SelectedItem = (object) data;
          this.model.StrapManager.NeedToSave = true;
        }
      }
      this.CustomCursor = (Cursor) null;
      this.Dragging = false;
    }

    private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Point position = e.GetPosition((IInputElement) this.Tiles);
      SelectableTile objectDataFromPoint = TileManagementControl.GetObjectDataFromPoint(this.Tiles, position) as SelectableTile;
      this.CurrentTile = (SelectableTile) null;
      if (objectDataFromPoint == null)
        return;
      this.Tiles.SelectedItem = (object) objectDataFromPoint;
      this.CurrentTile = objectDataFromPoint;
      this.StartingPoint = position;
      this.OriginalPosition = this.Tiles.Items.IndexOf((object) objectDataFromPoint);
    }

    private static object GetObjectDataFromPoint(ListView source, Point point, bool returnElement = false)
    {
      if (source.InputHitTest(point) is UIElement uiElement)
      {
        object obj = DependencyProperty.UnsetValue;
        while (obj == DependencyProperty.UnsetValue)
        {
          obj = source.ItemContainerGenerator.ItemFromContainer((DependencyObject) uiElement);
          if (obj == DependencyProperty.UnsetValue)
            uiElement = VisualTreeHelper.GetParent((DependencyObject) uiElement) as UIElement;
          if (uiElement == source)
            return (object) null;
        }
        if (obj != DependencyProperty.UnsetValue)
          return !returnElement ? obj : (object) uiElement;
      }
      return (object) null;
    }

    private void SettingsEdit_Click(object sender, RoutedEventArgs e)
    {
      if (this.Tiles.SelectedItem == null)
        return;
      SelectableTile selectedItem = (SelectableTile) this.Tiles.SelectedItem;
      if (!selectedItem.HasSettingsPage)
        return;
      ((TileManagementControl) this.container).ShowSettingsPage(selectedItem.SettingsPage);
    }

    private void TileDragDropFeedback(object sender, GiveFeedbackEventArgs e)
    {
      if (e.Effects == DragDropEffects.Move)
      {
        if (this.CustomCursor == null)
          this.CustomCursor = CursorUIElement.CreateCursor(this.CurrentTile.Icon == null ? (UIElement) new DisplayTile(this.CurrentTile.StrapImage, this.model.StrapManager.TileBackground) : (UIElement) new DisplayTile(this.CurrentTile.Icon, this.model.StrapManager.TileBackground), 42, 42);
        if (this.CustomCursor != null)
        {
          e.UseDefaultCursors = false;
          Mouse.SetCursor(this.CustomCursor);
        }
      }
      else
        e.UseDefaultCursors = true;
      e.Handled = true;
    }

    private void ListView_MouseMove(object sender, MouseEventArgs e)
    {
      if (e.LeftButton != MouseButtonState.Pressed || this.CurrentTile == null || this.Dragging)
        return;
      Point position1 = e.GetPosition((IInputElement) this.Tiles);
      if (Math.Abs(this.StartingPoint.X - position1.X) <= 10.0 && Math.Abs(this.StartingPoint.Y - position1.Y) <= 10.0)
        return;
      this.CustomCursor = (Cursor) null;
      DisplayTile displayTile = this.CurrentTile.Icon == null ? new DisplayTile(this.CurrentTile.StrapImage, this.model.StrapManager.TileBackground) : new DisplayTile(this.CurrentTile.Icon, this.model.StrapManager.TileBackground);
      Point position2 = e.GetPosition((IInputElement) this.Tiles);
      object objectDataFromPoint = TileManagementControl.GetObjectDataFromPoint(this.Tiles, e.GetPosition((IInputElement) this.Tiles), true);
      e.GetPosition((IInputElement) objectDataFromPoint);
      int result = 0;
      Math.DivRem((int) position2.X, 72, out result);
      this.CustomCursor = CursorUIElement.CreateCursor((UIElement) displayTile, result, (int) position2.Y);
      this.Dragging = true;
      this.stripManager.Remove(this.CurrentTile);
      this.stripManager.Insert(this.OriginalPosition, this.BlankTile);
      int num = (int) DragDrop.DoDragDrop((DependencyObject) this.Tiles, (object) this.CurrentTile, DragDropEffects.Move);
      this.Dragging = false;
      this.stripManager.Remove(this.BlankTile);
      if (num != 0)
        return;
      this.stripManager.Insert(this.OriginalPosition, this.CurrentTile);
    }

    private void TilesScroller_Loaded(object sender, RoutedEventArgs e)
    {
      this.TilesScroller = ((Decorator) VisualTreeHelper.GetChild((DependencyObject) this.Tiles, 0)).Child as ScrollViewer;
      this.TilesScrollBar = this.TilesScroller.Template.FindName("PART_HorizontalScrollBar", (FrameworkElement) this.TilesScroller) as ScrollBar;
    }

    private void Tiles_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (!(TileManagementControl.GetObjectDataFromPoint(this.Tiles, e.GetPosition((IInputElement) this.Tiles)) is SelectableTile objectDataFromPoint) || !objectDataFromPoint.HasSettingsPage)
        return;
      this.ShowSettingsPage(objectDataFromPoint.SettingsPage);
      this.SettingPageManager.Visibility = Visibility.Visible;
    }

    private void Tiles_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
      e.Handled = true;
      if (e.Delta > 0)
      {
        this.TilesScroller.LineLeft();
      }
      else
      {
        if (e.Delta >= 0)
          return;
        this.TilesScroller.LineRight();
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Microsoft Band Sync;component/pagecontrols/tilemanagementcontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 1)
      {
        if (connectionId == 2)
          this.SettingPageManager = (AnimatedPageControl) target;
        else
          this._contentLoaded = true;
      }
      else
      {
        this.Tiles = (ListView) target;
        this.Tiles.DragEnter += new DragEventHandler(this.ListView_DragEnter);
        this.Tiles.DragOver += new DragEventHandler(this.ListView_DragOver);
        this.Tiles.Drop += new DragEventHandler(this.ListView_Drop);
        this.Tiles.GiveFeedback += new GiveFeedbackEventHandler(this.TileDragDropFeedback);
        this.Tiles.MouseMove += new MouseEventHandler(this.ListView_MouseMove);
        this.Tiles.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.ListView_PreviewMouseLeftButtonDown);
        this.Tiles.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.Tiles_PreviewMouseLeftButtonUp);
        this.Tiles.PreviewMouseWheel += new MouseWheelEventHandler(this.Tiles_PreviewMouseWheel);
        this.Tiles.Loaded += new RoutedEventHandler(this.TilesScroller_Loaded);
      }
    }

    private enum ScrollDirection
    {
      None,
      Left,
      Right,
    }
  }
}
