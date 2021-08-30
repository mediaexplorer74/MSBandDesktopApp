// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Helpers.WeakAction`1
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

using System;
using System.Reflection;

namespace GalaSoft.MvvmLight.Helpers
{
  public class WeakAction<T> : WeakAction, IExecuteWithObject
  {
    private Action<T> _staticAction;

    public override string MethodName => this._staticAction != null ? this._staticAction.GetMethodInfo().Name : this.Method.Name;

    public override bool IsAlive
    {
      get
      {
        if (this._staticAction == null && this.Reference == null)
          return false;
        if (this._staticAction == null)
          return this.Reference.IsAlive;
        return this.Reference == null || this.Reference.IsAlive;
      }
    }

    public WeakAction(Action<T> action)
      : this(action == null ? (object) null : action.Target, action)
    {
    }

    public WeakAction(object target, Action<T> action)
    {
      if (action.GetMethodInfo().IsStatic)
      {
        this._staticAction = action;
        if (target == null)
          return;
        this.Reference = new WeakReference(target);
      }
      else
      {
        this.Method = action.GetMethodInfo();
        this.ActionReference = new WeakReference(action.Target);
        this.Reference = new WeakReference(target);
      }
    }

    public new void Execute() => this.Execute(default (T));

    public void Execute(T parameter)
    {
      if (this._staticAction != null)
      {
        this._staticAction(parameter);
      }
      else
      {
        object actionTarget = this.ActionTarget;
        if (!this.IsAlive || (object) this.Method == null || this.ActionReference == null || actionTarget == null)
          return;
        this.Method.Invoke(actionTarget, new object[1]
        {
          (object) parameter
        });
      }
    }

    public void ExecuteWithObject(object parameter) => this.Execute((T) parameter);

    public new void MarkForDeletion()
    {
      this._staticAction = (Action<T>) null;
      base.MarkForDeletion();
    }
  }
}
