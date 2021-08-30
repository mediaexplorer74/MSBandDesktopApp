// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ExtensionContext
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.ObjectBuilder;
using System;

namespace Microsoft.Practices.Unity
{
  public abstract class ExtensionContext
  {
    public abstract IUnityContainer Container { get; }

    public abstract StagedStrategyChain<UnityBuildStage> Strategies { get; }

    public abstract StagedStrategyChain<UnityBuildStage> BuildPlanStrategies { get; }

    public abstract IPolicyList Policies { get; }

    public abstract ILifetimeContainer Lifetime { get; }

    public abstract void RegisterNamedType(Type t, string name);

    public abstract event EventHandler<RegisterEventArgs> Registering;

    public abstract event EventHandler<RegisterInstanceEventArgs> RegisteringInstance;

    public abstract event EventHandler<ChildContainerCreatedEventArgs> ChildContainerCreated;
  }
}
