// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.AppBar.AppBarButton
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Windows.Input;

namespace Microsoft.Health.App.Core.Models.AppBar
{
  public class AppBarButton
  {
    public AppBarButton(string text, AppBarIcon icon, ICommand command)
    {
      this.Text = text;
      this.Icon = icon;
      this.Command = command;
    }

    public AppBarIcon Icon { get; set; }

    public string Text { get; set; }

    public ICommand Command { get; set; }
  }
}
