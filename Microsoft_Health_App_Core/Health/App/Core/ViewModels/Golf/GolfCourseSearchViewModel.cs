// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.GolfCourseSearchViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  [PageTaxonomy(new string[] {"Fitness", "Golf", "Search"})]
  public class GolfCourseSearchViewModel : SearchViewModelBase<GolfCourseSearchResultsViewModel>
  {
    public GolfCourseSearchViewModel(
      ISmoothNavService smoothNavService,
      INetworkService networkService)
      : base(smoothNavService, networkService)
    {
    }

    protected override IDictionary<string, string> GetSearchArguments()
    {
      IDictionary<string, string> searchArguments = base.GetSearchArguments();
      searchArguments.Add("RequiresFilterInitialization", bool.TrueString);
      return searchArguments;
    }
  }
}
