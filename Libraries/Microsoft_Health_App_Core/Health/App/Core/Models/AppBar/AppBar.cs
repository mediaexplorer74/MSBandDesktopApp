// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.AppBar.AppBar
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Models.AppBar
{
  public class AppBar
  {
    public AppBar(params AppBarButton[] buttons) => this.Buttons = (IReadOnlyList<AppBarButton>) ((IEnumerable<AppBarButton>) buttons).ToList<AppBarButton>();

    public AppBar(IEnumerable<AppBarButton> buttons) => this.Buttons = (IReadOnlyList<AppBarButton>) buttons.ToList<AppBarButton>();

    public IReadOnlyList<AppBarButton> Buttons { get; }
  }
}
