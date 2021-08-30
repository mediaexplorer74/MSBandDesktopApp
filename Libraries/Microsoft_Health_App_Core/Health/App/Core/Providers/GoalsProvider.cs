// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Providers.GoalsProvider
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.Cloud.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Providers
{
  public class GoalsProvider : IGoalsProvider
  {
    public const int UnderGoal = 95;
    public const int NearGoal = 100;
    public const int ReachedGoal = 102;
    private readonly INetworkService networkService;
    private readonly IHealthCloudClient healthCloudClient;

    public GoalsProvider(IHealthCloudClient healthCloudClient, INetworkService networkService)
    {
      this.healthCloudClient = healthCloudClient;
      this.networkService = networkService;
    }

    public async Task<UsersGoal> GetGoalAsync(GoalType type)
    {
      IList<UsersGoal> usersGoalsAsync = await this.healthCloudClient.GetUsersGoalsAsync(type, CancellationToken.None);
      if ((usersGoalsAsync == null || usersGoalsAsync.Count == 0) && !this.networkService.IsInternetAvailable)
        throw new InternetException("Internet is not available.");
      return usersGoalsAsync == null ? (UsersGoal) null : usersGoalsAsync.FirstOrDefault<UsersGoal>();
    }

    public async Task<UsersGoal> GetGoalExpandedAsync(GoalType type)
    {
      IList<UsersGoal> usersGoalsAsync = await this.healthCloudClient.GetUsersGoalsAsync(type, CancellationToken.None, shouldExpand: true);
      return usersGoalsAsync == null ? (UsersGoal) null : usersGoalsAsync.FirstOrDefault<UsersGoal>();
    }

    public async Task SetGoalAsync(GoalType type, object value)
    {
      IList<UsersGoal> goals = await this.healthCloudClient.GetUsersGoalsAsync(type, CancellationToken.None);
      if (goals == null || goals.Count == 0)
      {
        GoalTemplate goalTemplate = (await this.healthCloudClient.GetGoalTemplatesAsync(CancellationToken.None)).Where<GoalTemplate>((Func<GoalTemplate, bool>) (t => t.Type == type)).SingleOrDefault<GoalTemplate>();
        UsersGoalCreation usersGoalCreation = new UsersGoalCreation();
        usersGoalCreation.Name = goalTemplate.Name;
        usersGoalCreation.TemplateId = goalTemplate.Id;
        usersGoalCreation.Description = goalTemplate.Description;
        GoalValueTemplate goalValueTemplate1 = goalTemplate.GoalValueTemplates.FirstOrDefault<GoalValueTemplate>();
        GoalValueTemplate goalValueTemplate2 = new GoalValueTemplate();
        if (goalValueTemplate1 == null)
          return;
        goalValueTemplate2.Name = goalValueTemplate1.Name;
        goalValueTemplate2.Description = goalValueTemplate1.Description;
        goalValueTemplate2.Threshold = value;
        usersGoalCreation.ValueHistory = (IList<GoalValueHistory>) new List<GoalValueHistory>()
        {
          new GoalValueHistory()
          {
            ValueTemplate = goalValueTemplate2
          }
        };
        await this.healthCloudClient.AddUsersGoalsAsync((ICollection<UsersGoalCreation>) new List<UsersGoalCreation>()
        {
          usersGoalCreation
        }, CancellationToken.None);
      }
      else
      {
        UsersGoal usersGoal = goals[0];
        UsersGoalUpdate usersGoalUpdate = new UsersGoalUpdate();
        usersGoalUpdate.Id = usersGoal.Id;
        GoalValueTemplate valueTemplate = usersGoal.ValueSummary.First<GoalValueSummary>().ValueTemplate;
        if (valueTemplate == null)
          return;
        usersGoalUpdate.ValueHistory = (IList<GoalValueHistory>) new List<GoalValueHistory>()
        {
          new GoalValueHistory()
          {
            ValueTemplate = new GoalValueTemplate()
            {
              Name = valueTemplate.Name,
              Description = valueTemplate.Description,
              Threshold = value
            }
          }
        };
        await this.healthCloudClient.UpdateUsersGoalAsync((ICollection<UsersGoalUpdate>) new List<UsersGoalUpdate>()
        {
          usersGoalUpdate
        }, CancellationToken.None);
      }
    }
  }
}
