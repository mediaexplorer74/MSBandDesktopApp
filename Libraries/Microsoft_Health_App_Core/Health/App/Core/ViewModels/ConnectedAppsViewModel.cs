// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.ConnectedAppsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageMetadata(PageContainerType.FullScreen)]
  public class ConnectedAppsViewModel : WebHostViewModel
  {
    private IUserProfileService userProfileService;

    public ConnectedAppsViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IMessageBoxService messageBoxService,
      IUserProfileService userProfileService)
      : base(networkService, smoothNavService, messageBoxService)
    {
      this.userProfileService = userProfileService;
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      this.PageUrl = new Uri(this.userProfileService.ThirdPartyPartnersPortalEndpoint);
      return (Task) Task.FromResult<object>((object) null);
    }
  }
}
