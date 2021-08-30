// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.WhatsNew.DesignWhatsNewViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Resources;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.ViewModels.WhatsNew
{
  public class DesignWhatsNewViewModel
  {
    public IList<WhatsNewPivotItemViewModel> Pivots => (IList<WhatsNewPivotItemViewModel>) new List<WhatsNewPivotItemViewModel>()
    {
      new WhatsNewPivotItemViewModel("Hike", AppResources.WhatsNewPivotHeader1, AppResources.WhatsNewPivotSubheader1, EmbeddedAsset.Hike, WhatsNewPivotColor.Primary, AppResources.WhatsNewPivotButtonLabel1),
      new WhatsNewPivotItemViewModel("View of you", AppResources.WhatsNewPivotHeader2, AppResources.WhatsNewPivotSubheader2, EmbeddedAsset.Email, WhatsNewPivotColor.Primary, AppResources.WhatsNewPivotButtonLabel2),
      new WhatsNewPivotItemViewModel("UWP", AppResources.WhatsNewPivotHeader3, AppResources.WhatsNewPivotSubheader3, EmbeddedAsset.UWP, WhatsNewPivotColor.Primary, AppResources.WhatsNewPivotButtonLabel3)
    };

    public int SelectedPivotIndex => 1;

    public WhatsNewPivotColor SelectedPivotColorTheme => this.Pivots[this.SelectedPivotIndex].ColorTheme;
  }
}
