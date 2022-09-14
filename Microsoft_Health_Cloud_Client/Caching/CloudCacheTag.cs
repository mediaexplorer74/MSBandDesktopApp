// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Caching.CloudCacheTag
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Globalization;

namespace Microsoft.Health.Cloud.Client.Caching
{
  public static class CloudCacheTag
  {
    public const string AllEventTypes = "EventType=All";
    private const string EventIdFormat = "EventId={0}";
    private const string EventTypeFormat = "EventType={0}";

    public static string ForEventId(string eventId) => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "EventId={0}", new object[1]
    {
      (object) eventId
    });

    public static string ForEventType(EventType eventType) => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "EventType={0}", new object[1]
    {
      (object) eventType
    });
  }
}
