// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.Validation.ModelBase`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Microsoft.Health.App.Core.Models.Validation
{
  public abstract class ModelBase<T> : HealthObservableObject, IModel where T : IModel
  {
    private readonly ObservableCollection<string> errors = new ObservableCollection<string>();
    private Action<IModel> validator;
    private IEnumerable<PropertyInfo> properties;
    private bool isValid = true;
    private bool isDirty;

    public ModelBase()
    {
      foreach (INotifyPropertyChanged notifyPropertyChanged in this.Properties().Select<PropertyInfo, IProperty>((Func<PropertyInfo, IProperty>) (x => x.GetValue((object) this) as IProperty)))
        notifyPropertyChanged.PropertyChanged += (PropertyChangedEventHandler) ((s, e) =>
        {
          if (!e.PropertyName.Equals("Value"))
            return;
          this.Validate();
        });
    }

    private event ModelBase<T>.ValidationFinishedHandler ValidationFinished;

    public IList<string> Errors => (IList<string>) this.errors;

    public bool IsValid
    {
      get => this.isValid;
      set => this.SetProperty<bool>(ref this.isValid, value, nameof (IsValid));
    }

    public bool IsDirty
    {
      get => this.isDirty;
      set => this.SetProperty<bool>(ref this.isDirty, value, nameof (IsDirty));
    }

    public Action<IModel> Validator
    {
      get => this.validator;
      set => this.SetProperty<Action<IModel>>(ref this.validator, value, nameof (Validator));
    }

    public void Revert()
    {
      foreach (IProperty property in this.Properties().Select<PropertyInfo, IProperty>((Func<PropertyInfo, IProperty>) (x => x.GetValue((object) this) as IProperty)))
        property.Revert();
    }

    public void MarkSaved()
    {
      foreach (IProperty property in this.Properties().Select<PropertyInfo, IProperty>((Func<PropertyInfo, IProperty>) (x => x.GetValue((object) this) as IProperty)))
        property.MarkSaved();
      this.Validate();
    }

    public bool Validate()
    {
      this.ValidationFinished += new ModelBase<T>.ValidationFinishedHandler(this.OnValidationFinished);
      IEnumerable<IProperty> source = this.Properties().Select<PropertyInfo, IProperty>((Func<PropertyInfo, IProperty>) (x => x.GetValue((object) this) as IProperty));
      foreach (IProperty property in source)
        property.Errors.Clear();
      this.Errors.Clear();
      if (this.Validator != null)
        this.Validator((IModel) this);
      foreach (string str in source.SelectMany<IProperty, string>((Func<IProperty, IEnumerable<string>>) (x => (IEnumerable<string>) x.Errors)))
        this.Errors.Add(str);
      this.IsDirty = source.Any<IProperty>((Func<IProperty, bool>) (x => x.IsDirty));
      this.IsValid = !source.Any<IProperty>((Func<IProperty, bool>) (x => !x.IsValid)) && !this.Errors.Any<string>();
      this.ValidationFinished();
      this.ValidationFinished -= new ModelBase<T>.ValidationFinishedHandler(this.OnValidationFinished);
      return this.IsValid;
    }

    protected IEnumerable<PropertyInfo> Properties()
    {
      if (this.properties != null)
        return this.properties;
      TypeInfo typeinfo = typeof (IProperty).GetTypeInfo();
      return this.properties = typeof (T).GetRuntimeProperties().Where<PropertyInfo>((Func<PropertyInfo, bool>) (x => typeinfo.IsAssignableFrom(x.PropertyType.GetTypeInfo())));
    }

    protected abstract void OnValidationFinished();

        private delegate void ValidationFinishedHandler();// where T : IModel;
  }
}
