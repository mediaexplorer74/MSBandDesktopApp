// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.BuildKeyMappingStrategy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class BuildKeyMappingStrategy : BuilderStrategy
  {
    public override void PreBuildUp(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      IBuildKeyMappingPolicy keyMappingPolicy = context.Policies.Get<IBuildKeyMappingPolicy>((object) context.BuildKey);
      if (keyMappingPolicy == null)
        return;
      context.BuildKey = keyMappingPolicy.Map(context.BuildKey, context);
    }
  }
}
