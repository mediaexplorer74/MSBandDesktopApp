// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.SelectedProperty
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class SelectedProperty
  {
    private readonly PropertyInfo property;
    private readonly IDependencyResolverPolicy resolver;

    public SelectedProperty(PropertyInfo property, IDependencyResolverPolicy resolver)
    {
      this.property = property;
      this.resolver = resolver;
    }

    public PropertyInfo Property => this.property;

    public IDependencyResolverPolicy Resolver => this.resolver;
  }
}
