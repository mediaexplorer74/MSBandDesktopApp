// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.HrAlgorithm
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using System;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  public enum HrAlgorithm : byte
  {
    [UnknownEnumValue, EnumMember(Value = "ALGO_RUN_DEFAULT")] RunDefault,
    [EnumMember(Value = "ALGO_RUN_MULTI_REG_WALK")] RunMultiRegWalk,
    [EnumMember(Value = "ALGO_RUN_MULTI_ELL_WALK")] RunMultiEllWalk,
    [EnumMember(Value = "ALGO_RUN_MULTI_ROWING")] RunMultiRowing,
    [Obsolete("Use ALGO_RUN_MULTI_HANDHOLD instead.", true), EnumMember(Value = "ALGO_RUN_SINGLE_BIKING")] RunSingleBiking,
    [EnumMember(Value = "ALGO_RUN_MULTI_SITUP")] RunMultiSitup,
    [EnumMember(Value = "ALGO_RUN_MULTI_RUN")] RunMultiRun,
    [EnumMember(Value = "ALGO_RUN_MULTI_HANDHOLD")] RunMultiHandhold,
    [EnumMember(Value = "ALGO_RUN_MULTI_SWIMMING")] RunMultiSwimming,
  }
}
