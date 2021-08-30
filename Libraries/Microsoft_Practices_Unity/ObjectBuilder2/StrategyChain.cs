// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.StrategyChain
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class StrategyChain : IStrategyChain, IEnumerable<IBuilderStrategy>, IEnumerable
  {
    private readonly List<IBuilderStrategy> strategies = new List<IBuilderStrategy>();

    public StrategyChain()
    {
    }

    public StrategyChain(IEnumerable strategies) => this.AddRange(strategies);

    public void Add(IBuilderStrategy strategy) => this.strategies.Add(strategy);

    public void AddRange(IEnumerable strategyEnumerable)
    {
      Guard.ArgumentNotNull((object) strategyEnumerable, nameof (strategyEnumerable));
      foreach (IBuilderStrategy strategy in strategyEnumerable)
        this.Add(strategy);
    }

    public IStrategyChain Reverse()
    {
      List<IBuilderStrategy> builderStrategyList = new List<IBuilderStrategy>((IEnumerable<IBuilderStrategy>) this.strategies);
      builderStrategyList.Reverse();
      return (IStrategyChain) new StrategyChain((IEnumerable) builderStrategyList);
    }

    public object ExecuteBuildUp(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      int index1 = 0;
      try
      {
        for (; index1 < this.strategies.Count && !context.BuildComplete; ++index1)
          this.strategies[index1].PreBuildUp(context);
        if (context.BuildComplete)
          --index1;
        for (int index2 = index1 - 1; index2 >= 0; --index2)
          this.strategies[index2].PostBuildUp(context);
        return context.Existing;
      }
      catch (Exception ex)
      {
        context.RecoveryStack.ExecuteRecovery();
        throw;
      }
    }

    public void ExecuteTearDown(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      int index1 = 0;
      try
      {
        for (; index1 < this.strategies.Count; ++index1)
        {
          if (context.BuildComplete)
          {
            --index1;
            break;
          }
          this.strategies[index1].PreTearDown(context);
        }
        for (int index2 = index1 - 1; index2 >= 0; --index2)
          this.strategies[index2].PostTearDown(context);
      }
      catch (Exception ex)
      {
        context.RecoveryStack.ExecuteRecovery();
        throw;
      }
    }

    IEnumerator<IBuilderStrategy> IEnumerable<IBuilderStrategy>.GetEnumerator() => (IEnumerator<IBuilderStrategy>) this.strategies.GetEnumerator();

    public IEnumerator GetEnumerator() => (IEnumerator) this.strategies.GetEnumerator();
  }
}
