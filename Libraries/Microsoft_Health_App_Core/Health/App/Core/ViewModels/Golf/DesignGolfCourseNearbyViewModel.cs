// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.DesignGolfCourseNearbyViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Providers.Golf.Courses;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  public class DesignGolfCourseNearbyViewModel
  {
    public string SearchTitle => AppResources.GolfCourseNearbyPageHeader;

    public string ResultsTitle => string.Format(AppResources.GolfCourseSearchResultsLabel, new object[1]
    {
      (object) this.GolfCourses.Count
    });

    public bool IsFilteringEnabled => true;

    public bool IsFiltered => false;

    public bool IsLoading => true;

    public LoadState LoadState => LoadState.Loaded;

    public IList<GolfCourseSummaryViewModel> GolfCourses
    {
      get
      {
        ObservableCollection<GolfCourseSummaryViewModel> observableCollection = new ObservableCollection<GolfCourseSummaryViewModel>();
        observableCollection.Add(new GolfCourseSummaryViewModel("Bellevue Golf Course", (string) Formatter.FormatDistanceDynamic(new Length?(Length.FromMiles(0.25)), DistanceUnitType.Imperial, true), GolfCourseType.Private, 18, "Bellevue", (GolfCourseSummary) null));
        observableCollection.Add(new GolfCourseSummaryViewModel("Redmond Golf Course", (string) Formatter.FormatDistanceDynamic(new Length?(Length.FromMiles(2.0)), DistanceUnitType.Imperial, true), GolfCourseType.Public, 9, "Redmond", (GolfCourseSummary) null));
        observableCollection.Add(new GolfCourseSummaryViewModel("Seattle Golf Course", (string) Formatter.FormatDistanceDynamic(new Length?(Length.FromMiles(4.5)), DistanceUnitType.Imperial, true), GolfCourseType.Private, 18, "Seattle", (GolfCourseSummary) null));
        observableCollection.Add(new GolfCourseSummaryViewModel("Attle Golf Course", (string) Formatter.FormatDistanceDynamic(new Length?(Length.FromMiles(4.5)), DistanceUnitType.Imperial, true), GolfCourseType.Private, 18, "Seattle", (GolfCourseSummary) null));
        observableCollection.Add(new GolfCourseSummaryViewModel("Beattle Golf Course", (string) Formatter.FormatDistanceDynamic(new Length?(Length.FromMiles(4.5)), DistanceUnitType.Imperial, true), GolfCourseType.Private, 18, "Seattle", (GolfCourseSummary) null));
        observableCollection.Add(new GolfCourseSummaryViewModel("Kirkland Golf Course", (string) Formatter.FormatDistanceDynamic(new Length?(Length.FromMiles(3.75)), DistanceUnitType.Imperial, true), GolfCourseType.Public, 9, "Kirkland", (GolfCourseSummary) null));
        return (IList<GolfCourseSummaryViewModel>) observableCollection;
      }
    }

    public IList<SemanticZoomGrouping<object>> GolfCoursesZoom
    {
      get
      {
        ObservableCollection<SemanticZoomGrouping<object>> source = new ObservableCollection<SemanticZoomGrouping<object>>();
        IEnumerable<IGrouping<char, GolfCourseSummaryViewModel>> groupings = this.GolfCourses.GroupBy<GolfCourseSummaryViewModel, char>((Func<GolfCourseSummaryViewModel, char>) (p => p.Name.ToLower()[0]));
        List<char> charList = new List<char>();
        foreach (char ch in ((IEnumerable<char>) "abcdefghijklmnopqrstuvwxyz".ToCharArray()).ToList<char>())
        {
          source.Add(new SemanticZoomGrouping<object>()
          {
            Key = ch
          });
          charList.Add(ch);
        }
        foreach (IGrouping<char, GolfCourseSummaryViewModel> grouping in groupings)
        {
          IGrouping<char, GolfCourseSummaryViewModel> g = grouping;
          SemanticZoomGrouping<object> semanticZoomGrouping = source.First<SemanticZoomGrouping<object>>((Func<SemanticZoomGrouping<object>, bool>) (i => i.Key.Equals(g.Key)));
          if (semanticZoomGrouping != null)
          {
            foreach (GolfCourseSummaryViewModel summaryViewModel in (IEnumerable<GolfCourseSummaryViewModel>) g)
              semanticZoomGrouping.Add((object) summaryViewModel);
          }
        }
        return (IList<SemanticZoomGrouping<object>>) source;
      }
    }

    public ICommand BackCommand => (ICommand) null;

    public ICommand OpenGolfCourseCommand => (ICommand) null;

    public ICommand LoadMoreCommand => (ICommand) null;
  }
}
