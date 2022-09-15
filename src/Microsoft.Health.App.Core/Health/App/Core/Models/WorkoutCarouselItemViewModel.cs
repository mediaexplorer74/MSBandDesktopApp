// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.WorkoutCarouselItemViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;

namespace Microsoft.Health.App.Core.Models
{
  public class WorkoutCarouselItemViewModel : HealthViewModelBase
  {
    private string title;
    private string subTitle;
    private EmbeddedOrRemoteImageSource image;
    private string id;
    private bool isSelected;
    private bool isSelectable;

    public string Id
    {
      get => this.id;
      set => this.SetProperty<string>(ref this.id, value, nameof (Id));
    }

    public string Title
    {
      get => this.title;
      set => this.SetProperty<string>(ref this.title, value, nameof (Title));
    }

    public string SubTitle
    {
      get => this.subTitle;
      set => this.SetProperty<string>(ref this.subTitle, value, nameof (SubTitle));
    }

    public EmbeddedOrRemoteImageSource Image
    {
      get => this.image;
      set => this.SetProperty<EmbeddedOrRemoteImageSource>(ref this.image, value, nameof (Image));
    }

    public bool IsSelected
    {
      get => this.isSelected;
      set => this.SetProperty<bool>(ref this.isSelected, value, nameof (IsSelected));
    }

    public bool IsSelectable
    {
      get => this.isSelectable;
      set => this.SetProperty<bool>(ref this.isSelectable, value, nameof (IsSelectable));
    }
  }
}
