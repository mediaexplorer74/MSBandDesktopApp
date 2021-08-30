// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.IPolicyList
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;

namespace Microsoft.Practices.ObjectBuilder2
{
  public interface IPolicyList
  {
    void Clear(Type policyInterface, object buildKey);

    void ClearAll();

    void ClearDefault(Type policyInterface);

    IBuilderPolicy Get(
      Type policyInterface,
      object buildKey,
      bool localOnly,
      out IPolicyList containingPolicyList);

    IBuilderPolicy GetNoDefault(
      Type policyInterface,
      object buildKey,
      bool localOnly,
      out IPolicyList containingPolicyList);

    void Set(Type policyInterface, IBuilderPolicy policy, object buildKey);

    void SetDefault(Type policyInterface, IBuilderPolicy policy);
  }
}
