// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Golf.TeePickViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Providers.Golf.Courses;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels.Golf
{
  [PageTaxonomy(new string[] {"Fitness", "Golf", "Course", "TeePick"})]
  public class TeePickViewModel : PageViewModelBase
  {
    internal const string CourseIdKey = "CourseId";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\Golf\\TeePickViewModel.cs");
    private static readonly IActivityManager ActivityManager = TeePickViewModel.Logger.CreateActivityManager();
    private readonly ISmoothNavService smoothNavService;
    private readonly IGolfCourseProvider golfCourseProvider;
    private HealthCommand backCommand;
    private HealthCommand<TeeChoice> chooseTeeCommand;
    private GolfCourseDetails courseDetails;
    private IList<TeeChoice> teeChoices;

    public TeePickViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IGolfCourseProvider golfCourseProvider)
      : base(networkService)
    {
      this.smoothNavService = smoothNavService;
      this.golfCourseProvider = golfCourseProvider;
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null) => TeePickViewModel.ActivityManager.RunAsActivityAsync(Level.Debug, (Func<string>) (() => "Loading tee pick page"), (Func<Task>) (async () =>
    {
      string stringParameter = this.GetStringParameter("CourseId");
      TeePickViewModel teePickViewModel = this;
      GolfCourseDetails courseDetails = teePickViewModel.courseDetails;
      GolfCourseDetails courseDetailsAsync = await this.golfCourseProvider.GetCourseDetailsAsync(stringParameter, CancellationToken.None);
      teePickViewModel.courseDetails = courseDetailsAsync;
      teePickViewModel = (TeePickViewModel) null;
      List<TeeChoice> teeChoiceList = new List<TeeChoice>();
      foreach (GolfCourseTee tee in (IEnumerable<GolfCourseTee>) this.courseDetails.Tees)
        teeChoiceList.Add(new TeeChoice()
        {
          Id = tee.Id,
          Name = tee.Name
        });
      teeChoiceList.Add(new TeeChoice()
      {
        Id = (string) null,
        Name = AppResources.TeePickOptionDontKnow
      });
      this.TeeChoices = (IList<TeeChoice>) teeChoiceList;
    }));

    public ICommand BackCommand => (ICommand) this.backCommand ?? (ICommand) (this.backCommand = new HealthCommand((Action) (() => this.smoothNavService.GoBack())));

    public ICommand ChooseTeeCommand => (ICommand) this.chooseTeeCommand ?? (ICommand) (this.chooseTeeCommand = new HealthCommand<TeeChoice>((Action<TeeChoice>) (teeChoice =>
    {
      string id;
      if (teeChoice.Id != null)
      {
        id = teeChoice.Id;
      }
      else
      {
        if (this.courseDetails.Tees != null)
        {
          if (this.courseDetails.Tees.Count != 0)
          {
            try
            {
              id = this.courseDetails.Tees.Single<GolfCourseTee>((Func<GolfCourseTee, bool>) (t => t.IsDefault)).Id;
              goto label_7;
            }
            catch (Exception ex)
            {
              TeePickViewModel.Logger.Warn((object) ("Error finding a valid default tee for course " + this.courseDetails.Name), ex);
              id = this.courseDetails.Tees.First<GolfCourseTee>().Id;
              goto label_7;
            }
          }
        }
        throw new InvalidOperationException("No tees found.");
      }
label_7:
      this.smoothNavService.Navigate(typeof (GolfSyncViewModel), (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "CourseId",
          this.Parameters["CourseId"]
        },
        {
          "TeeId",
          id
        }
      }, NavigationStackAction.RemovePrevious);
    })));

    public IList<TeeChoice> TeeChoices
    {
      get => this.teeChoices;
      private set => this.SetProperty<IList<TeeChoice>>(ref this.teeChoices, value, nameof (TeeChoices));
    }
  }
}
