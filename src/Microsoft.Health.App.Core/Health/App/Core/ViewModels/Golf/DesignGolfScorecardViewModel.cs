// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.DesignGolfScorecardViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Documents;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.Cloud.Client;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  public sealed class DesignGolfScorecardViewModel
  {
    public DesignGolfScorecardViewModel()
    {
      ServiceLocator.Current.GetInstance<IFormattingService>();
      ServiceLocator.Current.GetInstance<IEnvironmentService>();
      this.Elements = (IEnumerable<GolfScorecardElementViewModel>) new GolfScorecardElementViewModel[21]
      {
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(1, 4, Length.FromYards(408.0), "408", "18", new int?(4), new int?(0), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(2, 4, Length.FromYards(442.0), "442", (string) null, new int?(4), new int?(0), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(3, 5, Length.FromYards(558.0), "558", "7", new int?(5), new int?(0), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(4, 3, Length.FromYards(183.0), "183", "8", new int?(5), new int?(2), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(5, 4, Length.FromYards(470.0), "470", "11", new int?(3), new int?(-1), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(6, 4, Length.FromYards(432.0), "432", "13", new int?(5), new int?(1), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(7, 3, Length.FromYards(215.0), "215", "4", new int?(4), new int?(1), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(8, 4, Length.FromYards(475.0), "475", "1", new int?(3), new int?(-1), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(9, 4, Length.FromYards(453.0), "453", "16", new int?(5), new int?(1), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHeaderViewModel(GolfScorecardHeaderType.Outward, 35, Length.FromYards(3636.0), "3636", new int?(38)),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(10, 4, Length.FromYards(428.0), "428", "17", new int?(4), new int?(0), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(11, 4, Length.FromYards(472.0), "472", "9", new int?(5), new int?(1), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(12, 3, Length.FromYards(192.0), "192", "6", new int?(3), new int?(0), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(13, 5, Length.FromYards(558.0), "558", "15", new int?(5), new int?(0), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(14, 4, Length.FromYards(490.0), "490", "10", new int?(6), new int?(2), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(15, 5, Length.FromYards(553.0), "553", "14", new int?(8), new int?(3), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(16, 3, Length.FromYards(163.0), "163", "5", new int?(3), new int?(0), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(17, 4, Length.FromYards(332.0), "332", "3", new int?(5), new int?(1), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHoleViewModel(18, 4, Length.FromYards(442.0), "442", "12", new int?(), new int?(), (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (StyledSpan) null, (Uri) null),
        (GolfScorecardElementViewModel) new GolfScorecardHeaderViewModel(GolfScorecardHeaderType.Inward, 36, Length.FromYards(3630.0), "3630", new int?()),
        (GolfScorecardElementViewModel) new GolfScorecardHeaderViewModel(GolfScorecardHeaderType.Total, 71, Length.FromYards(7266.0), "7266", new int?(82))
      };
      this.YardageUnitType = AppResources.GolfScorecardYardsColumnLabel;
      this.TeesPlayed = string.Format(AppResources.GolfScorecardTeesPlayedFormatString, new object[1]
      {
        (object) "White"
      });
      this.ShowTopConnectMessage = true;
    }

    public IEnumerable<GolfScorecardElementViewModel> Elements { get; private set; }

    public string YardageUnitType { get; private set; }

    public string TeesPlayed { get; private set; }

    public bool ShowTopConnectMessage { get; private set; }

    public ICommand ConnectCommand { get; private set; }
  }
}
