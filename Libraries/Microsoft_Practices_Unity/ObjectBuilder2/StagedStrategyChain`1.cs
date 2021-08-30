// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.StagedStrategyChain`1
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class StagedStrategyChain<TStageEnum> : IStagedStrategyChain
  {
    private readonly StagedStrategyChain<TStageEnum> innerChain;
    private readonly object lockObject = new object();
    private readonly List<IBuilderStrategy>[] stages;

    public StagedStrategyChain()
    {
      this.stages = new List<IBuilderStrategy>[StagedStrategyChain<TStageEnum>.NumberOfEnumValues()];
      for (int index = 0; index < this.stages.Length; ++index)
        this.stages[index] = new List<IBuilderStrategy>();
    }

    public StagedStrategyChain(StagedStrategyChain<TStageEnum> innerChain)
      : this()
    {
      this.innerChain = innerChain;
    }

    public void Add(IBuilderStrategy strategy, TStageEnum stage)
    {
      lock (this.lockObject)
        this.stages[Convert.ToInt32((object) stage)].Add(strategy);
    }

    public void AddNew<TStrategy>(TStageEnum stage) where TStrategy : IBuilderStrategy, new() => this.Add((IBuilderStrategy) new TStrategy(), stage);

    public void Clear()
    {
      lock (this.lockObject)
      {
        foreach (List<IBuilderStrategy> stage in this.stages)
          stage.Clear();
      }
    }

    public IStrategyChain MakeStrategyChain()
    {
      lock (this.lockObject)
      {
        StrategyChain chain = new StrategyChain();
        for (int index = 0; index < this.stages.Length; ++index)
          this.FillStrategyChain(chain, index);
        return (IStrategyChain) chain;
      }
    }

    private void FillStrategyChain(StrategyChain chain, int index)
    {
      lock (this.lockObject)
      {
        if (this.innerChain != null)
          this.innerChain.FillStrategyChain(chain, index);
        chain.AddRange((IEnumerable) this.stages[index]);
      }
    }

    private static int NumberOfEnumValues() => typeof (TStageEnum).GetTypeInfo().DeclaredFields.Where<FieldInfo>((Func<FieldInfo, bool>) (f => f.IsPublic && f.IsStatic)).Count<FieldInfo>();
  }
}
