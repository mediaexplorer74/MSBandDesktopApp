// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.DesignWebTileViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Tiles;
using System.Collections.Generic;
using System.Windows.Input;

namespace Microsoft.Health.App.Core.ViewModels
{
  public class DesignWebTileViewModel
  {
    public string Status => string.Empty;

    public bool IsUpdating => false;

    public WebTileIconViewModel WebTileIcon => new WebTileIconViewModel("Test Web Tile", (BandIcon) null, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", "Tester", "Microsoft", "1.23.4567.89", (IEnumerable<string>) new string[3]
    {
      "http://www.microsoft.com",
      "http://www.microsoft.com/mobile",
      "http://www.microsoft.com/microsoft-band"
    });

    public ICommand CancelCommand => (ICommand) null;

    public ICommand InstallWebTileCommand => (ICommand) null;
  }
}
