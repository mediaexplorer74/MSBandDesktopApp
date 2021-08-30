﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Bing.HealthAndFitness.CircuitDetails
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client.Bing.HealthAndFitness
{
  [DataContract]
  public class CircuitDetails : DeserializableObjectBase
  {
    [DataMember(Name = "exs")]
    public IList<Exercise> Exercises { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "grouptype")]
    public CircuitGroupType GroupType { get; set; }

    [DataMember(Name = "kcircuittype")]
    public CircuitType CircuitType { get; set; }

    [DataMember(Name = "kcompletionvalue")]
    public int CompletionValue { get; set; }

    [DataMember(Name = "kcompletiontype")]
    public CompletionType CompletionType { get; set; }

    [DataMember(Name = "kexercisetraversaltype")]
    public ExerciseTraversalType TraversalType { get; set; }

    [DataMember(Name = "droplastrest")]
    public bool DropLastRest { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsNotNull("name", (object) this.Name);
    }

    [System.Runtime.Serialization.OnDeserializing]
    private void OnDeserializing(StreamingContext context) => this.GroupType = CircuitGroupType.List;
  }
}