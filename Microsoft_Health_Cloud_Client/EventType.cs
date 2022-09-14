// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.EventType
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;

namespace Microsoft.Health.Cloud.Client
{
  public enum EventType
  {
    [UnknownEnumValue] Unknown = -2, // 0xFFFFFFFE
    Best = -1, // 0xFFFFFFFF
    None = 0,
    Driving = 1,
    Running = 2,
    Sleeping = 3,
    Workout = 4,
    Walking = 5,
    GuidedWorkout = 6,
    Biking = 7,
    Hike = 8,
    Golf = 9,
  }
}
