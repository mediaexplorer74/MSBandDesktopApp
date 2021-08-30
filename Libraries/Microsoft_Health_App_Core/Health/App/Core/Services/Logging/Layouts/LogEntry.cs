// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Logging.Layouts.LogEntry
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Services.Logging.Layouts
{
  [DataContract]
  public class LogEntry
  {
    [DataMember(Name = "time")]
    public DateTimeOffset Time { get; set; }

    [DataMember(Name = "level")]
    public string Level { get; set; }

    [DataMember(Name = "category")]
    public string Category { get; set; }

    [DataMember(Name = "message")]
    public string Message { get; set; }

    [DataMember(Name = "exception")]
    public LogEntryException Exception { get; set; }
  }
}
