// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.VideoViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels
{
  [PageTaxonomy(new string[] {"Fitness", "Video"})]
  public class VideoViewModel : PageViewModelBase
  {
    private Uri videoUrl;

    public VideoViewModel(INetworkService networkService)
      : base(networkService)
    {
    }

    public Uri VideoUrl
    {
      get => this.videoUrl;
      set => this.SetProperty<Uri>(ref this.videoUrl, value, nameof (VideoUrl));
    }

    protected override Task LoadDataAsync(IDictionary<string, string> parameters = null)
    {
      if (parameters == null)
        throw new ArgumentNullException(nameof (parameters));
      this.VideoUrl = parameters.ContainsKey("Url") ? new Uri(parameters["Url"], UriKind.Absolute) : throw new ArgumentException("Must specify video URL.", nameof (parameters));
      return (Task) Task.FromResult<object>((object) null);
    }
  }
}
