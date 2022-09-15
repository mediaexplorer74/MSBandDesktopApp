// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Navigation.DeepLinkRouter
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Providers;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.App.Core.ViewModels;
using Microsoft.Health.App.Core.ViewModels.Calories;
using Microsoft.Health.App.Core.ViewModels.Goals;
using Microsoft.Health.App.Core.ViewModels.Home;
using Microsoft.Health.App.Core.ViewModels.Steps;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Navigation
{
  public class DeepLinkRouter : IDeepLinkRouter
  {
    private const string DefaultReferrer = "Manual";
    private readonly ISmoothNavService smoothNavService;
    private readonly IConfig config;
    private readonly IDateTimeService dateTimeService;
    private readonly IUserDailySummaryProvider userDailySummaryProvider;
    private readonly IApplicationService applicationService;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Navigation\\DeepLinkRouter.cs");
    private IDictionary<string, Action<IDictionary<string, string>>> predefinedRoutes;
    private IDictionary<string, Action<string, string, IDictionary<string, string>>> predefinedDetailedViewRoutes;
    private IDictionary<string, Tuple<EventType, Action<EventType, IDictionary<string, string>>>> predefinedEventRoutes;
    private IDictionary<string, Action<IDictionary<string, string>>> predefinedGoalRoute;
    private IDictionary<string, string> telemetryTargetMap = (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "summary",
        "Home"
      },
      {
        "bests",
        "Bests"
      },
      {
        "profile",
        "Profile"
      },
      {
        "steps",
        "Steps"
      },
      {
        "calories",
        "Calories"
      },
      {
        "run",
        "Run"
      },
      {
        "bike",
        "Bike"
      },
      {
        "exercise",
        "Exercise"
      },
      {
        "sleep",
        "Sleep"
      },
      {
        "goal",
        "Goal"
      }
    };
    private IDictionary<string, string> telemetryReferrerMap = (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "email",
        "Email"
      },
      {
        "app",
        "App"
      },
      {
        "web",
        "Web"
      },
      {
        "manual",
        "Manual"
      }
    };

    public DeepLinkRouter(
      ISmoothNavService smoothNavService,
      IConfig config,
      IDateTimeService dateTimeService,
      IUserDailySummaryProvider userDailySummaryProvider,
      IApplicationService applicationService)
    {
      this.predefinedRoutes = (IDictionary<string, Action<IDictionary<string, string>>>) new Dictionary<string, Action<IDictionary<string, string>>>()
      {
        {
          "summary",
          new Action<IDictionary<string, string>>(this.NavigateToHome)
        },
        {
          "bests",
          new Action<IDictionary<string, string>>(this.NavigateToBests)
        },
        {
          "profile",
          new Action<IDictionary<string, string>>(this.NavigateToProfile)
        }
      };
      this.predefinedDetailedViewRoutes = (IDictionary<string, Action<string, string, IDictionary<string, string>>>) new Dictionary<string, Action<string, string, IDictionary<string, string>>>()
      {
        {
          "steps",
          new Action<string, string, IDictionary<string, string>>(this.NavigateToSteps)
        },
        {
          "calories",
          new Action<string, string, IDictionary<string, string>>(this.NavigateToCalories)
        }
      };
      this.predefinedEventRoutes = (IDictionary<string, Tuple<EventType, Action<EventType, IDictionary<string, string>>>>) new Dictionary<string, Tuple<EventType, Action<EventType, IDictionary<string, string>>>>()
      {
        {
          "run",
          new Tuple<EventType, Action<EventType, IDictionary<string, string>>>(EventType.Running, new Action<EventType, IDictionary<string, string>>(this.NavigateToEvent))
        },
        {
          "bike",
          new Tuple<EventType, Action<EventType, IDictionary<string, string>>>(EventType.Biking, new Action<EventType, IDictionary<string, string>>(this.NavigateToEvent))
        },
        {
          "exercise",
          new Tuple<EventType, Action<EventType, IDictionary<string, string>>>(EventType.Workout, new Action<EventType, IDictionary<string, string>>(this.NavigateToEvent))
        },
        {
          "sleep",
          new Tuple<EventType, Action<EventType, IDictionary<string, string>>>(EventType.Sleeping, new Action<EventType, IDictionary<string, string>>(this.NavigateToEvent))
        }
      };
      this.predefinedGoalRoute = (IDictionary<string, Action<IDictionary<string, string>>>) new Dictionary<string, Action<IDictionary<string, string>>>()
      {
        {
          "goal",
          new Action<IDictionary<string, string>>(this.NavigateToGoal)
        }
      };
      this.smoothNavService = smoothNavService;
      this.config = config;
      this.dateTimeService = dateTimeService;
      this.userDailySummaryProvider = userDailySummaryProvider;
      this.applicationService = applicationService;
    }

    public async Task RouteAsync(Uri uri)
    {
      Assert.ParamIsNotNull((object) uri, nameof (uri));
      DeepLinkRouter.Logger.Debug((object) string.Format("Deep link router is routing the uri: {0}", new object[1]
      {
        (object) uri
      }));
      string lowerInvariant = uri.Scheme.ToLowerInvariant();
      if (!"mshealth".Equals(lowerInvariant))
      {
        string message = string.Format("uri with scheme {0} is not recognized", new object[1]
        {
          (object) lowerInvariant
        });
        DeepLinkRouter.Logger.Error((object) message);
        throw new InvalidOperationException(message);
      }
      string host = uri.Host.ToLowerInvariant();
      if (!"navigate".Equals(host, StringComparison.OrdinalIgnoreCase))
      {
        string message = string.Format("uri with host {0} is not recognized", new object[1]
        {
          (object) host
        });
        DeepLinkRouter.Logger.Error((object) message);
        throw new InvalidOperationException(message);
      }
      if (await this.applicationService.HandleAppStartUpNavigationGatesAsync())
        return;
            
      IDictionary<string, string> withKeysLowerCase = uri.ParseQueryWithKeysLowerCase();
      Dictionary<string, string> viewModelParameters = new Dictionary<string, string>();
      string valueOrDefault1 = withKeysLowerCase.GetValueOrDefault<string, string>("target", string.Empty);
      string valueOrDefault2 = withKeysLowerCase.GetValueOrDefault<string, string>("referrer", "Manual");
      string valueOrDefault3 = this.telemetryReferrerMap.GetValueOrDefault<string, string>(valueOrDefault2, "Manual");
      Action<IDictionary<string, string>> preDefinedRoute = (Action<IDictionary<string, string>>) null;
      this.predefinedRoutes.TryGetValue(valueOrDefault1, out preDefinedRoute);
      if (preDefinedRoute != null)
      {
        this.HandleRoute(valueOrDefault1, valueOrDefault3, viewModelParameters, preDefinedRoute);
      }
      else
      {
        Action<string, string, IDictionary<string, string>> preDefinedDetailedView = (Action<string, string, IDictionary<string, string>>) null;
        this.predefinedDetailedViewRoutes.TryGetValue(valueOrDefault1, out preDefinedDetailedView);
        if (preDefinedDetailedView != null)
        {
          this.HandleStepsAndCalories(valueOrDefault1, withKeysLowerCase, valueOrDefault3, viewModelParameters, preDefinedDetailedView);
        }
        else
        {
          Tuple<EventType, Action<EventType, IDictionary<string, string>>> preEventDefined = (Tuple<EventType, Action<EventType, IDictionary<string, string>>>) null;
          this.predefinedEventRoutes.TryGetValue(valueOrDefault1, out preEventDefined);
          if (preEventDefined != null)
          {
            this.HandleEvents(valueOrDefault1, withKeysLowerCase, valueOrDefault3, viewModelParameters, preEventDefined);
          }
          else
          {
            Action<IDictionary<string, string>> goalRoute = (Action<IDictionary<string, string>>) null;
            this.predefinedGoalRoute.TryGetValue(valueOrDefault1, out goalRoute);
            if (goalRoute != null)
            {
              this.HandleGoal(valueOrDefault1, withKeysLowerCase, valueOrDefault3, (IDictionary<string, string>) viewModelParameters, goalRoute);
            }
            else
            {
              DeepLinkRouter.Logger.Debug((object) string.Format("{0} is not recognized so navigating to home", new object[1]
              {
                (object) uri
              }));
              ApplicationTelemetry.LogDeepLinkError(valueOrDefault2, valueOrDefault1, withKeysLowerCase.GetValueOrDefault<string, string>("pivot", string.Empty), host);
              this.NavigateToHome((IDictionary<string, string>) null);
            }
          }
        }
      }
    }

    private void HandleRoute(
      string path,
      string referrer,
      Dictionary<string, string> viewModelParameters,
      Action<IDictionary<string, string>> preDefinedRoute)
    {
      DeepLinkRouter.Logger.Debug((object) string.Format("Path: {0} is predefined route", new object[1]
      {
        (object) path
      }));
      ApplicationTelemetry.LogDeepLinkSuccess(referrer, this.telemetryTargetMap[path], "NA");
      preDefinedRoute((IDictionary<string, string>) viewModelParameters);
    }

    private void HandleEvents(
      string path,
      IDictionary<string, string> queryParams,
      string referrer,
      Dictionary<string, string> viewModelParameters,
      Tuple<EventType, Action<EventType, IDictionary<string, string>>> preEventDefined)
    {
      DeepLinkRouter.Logger.Debug((object) string.Format("Path: {0} is predefined event route", new object[1]
      {
        (object) path
      }));
      ApplicationTelemetry.LogDeepLinkSuccess(referrer, this.telemetryTargetMap[path], "NA");
      this.PopulateViewModelParams(new string[1]{ "ID" }, queryParams, (IDictionary<string, string>) viewModelParameters);
      preEventDefined.Item2(preEventDefined.Item1, (IDictionary<string, string>) viewModelParameters);
    }

    private void HandleStepsAndCalories(
      string path,
      IDictionary<string, string> queryParams,
      string referrer,
      Dictionary<string, string> viewModelParameters,
      Action<string, string, IDictionary<string, string>> preDefinedDetailedView)
    {
      DeepLinkRouter.Logger.Debug((object) string.Format("Path: {0} is predefined detail view route", new object[1]
      {
        (object) path
      }));
      string valueOrDefault = queryParams.GetValueOrDefault<string, string>("pivot", "week");
      string str = (string) null;
      queryParams.TryGetValue("date", out str);
      string pivot = "Week";
      if ("Day".Equals(valueOrDefault, StringComparison.OrdinalIgnoreCase))
        pivot = "Day";
      if ("Last7Days".Equals(valueOrDefault, StringComparison.OrdinalIgnoreCase))
        pivot = "Last7Days";
      ApplicationTelemetry.LogDeepLinkSuccess(referrer, this.telemetryTargetMap[path], pivot);
      preDefinedDetailedView(valueOrDefault, str, (IDictionary<string, string>) viewModelParameters);
    }

    private void HandleGoal(
      string path,
      IDictionary<string, string> queryParams,
      string referrer,
      IDictionary<string, string> viewModelParameters,
      Action<IDictionary<string, string>> goalRoute)
    {
      DeepLinkRouter.Logger.Debug((object) string.Format("Path: {0} is predefined goal route", new object[1]
      {
        (object) path
      }));
      ApplicationTelemetry.LogDeepLinkSuccess(referrer, this.telemetryTargetMap[path], "Steps");
      this.PopulateViewModelParams(new string[1]{ "Value" }, queryParams, viewModelParameters);
      string empty = string.Empty;
      queryParams.TryGetValue("type", out empty);
      GoalType goalType = GoalType.StepGoal;
      if ("calories".Equals(empty, StringComparison.OrdinalIgnoreCase))
        goalType = GoalType.CalorieGoal;
      viewModelParameters.Add("type", ((int) goalType).ToString());
      goalRoute(viewModelParameters);
    }

    private void NavigateToHome(IDictionary<string, string> viewModelParams) => this.smoothNavService.Navigate<TilesViewModel>(viewModelParams);

    private void NavigateToBests(IDictionary<string, string> viewModelParams)
    {
      viewModelParams.Add("Filter", EventType.Best.ToString());
      this.smoothNavService.Navigate<HistoryViewModel>(viewModelParams);
    }

    private void NavigateToSteps(
      string pivotValue,
      string queryDate,
      IDictionary<string, string> viewModelParams)
    {
      DeepLinkRouter.Logger.Debug((object) "Routing the deep link to steps detail page");
      if (pivotValue.Equals("day", StringComparison.OrdinalIgnoreCase))
      {
        this.NavigateToDetailsViewModel(typeof (StepsDayViewModel), typeof (StepsDayHeaderViewModel), queryDate, viewModelParams);
      }
      else
      {
        queryDate = this.GetStartWeekDate(queryDate);
        this.NavigateToDetailsViewModel(typeof (StepsWeekViewModel), typeof (StepsWeekHeaderViewModel), queryDate, viewModelParams);
      }
    }

    private void NavigateToCalories(
      string pivotValue,
      string queryDate,
      IDictionary<string, string> viewModelParams)
    {
      DeepLinkRouter.Logger.Debug((object) "Routing the deep link to calories detail page");
      if (pivotValue.Equals("day", StringComparison.OrdinalIgnoreCase))
      {
        this.NavigateToDetailsViewModel(typeof (CaloriesDayViewModel), typeof (CaloriesDayHeaderViewModel), queryDate, viewModelParams);
      }
      else
      {
        queryDate = this.GetStartWeekDate(queryDate);
        this.NavigateToDetailsViewModel(typeof (CaloriesWeekViewModel), typeof (CaloriesWeekHeaderViewModel), queryDate, viewModelParams);
      }
    }

    private void NavigateToEvent(EventType eventType, IDictionary<string, string> viewModelParams)
    {
      this.EnsureNavigationHistoryOnLaunch();
      viewModelParams.Add("Type", eventType.ToString());
      viewModelParams.Add("History", bool.FalseString);
      this.smoothNavService.Navigate(typeof (PivotDetailsViewModel), viewModelParams);
    }

    private void NavigateToProfile(IDictionary<string, string> viewModelParams) => this.smoothNavService.Navigate(typeof (SettingsProfileViewModel), viewModelParams);

    private void NavigateToDetailsViewModel(
      Type contentViewModelType,
      Type headerViewModelType,
      string queryDate,
      IDictionary<string, string> viewModelParams)
    {
      this.EnsureNavigationHistoryOnLaunch();
      DateTimeOffset result;
      if (DateTimeOffset.TryParse(queryDate, out result))
        viewModelParams.Add("StartDate", Formatter.FormatStatDateParameter(result));
      viewModelParams.Add("TargetViewModelType", contentViewModelType.FullName);
      viewModelParams.Add("HeaderViewModelType", headerViewModelType.FullName);
      this.smoothNavService.Navigate<DetailsViewModel>(viewModelParams);
    }

    private void NavigateToGoal(IDictionary<string, string> viewModelParams) => this.smoothNavService.Navigate(typeof (EditGoalsViewModel), viewModelParams);

    public void PopulateViewModelParams(
      string[] paramNames,
      IDictionary<string, string> queryParams,
      IDictionary<string, string> viewModelParameters)
    {
      Assert.ParamIsNotNull((object) paramNames, nameof (paramNames));
      Assert.ParamIsNotNull((object) queryParams, nameof (queryParams));
      Assert.ParamIsNotNull((object) viewModelParameters, nameof (viewModelParameters));
      foreach (string paramName in paramNames)
      {
        string str = (string) null;
        string lowerInvariant = paramName.ToLowerInvariant();
        if (queryParams.TryGetValue(lowerInvariant, out str) && !viewModelParameters.ContainsKey(paramName))
          viewModelParameters.Add(paramName, str);
      }
    }

    private void EnsureNavigationHistoryOnLaunch()
    {
      if (this.smoothNavService.CurrentJournalEntry != null)
        return;
      this.smoothNavService.Reset(new JournalEntry()
      {
        ViewModelType = typeof (TilesViewModel),
        IsPageCached = false
      });
    }

    private string GetStartWeekDate(string queryDate)
    {
      DateTimeOffset result;
      if (DateTimeOffset.TryParse(queryDate, out result))
        queryDate = Formatter.FormatStatDateParameter(this.dateTimeService.AddWeeks(result.AddDays(1.0), -1));
      return queryDate;
    }
  }
}
