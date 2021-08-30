// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.ObservableObject
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GalaSoft.MvvmLight
{
  public class ObservableObject : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected PropertyChangedEventHandler PropertyChangedHandler => this.PropertyChanged;

    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public void VerifyPropertyName(string propertyName)
    {
      Type type = this.GetType();
      if (!string.IsNullOrEmpty(propertyName) && (object) type.GetTypeInfo().GetDeclaredProperty(propertyName) == null)
        throw new ArgumentException("Property not found", propertyName);
    }

    protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      string propertyName = ObservableObject.GetPropertyName<T>(propertyExpression);
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    protected static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
    {
      if (propertyExpression == null)
        throw new ArgumentNullException(nameof (propertyExpression));
      if (!(propertyExpression.Body is MemberExpression body))
        throw new ArgumentException("Invalid argument", nameof (propertyExpression));
      return (body.Member as PropertyInfo ?? throw new ArgumentException("Argument is not a property", nameof (propertyExpression))).Name;
    }

    protected bool Set<T>(Expression<Func<T>> propertyExpression, ref T field, T newValue)
    {
      if (EqualityComparer<T>.Default.Equals(field, newValue))
        return false;
      field = newValue;
      this.RaisePropertyChanged<T>(propertyExpression);
      return true;
    }

    protected bool Set<T>(string propertyName, ref T field, T newValue)
    {
      if (EqualityComparer<T>.Default.Equals(field, newValue))
        return false;
      field = newValue;
      this.RaisePropertyChanged(propertyName);
      return true;
    }

    protected bool Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null) => this.Set<T>(propertyName, ref field, newValue);
  }
}
