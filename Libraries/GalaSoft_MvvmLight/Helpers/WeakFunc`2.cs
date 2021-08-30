// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Helpers.WeakFunc`2
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

using System;
using System.Reflection;

namespace GalaSoft.MvvmLight.Helpers
{
  public class WeakFunc<T, TResult> : WeakFunc<TResult>, IExecuteWithObjectAndResult
  {
    private Func<T, TResult> _staticFunc;

    public override string MethodName => this._staticFunc != null ? this._staticFunc.GetMethodInfo().Name : this.Method.Name;

    public override bool IsAlive
    {
      get
      {
        if (this._staticFunc == null && this.Reference == null)
          return false;
        if (this._staticFunc == null)
          return this.Reference.IsAlive;
        return this.Reference == null || this.Reference.IsAlive;
      }
    }

    public WeakFunc(Func<T, TResult> func)
      : this(func == null ? (object) null : func.Target, func)
    {
    }

    public WeakFunc(object target, Func<T, TResult> func)
    {
      if (func.GetMethodInfo().IsStatic)
      {
        this._staticFunc = func;
        if (target == null)
          return;
        this.Reference = new WeakReference(target);
      }
      else
      {
        this.Method = func.GetMethodInfo();
        this.FuncReference = new WeakReference(func.Target);
        this.Reference = new WeakReference(target);
      }
    }

    public new TResult Execute() => this.Execute(default (T));

    public TResult Execute(T parameter)
    {
      if (this._staticFunc != null)
        return this._staticFunc(parameter);
      object funcTarget = this.FuncTarget;
      if (!this.IsAlive || (object) this.Method == null || this.FuncReference == null || funcTarget == null)
        return default (TResult);
      return (TResult) this.Method.Invoke(funcTarget, new object[1]
      {
        (object) parameter
      });
    }

    public object ExecuteWithObject(object parameter) => (object) this.Execute((T) parameter);

    public new void MarkForDeletion()
    {
      this._staticFunc = (Func<T, TResult>) null;
      base.MarkForDeletion();
    }
  }
}
