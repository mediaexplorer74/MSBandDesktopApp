// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.FavoriteWorkout
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class FavoriteWorkout
  {
    [DataMember]
    public string WorkoutPlanId { get; set; }

    [DataMember]
    public string UserId { get; set; }

    [DataMember]
    public string LastUpdateTime { get; set; }

    [DataMember]
    public string TimeFavorited { get; set; }

    [DataMember]
    public int NumberOfTimesCompleted { get; set; }

    [DataMember]
    public int Status { get; set; }

    [DataMember]
    public int CurrentInstanceId { get; set; }

    [DataMember]
    public bool IsSubscribed { get; set; }

    [DataMember]
    public string TimeSubscribed { get; set; }

    [DataMember]
    public FavoriteWorkoutDetails WorkoutPlanBrowseDetails { get; set; }
  }
}
