// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Coaching.RecommendationChoiceViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Cirrious.MvvmCross.ViewModels;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Providers.Coaching;

namespace Microsoft.Health.App.Core.ViewModels.Coaching
{
  public class RecommendationChoiceViewModel : MvxNotifyPropertyChanged
  {
    private string text;

    public RecommendationChoiceViewModel(SleepPlanRecommendationOption option, string text)
    {
      Assert.EnumIsDefined<SleepPlanRecommendationOption>(option, nameof (option));
      Assert.ParamIsNotNullOrEmpty(text, nameof (text));
      this.Option = option;
      this.Text = text;
    }

    public RecommendationChoiceViewModel(
      SleepPlanRecommendationOption option,
      string text,
      string proposedTime)
      : this(option, text)
    {
      Assert.ParamIsNotNullOrEmpty(proposedTime, nameof (proposedTime));
      this.ProposedTime = proposedTime;
    }

    public SleepPlanRecommendationOption Option { get; }

    public string Text { get; }

    public string ProposedTime { get; }
  }
}
