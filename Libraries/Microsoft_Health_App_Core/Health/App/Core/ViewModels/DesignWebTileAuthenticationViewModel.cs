// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.DesignWebTileAuthenticationViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Tiles;
using Microsoft.Health.App.Core.Resources;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class DesignWebTileAuthenticationViewModel
  {
    public string Status => string.Empty;

    public bool IsUpdating => false;

    public bool ShowError => true;

    public string Username => "tester";

    public string Password => "123abc";

    public string ResourceIndexProgress => string.Format(AppResources.WebTileResourceIndexProgressFormat, new object[2]
    {
      (object) 2,
      (object) 3
    });

    public string ResourceSource => "http://www.microsoft.com/mobile";

    public WebTileIconViewModel WebTileIcon => new WebTileIconViewModel("Test Web Tile", (BandIcon) null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");

    public ICommand CancelCommand => (ICommand) null;

    public ICommand InstallWebTileCommand => (ICommand) null;
  }
}
