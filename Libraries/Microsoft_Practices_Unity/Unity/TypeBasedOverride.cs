// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.TypeBasedOverride
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Utility;
using System;

namespace Microsoft.Practices.Unity
{
  public class TypeBasedOverride : ResolverOverride
  {
    private readonly Type targetType;
    private readonly ResolverOverride innerOverride;

    public TypeBasedOverride(Type targetType, ResolverOverride innerOverride)
    {
      Guard.ArgumentNotNull((object) targetType, nameof (targetType));
      Guard.ArgumentNotNull((object) innerOverride, nameof (innerOverride));
      this.targetType = targetType;
      this.innerOverride = innerOverride;
    }

    public override IDependencyResolverPolicy GetResolver(
      IBuilderContext context,
      Type dependencyType)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      return context.CurrentOperation is BuildOperation currentOperation && (object) currentOperation.TypeBeingConstructed == (object) this.targetType ? this.innerOverride.GetResolver(context, dependencyType) : (IDependencyResolverPolicy) null;
    }
  }
}
