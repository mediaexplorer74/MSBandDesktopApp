// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.Messenger
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

using GalaSoft.MvvmLight.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace GalaSoft.MvvmLight.Messaging
{
  public class Messenger : IMessenger
  {
    private static readonly object CreationLock = new object();
    private static IMessenger _defaultInstance;
    private readonly object _registerLock = new object();
    private Dictionary<Type, List<Messenger.WeakActionAndToken>> _recipientsOfSubclassesAction;
    private Dictionary<Type, List<Messenger.WeakActionAndToken>> _recipientsStrictAction;
    private readonly SynchronizationContext _context = SynchronizationContext.Current;
    private bool _isCleanupRegistered;

    public static IMessenger Default
    {
      get
      {
        if (Messenger._defaultInstance == null)
        {
          lock (Messenger.CreationLock)
          {
            if (Messenger._defaultInstance == null)
              Messenger._defaultInstance = (IMessenger) new Messenger();
          }
        }
        return Messenger._defaultInstance;
      }
    }

    public virtual void Register<TMessage>(object recipient, Action<TMessage> action) => this.Register<TMessage>(recipient, (object) null, false, action);

    public virtual void Register<TMessage>(
      object recipient,
      bool receiveDerivedMessagesToo,
      Action<TMessage> action)
    {
      this.Register<TMessage>(recipient, (object) null, receiveDerivedMessagesToo, action);
    }

    public virtual void Register<TMessage>(object recipient, object token, Action<TMessage> action) => this.Register<TMessage>(recipient, token, false, action);

    public virtual void Register<TMessage>(
      object recipient,
      object token,
      bool receiveDerivedMessagesToo,
      Action<TMessage> action)
    {
      lock (this._registerLock)
      {
        Type key = typeof (TMessage);
        Dictionary<Type, List<Messenger.WeakActionAndToken>> dictionary;
        if (receiveDerivedMessagesToo)
        {
          if (this._recipientsOfSubclassesAction == null)
            this._recipientsOfSubclassesAction = new Dictionary<Type, List<Messenger.WeakActionAndToken>>();
          dictionary = this._recipientsOfSubclassesAction;
        }
        else
        {
          if (this._recipientsStrictAction == null)
            this._recipientsStrictAction = new Dictionary<Type, List<Messenger.WeakActionAndToken>>();
          dictionary = this._recipientsStrictAction;
        }
        lock (dictionary)
        {
          List<Messenger.WeakActionAndToken> weakActionAndTokenList;
          if (!dictionary.ContainsKey(key))
          {
            weakActionAndTokenList = new List<Messenger.WeakActionAndToken>();
            dictionary.Add(key, weakActionAndTokenList);
          }
          else
            weakActionAndTokenList = dictionary[key];
          WeakAction<TMessage> weakAction = new WeakAction<TMessage>(recipient, action);
          Messenger.WeakActionAndToken weakActionAndToken = new Messenger.WeakActionAndToken()
          {
            Action = (WeakAction) weakAction,
            Token = token
          };
          weakActionAndTokenList.Add(weakActionAndToken);
        }
      }
      this.RequestCleanup();
    }

    public virtual void Send<TMessage>(TMessage message) => this.SendToTargetOrType<TMessage>(message, (Type) null, (object) null);

    public virtual void Send<TMessage, TTarget>(TMessage message) => this.SendToTargetOrType<TMessage>(message, typeof (TTarget), (object) null);

    public virtual void Send<TMessage>(TMessage message, object token) => this.SendToTargetOrType<TMessage>(message, (Type) null, token);

    public virtual void Unregister(object recipient)
    {
      Messenger.UnregisterFromLists(recipient, this._recipientsOfSubclassesAction);
      Messenger.UnregisterFromLists(recipient, this._recipientsStrictAction);
    }

    public virtual void Unregister<TMessage>(object recipient) => this.Unregister<TMessage>(recipient, (object) null, (Action<TMessage>) null);

    public virtual void Unregister<TMessage>(object recipient, object token) => this.Unregister<TMessage>(recipient, token, (Action<TMessage>) null);

    public virtual void Unregister<TMessage>(object recipient, Action<TMessage> action) => this.Unregister<TMessage>(recipient, (object) null, action);

    public virtual void Unregister<TMessage>(
      object recipient,
      object token,
      Action<TMessage> action)
    {
      Messenger.UnregisterFromLists<TMessage>(recipient, token, action, this._recipientsStrictAction);
      Messenger.UnregisterFromLists<TMessage>(recipient, token, action, this._recipientsOfSubclassesAction);
      this.RequestCleanup();
    }

    public static void OverrideDefault(IMessenger newMessenger) => Messenger._defaultInstance = newMessenger;

    public static void Reset() => Messenger._defaultInstance = (IMessenger) null;

    public void ResetAll() => Messenger.Reset();

    private static void CleanupList(
      IDictionary<Type, List<Messenger.WeakActionAndToken>> lists)
    {
      if (lists == null)
        return;
      lock (lists)
      {
        List<Type> typeList = new List<Type>();
        foreach (KeyValuePair<Type, List<Messenger.WeakActionAndToken>> list in (IEnumerable<KeyValuePair<Type, List<Messenger.WeakActionAndToken>>>) lists)
        {
          foreach (Messenger.WeakActionAndToken weakActionAndToken in list.Value.Where<Messenger.WeakActionAndToken>((Func<Messenger.WeakActionAndToken, bool>) (item => item.Action == null || !item.Action.IsAlive)).ToList<Messenger.WeakActionAndToken>())
            list.Value.Remove(weakActionAndToken);
          if (list.Value.Count == 0)
            typeList.Add(list.Key);
        }
        foreach (Type key in typeList)
          lists.Remove(key);
      }
    }

    private static void SendToList<TMessage>(
      TMessage message,
      IEnumerable<Messenger.WeakActionAndToken> weakActionsAndTokens,
      Type messageTargetType,
      object token)
    {
      if (weakActionsAndTokens == null)
        return;
      List<Messenger.WeakActionAndToken> list = weakActionsAndTokens.ToList<Messenger.WeakActionAndToken>();
      foreach (Messenger.WeakActionAndToken weakActionAndToken in list.Take<Messenger.WeakActionAndToken>(list.Count<Messenger.WeakActionAndToken>()).ToList<Messenger.WeakActionAndToken>())
      {
        if (weakActionAndToken.Action is IExecuteWithObject action1 && weakActionAndToken.Action.IsAlive && weakActionAndToken.Action.Target != null && ((object) messageTargetType == null || (object) weakActionAndToken.Action.Target.GetType() == (object) messageTargetType || messageTargetType.GetTypeInfo().IsAssignableFrom(weakActionAndToken.Action.Target.GetType().GetTypeInfo())) && (weakActionAndToken.Token == null && token == null || weakActionAndToken.Token != null && weakActionAndToken.Token.Equals(token)))
          action1.ExecuteWithObject((object) message);
      }
    }

    private static void UnregisterFromLists(
      object recipient,
      Dictionary<Type, List<Messenger.WeakActionAndToken>> lists)
    {
      if (recipient == null || lists == null || lists.Count == 0)
        return;
      lock (lists)
      {
        foreach (Type key in lists.Keys)
        {
          foreach (Messenger.WeakActionAndToken weakActionAndToken in lists[key])
          {
            IExecuteWithObject action = (IExecuteWithObject) weakActionAndToken.Action;
            if (action != null && recipient == action.Target)
              action.MarkForDeletion();
          }
        }
      }
    }

    private static void UnregisterFromLists<TMessage>(
      object recipient,
      object token,
      Action<TMessage> action,
      Dictionary<Type, List<Messenger.WeakActionAndToken>> lists)
    {
      Type key = typeof (TMessage);
      if (recipient == null || lists == null || lists.Count == 0 || !lists.ContainsKey(key))
        return;
      lock (lists)
      {
        foreach (Messenger.WeakActionAndToken weakActionAndToken in lists[key])
        {
          if (weakActionAndToken.Action is WeakAction<TMessage> action3 && recipient == action3.Target && (action == null || action.GetMethodInfo().Name == action3.MethodName) && (token == null || token.Equals(weakActionAndToken.Token)))
            weakActionAndToken.Action.MarkForDeletion();
        }
      }
    }

    public void RequestCleanup()
    {
      if (this._isCleanupRegistered)
        return;
      Action cleanupAction = new Action(this.Cleanup);
      if (this._context != null)
        this._context.Post((SendOrPostCallback) (_ => cleanupAction()), (object) null);
      else
        cleanupAction();
      this._isCleanupRegistered = true;
    }

    public void Cleanup()
    {
      Messenger.CleanupList((IDictionary<Type, List<Messenger.WeakActionAndToken>>) this._recipientsOfSubclassesAction);
      Messenger.CleanupList((IDictionary<Type, List<Messenger.WeakActionAndToken>>) this._recipientsStrictAction);
      this._isCleanupRegistered = false;
    }

    private void SendToTargetOrType<TMessage>(
      TMessage message,
      Type messageTargetType,
      object token)
    {
      Type type1 = typeof (TMessage);
      if (this._recipientsOfSubclassesAction != null)
      {
        foreach (Type type2 in this._recipientsOfSubclassesAction.Keys.Take<Type>(this._recipientsOfSubclassesAction.Count<KeyValuePair<Type, List<Messenger.WeakActionAndToken>>>()).ToList<Type>())
        {
          List<Messenger.WeakActionAndToken> weakActionAndTokenList = (List<Messenger.WeakActionAndToken>) null;
          if ((object) type1 == (object) type2 || type1.GetTypeInfo().IsSubclassOf(type2) || type2.GetTypeInfo().IsAssignableFrom(type1.GetTypeInfo()))
          {
            lock (this._recipientsOfSubclassesAction)
              weakActionAndTokenList = this._recipientsOfSubclassesAction[type2].Take<Messenger.WeakActionAndToken>(this._recipientsOfSubclassesAction[type2].Count<Messenger.WeakActionAndToken>()).ToList<Messenger.WeakActionAndToken>();
          }
          Messenger.SendToList<TMessage>(message, (IEnumerable<Messenger.WeakActionAndToken>) weakActionAndTokenList, messageTargetType, token);
        }
      }
      if (this._recipientsStrictAction != null)
      {
        List<Messenger.WeakActionAndToken> weakActionAndTokenList = (List<Messenger.WeakActionAndToken>) null;
        lock (this._recipientsStrictAction)
        {
          if (this._recipientsStrictAction.ContainsKey(type1))
            weakActionAndTokenList = this._recipientsStrictAction[type1].Take<Messenger.WeakActionAndToken>(this._recipientsStrictAction[type1].Count<Messenger.WeakActionAndToken>()).ToList<Messenger.WeakActionAndToken>();
        }
        if (weakActionAndTokenList != null)
          Messenger.SendToList<TMessage>(message, (IEnumerable<Messenger.WeakActionAndToken>) weakActionAndTokenList, messageTargetType, token);
      }
      this.RequestCleanup();
    }

    private struct WeakActionAndToken
    {
      public WeakAction Action;
      public object Token;
    }
  }
}
