// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.ExerciseTag
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class ExerciseTag
  {
    [DataMember]
    public string Text { get; set; }

    [DataMember]
    public Guid ExerciseTypeId { get; set; }

    [DataMember]
    public bool IsDefault { get; set; }

    [DataMember]
    public bool IsChecked { get; set; }

    [DataMember]
    public HrAlgorithm Algorithm { get; set; }

    [DataMember]
    public uint Flags { get; set; }
  }
}
