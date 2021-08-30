// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.DesignGolfScorecardHoleViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.Cloud.Client;
using System;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  public sealed class DesignGolfScorecardHoleViewModel : GolfScorecardHoleViewModel
  {
    public DesignGolfScorecardHoleViewModel()
      : base(18, 4, Length.FromYards(372.0), "372", "18", new int?(5), new int?(1), new StyledSpan("1h 32m", new object[0]), new StyledSpan("32m", new object[0]), new StyledSpan("1234", new object[0]), new StyledSpan("32 cals", new object[0]), new StyledSpan("102 bpm", new object[0]), new StyledSpan("139 bpm", new object[0]), new StyledSpan("115 bpm", new object[0]), (Uri) null)
    {
    }
  }
}
