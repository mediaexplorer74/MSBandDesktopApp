// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.PivotDefinition
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;

namespace Microsoft.Health.App.Core.Models
{
  public class PivotDefinition : HealthObservableObject
  {
    private string header;
    private bool isSelected;

    public PivotDefinition(string header, object content)
    {
      this.Header = header;
      this.Content = content;
    }

    public string Header
    {
      get => this.header;
      set => this.SetProperty<string>(ref this.header, value, nameof (Header));
    }

    public object Content { get; private set; }

    public bool IsSelected
    {
      get => this.isSelected;
      set => this.SetProperty<bool>(ref this.isSelected, value, nameof (IsSelected));
    }
  }
}
