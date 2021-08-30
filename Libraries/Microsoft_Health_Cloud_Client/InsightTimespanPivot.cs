// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.InsightTimespanPivot
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;

namespace Microsoft.Health.Cloud.Client
{
  public enum InsightTimespanPivot
  {
    [UnknownEnumValue] Unknown = -1, // 0xFFFFFFFF
    Unspecified = 0,
    Event = 1,
    Day = 2,
    Week = 3,
    Month = 4,
    Year = 5,
    Lifetime = 6,
  }
}
