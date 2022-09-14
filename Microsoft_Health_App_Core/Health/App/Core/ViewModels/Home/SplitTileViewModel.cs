// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Home.SplitTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.ViewModels.Home
{
  public abstract class SplitTileViewModel : TileViewModel
  {
    private string header;

    protected SplitTileViewModel()
      : base(TileViewModel.TileType.Split)
    {
    }

    public string Header
    {
      get => this.header;
      protected set => this.SetProperty<string>(ref this.header, value, nameof (Header));
    }

    public override bool WillOpenOnTileCommand() => false;
  }
}
