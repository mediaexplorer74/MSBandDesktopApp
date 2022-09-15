// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Oobe.OobePageViewModelBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services;

namespace Microsoft.Health.App.Core.ViewModels.Oobe
{
  public abstract class OobePageViewModelBase : PageViewModelBase, IOobeViewModel
  {
    public OobePageViewModelBase(INetworkService networkService)
      : base(networkService)
    {
    }

    public virtual ActionViewModel PositiveAction { get; }

    public virtual ActionViewModel NegativeAction { get; }

    public virtual string PageTitle { get; }

    public virtual string PageSubtitle { get; }
  }
}
