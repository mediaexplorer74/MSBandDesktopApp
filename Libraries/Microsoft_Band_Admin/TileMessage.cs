// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.TileMessage
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;

namespace Microsoft.Band.Admin
{
  public sealed class TileMessage
  {
    private string title;
    private string body;
    public bool timestampHasValue;
    private DateTime timestamp;
    public NotificationFlags flags;

    public TileMessage(string title, string body)
    {
      this.Title = title;
      this.Body = body;
      this.timestampHasValue = false;
    }

    public TileMessage(string title, string body, DateTime timestamp, NotificationFlags flagbits = NotificationFlags.UnmodifiedNotificationSettings)
    {
      this.Title = title;
      this.Body = body;
      this.Timestamp = timestamp;
      this.Flags = flagbits;
    }

    public string Title
    {
      get => this.title;
      set => this.title = value != null ? value : throw new ArgumentNullException(nameof (Title));
    }

    public string Body
    {
      get => this.body;
      set => this.body = value != null ? value : throw new ArgumentNullException(nameof (Body));
    }

    public DateTime Timestamp
    {
      get => this.timestamp;
      set
      {
        this.timestamp = value;
        this.timestampHasValue = true;
      }
    }

    public NotificationFlags Flags
    {
      get => this.flags;
      set => this.flags = value;
    }
  }
}
