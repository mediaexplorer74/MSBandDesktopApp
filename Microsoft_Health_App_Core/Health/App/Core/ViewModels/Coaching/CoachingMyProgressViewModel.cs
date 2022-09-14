// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Coaching.CoachingMyProgressViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Providers.Coaching;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels.Coaching
{
  [PageTaxonomy(new string[] {"Fitness", "Coaching Plan", "My Progress"})]
  public class CoachingMyProgressViewModel : PanelViewModelBase
  {
    private readonly ICoachingProvider coachingProvider;
    private IList<CoachingProgressSection> sections;

    public CoachingMyProgressViewModel(
      INetworkService networkService,
      ICoachingProvider coachingProvider)
      : base(networkService)
    {
      this.coachingProvider = coachingProvider;
    }

    public IList<CoachingProgressSection> Sections
    {
      get => this.sections;
      set => this.SetProperty<IList<CoachingProgressSection>>(ref this.sections, value, nameof (Sections));
    }

    protected override async Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      try
      {
        using (CancellationTokenSource source = new CancellationTokenSource(CancellationTokenUtilities.DefaultCancellationTokenTimespan))
          this.Sections = (IList<CoachingProgressSection>) new ObservableCollection<CoachingProgressSection>((IEnumerable<CoachingProgressSection>) await this.coachingProvider.GetProgressSectionsAsync(source.Token));
      }
      catch (Exception ex)
      {
        throw new CustomErrorException(AppResources.ActionPlanProgressError, ex);
      }
    }
  }
}
