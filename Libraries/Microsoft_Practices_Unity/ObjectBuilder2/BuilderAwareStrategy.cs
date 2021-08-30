// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.BuilderAwareStrategy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class BuilderAwareStrategy : BuilderStrategy
  {
    public override void PreBuildUp(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      if (!(context.Existing is IBuilderAware existing))
        return;
      existing.OnBuiltUp(context.BuildKey);
    }

    public override void PreTearDown(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      if (!(context.Existing is IBuilderAware existing))
        return;
      existing.OnTearingDown();
    }
  }
}
