// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Band.BandMessage
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Band
{
  public abstract class BandMessage
  {
    protected BandMessage(string displayName, string body, DateTimeOffset timestamp, int id)
    {
      this.DisplayName = displayName;
      this.Body = body;
      this.Timestamp = timestamp;
      this.Id = id;
    }

    public string DisplayName { get; }

    public string Body { get; }

    public DateTimeOffset Timestamp { get; }

    public int Id { get; }
  }
}
