// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Helpers.WeakAction
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

using System;
using System.Reflection;

namespace GalaSoft.MvvmLight.Helpers
{
  public class WeakAction
  {
    private Action _staticAction;

    protected MethodInfo Method { get; set; }

    public virtual string MethodName => this._staticAction != null ? this._staticAction.GetMethodInfo().Name : this.Method.Name;

    protected WeakReference ActionReference { get; set; }

    protected WeakReference Reference { get; set; }

    public bool IsStatic => this._staticAction != null;

    protected WeakAction()
    {
    }

    public WeakAction(Action action)
      : this(action == null ? (object) null : action.Target, action)
    {
    }

    public WeakAction(object target, Action action)
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

    public virtual bool IsAlive
    {
      get
      {
        if (this._staticAction == null && this.Reference == null)
          return false;
        return this._staticAction != null && this.Reference == null || this.Reference.IsAlive;
      }
    }

    public object Target => this.Reference == null ? (object) null : this.Reference.Target;

    protected object ActionTarget => this.ActionReference == null ? (object) null : this.ActionReference.Target;

    public void Execute()
    {
      if (this._staticAction != null)
      {
        this._staticAction();
      }
      else
      {
        object actionTarget = this.ActionTarget;
        if (!this.IsAlive || (object) this.Method == null || this.ActionReference == null || actionTarget == null)
          return;
        this.Method.Invoke(actionTarget, (object[]) null);
      }
    }

    public void MarkForDeletion()
    {
      this.Reference = (WeakReference) null;
      this.ActionReference = (WeakReference) null;
      this.Method = (MethodInfo) null;
      this._staticAction = (Action) null;
    }
  }
}
