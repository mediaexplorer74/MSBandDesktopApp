// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.Goal
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.Models
{
  public class Goal
  {
    public Goal(
      string goalId,
      string iconSource,
      bool isImplemented,
      string unit,
      int intervalSteps,
      int minVal,
      int maxVal,
      int defaultVal,
      string backgroundColor,
      string selectionMessage)
    {
      this.GoalId = goalId;
      this.IconSource = iconSource;
      this.IsImplemented = isImplemented;
      this.Unit = unit;
      this.MinVal = minVal;
      this.MaxVal = maxVal;
      this.IntervalSteps = intervalSteps;
      this.DefaultValue = defaultVal;
      this.BackgroundColor = backgroundColor;
      this.SelectionMessage = selectionMessage;
    }

    public string GoalId { get; set; }

    public string IconSource { get; set; }

    public bool IsImplemented { get; set; }

    public string Unit { get; set; }

    public int IntervalSteps { get; set; }

    public int DefaultValue { get; set; }

    public int MinVal { get; set; }

    public int MaxVal { get; set; }

    public string BackgroundColor { get; set; }

    public string SelectionMessage { get; set; }
  }
}
