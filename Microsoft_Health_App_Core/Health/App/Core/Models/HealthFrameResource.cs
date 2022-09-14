// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.HealthFrameResource
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.Models
{
  public class HealthFrameResource
  {
    public double NegativeHeaderHeight => (double) -HealthFrameResource.HeaderHeightConst;

    public static int HeaderHeightConst => 60;

    public double NavDrawerWidth => 280.0;

    public double NavDrawerNarrowWidth => 48.0;

    public double NegativeNavDrawerWidth => -this.NavDrawerWidth;

    public double SlidePanelDistance => -66.0;
  }
}
