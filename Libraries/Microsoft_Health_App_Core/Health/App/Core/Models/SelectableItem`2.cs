// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.SelectableItem`2
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;

namespace Microsoft.Health.App.Core.Models
{
  public class SelectableItem<T, TU> : HealthObservableObject
  {
    private bool isSelected;

    public SelectableItem(T item, TU displayInfo, bool isSelected = false)
    {
      this.DisplayInfo = displayInfo;
      this.Item = item;
      this.IsSelected = isSelected;
    }

    public bool IsSelected
    {
      get => this.isSelected;
      set => this.SetProperty<bool>(ref this.isSelected, value, nameof (IsSelected));
    }

    public TU DisplayInfo { get; private set; }

    public T Item { get; private set; }
  }
}
