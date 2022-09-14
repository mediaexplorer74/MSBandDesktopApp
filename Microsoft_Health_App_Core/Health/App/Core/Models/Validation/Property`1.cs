// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.Validation.Property`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Microsoft.Health.App.Core.Models.Validation
{
  public class Property<T> : HealthObservableObject, IProperty, INotifyPropertyChanged
  {
    private T original;
    private T value;
    private bool isEnabled;
    private bool valueHasBeenSet;
    private ObservableCollection<string> errors = new ObservableCollection<string>();

    public Property() => this.errors.CollectionChanged += (NotifyCollectionChangedEventHandler) ((s, e) => this.RaisePropertyChanged(nameof (IsValid)));

    public IList<string> Errors => (IList<string>) this.errors;

    public bool IsDirty
    {
      get
      {
        if ((object) this.Value == null && (object) this.Original == null)
          return false;
        return (object) this.Value == null && (object) this.Original != null || !this.Value.Equals((object) this.Original);
      }
    }

    public bool IsValid => !this.Errors.Any<string>();

    public bool IsEnabled
    {
      get => this.isEnabled;
      set => this.SetProperty<bool>(ref this.isEnabled, value, nameof (IsEnabled));
    }

    public T Original
    {
      get => this.original;
      set
      {
        this.valueHasBeenSet = true;
        this.SetProperty<T>(ref this.original, value, nameof (Original));
      }
    }

    public T Value
    {
      get => this.value;
      set
      {
        if (!this.valueHasBeenSet)
          this.Original = value;
        this.SetProperty<T>(ref this.value, value, nameof (Value));
        this.RaisePropertyChanged("IsDirty");
      }
    }

    public void Revert() => this.Value = this.Original;

    public void MarkSaved() => this.Original = this.Value;

    public override string ToString() => (object) this.Value == null ? string.Empty : this.Value.ToString();
  }
}
