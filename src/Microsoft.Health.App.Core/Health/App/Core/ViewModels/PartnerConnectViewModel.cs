// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.PartnerConnectViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Navigation;
using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.Cloud.Client.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class PartnerConnectViewModel : WebHostViewModel
  {
    public const string PartnerNameKey = "PartnerName";
    private const string BindQueryParameterName = "bind";
    private const string BindQueryParameterSuccessValue = "success";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\ViewModels\\PartnerConnectViewModel.cs");
    private readonly IUserProfileService userProfileService;
    private readonly ISmoothNavService smoothNavService;

    public PartnerConnectViewModel(
      INetworkService networkService,
      ISmoothNavService smoothNavService,
      IMessageBoxService messageBoxService,
      IUserProfileService userProfileService)
      : base(networkService, smoothNavService, messageBoxService)
    {
      this.smoothNavService = smoothNavService;
      this.userProfileService = userProfileService;
    }

    protected override void OnNavigationCompleted(
      WebViewNavigationCompletedEventArgsWrapper eventArgs)
    {
      if (eventArgs.IsSuccess)
      {
        IDictionary<string, string> query = eventArgs.Uri.ParseQuery();
        if (query.ContainsKey("bind") && query["bind"] == "success")
        {
          PartnerConnectViewModel.Logger.Info((object) "Partner connect succeeded.");
          this.smoothNavService.GoBack();
        }
      }
      base.OnNavigationCompleted(eventArgs);
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      UriBuilder uriBuilder = new UriBuilder(this.userProfileService.ThirdPartyPartnersPortalEndpoint);
      string str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "partner={0}", new object[1]
      {
        (object) this.GetStringParameter("PartnerName")
      });
      uriBuilder.Query = uriBuilder == null || uriBuilder.Query.Length <= 1 ? str : uriBuilder.Query.Substring(1) + "&" + str;
      this.PageUrl = uriBuilder.Uri;
      return (Task) Task.FromResult<object>((object) null);
    }
  }
}
