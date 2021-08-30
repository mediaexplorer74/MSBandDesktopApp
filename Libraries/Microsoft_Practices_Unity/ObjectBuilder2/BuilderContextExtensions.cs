// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.BuilderContextExtensions
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;
using System.Collections.Generic;

namespace Microsoft.Practices.ObjectBuilder2
{
  public static class BuilderContextExtensions
  {
    public static TResult NewBuildUp<TResult>(this IBuilderContext context) => context.NewBuildUp<TResult>((string) null);

    public static TResult NewBuildUp<TResult>(this IBuilderContext context, string name)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      return (TResult) context.NewBuildUp(NamedTypeBuildKey.Make<TResult>(name));
    }

    public static void AddResolverOverrides(
      this IBuilderContext context,
      params ResolverOverride[] overrides)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      context.AddResolverOverrides((IEnumerable<ResolverOverride>) overrides);
    }
  }
}
