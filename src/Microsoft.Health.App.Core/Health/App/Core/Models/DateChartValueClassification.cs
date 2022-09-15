// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.DateChartValueClassification
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.Models
{
  public enum DateChartValueClassification
  {
    Grey = -1, // 0xFFFFFFFF
    Activity = 0,
    AboveAverage = 1,
    Highlight = 2,
    Awake = 3,
    LightSleep = 4,
    DeepSleep = 5,
    Unknown = 6,
  }
}
