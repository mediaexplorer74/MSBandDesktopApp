// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Helpers.WeakFunc`1
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

using System;
using System.Reflection;

namespace GalaSoft.MvvmLight.Helpers
{
  public class WeakFunc<TResult>
  {
    private Func<TResult> _staticFunc;

    protected MethodInfo Method { get; set; }

    public bool IsStatic => this._staticFunc != null;

    public virtual string MethodName => this._staticFunc != null ? this._staticFunc.GetMethodInfo().Name : this.Method.Name;

    protected WeakReference FuncReference { get; set; }

    protected WeakReference Reference { get; set; }

    protected WeakFunc()
    {
    }

    public WeakFunc(Func<TResult> func)
      : this(func == null ? (object) null : func.Target, func)
    {
    }

    public WeakFunc(object target, Func<TResult> func)
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

    public virtual bool IsAlive
    {
      get
      {
        if (this._staticFunc == null && this.Reference == null)
          return false;
        return this._staticFunc != null && this.Reference == null || this.Reference.IsAlive;
      }
    }

    public object Target => this.Reference == null ? (object) null : this.Reference.Target;

    protected object FuncTarget => this.FuncReference == null ? (object) null : this.FuncReference.Target;

    public TResult Execute()
    {
      if (this._staticFunc != null)
        return this._staticFunc();
      object funcTarget = this.FuncTarget;
      return this.IsAlive && (object) this.Method != null && this.FuncReference != null && funcTarget != null ? (TResult) this.Method.Invoke(funcTarget, (object[]) null) : default (TResult);
    }

    public void MarkForDeletion()
    {
      this.Reference = (WeakReference) null;
      this.FuncReference = (WeakReference) null;
      this.Method = (MethodInfo) null;
      this._staticFunc = (Func<TResult>) null;
    }
  }
}
