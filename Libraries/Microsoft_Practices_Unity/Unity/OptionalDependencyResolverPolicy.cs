// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.OptionalDependencyResolverPolicy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Properties;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Practices.Unity
{
  public class OptionalDependencyResolverPolicy : IDependencyResolverPolicy, IBuilderPolicy
  {
    private readonly Type type;
    private readonly string name;

    public OptionalDependencyResolverPolicy(Type type, string name)
    {
      Guard.ArgumentNotNull((object) type, nameof (type));
      this.type = !type.GetTypeInfo().IsValueType ? type : throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.OptionalDependenciesMustBeReferenceTypes, new object[1]
      {
        (object) type.GetTypeInfo().Name
      }));
      this.name = name;
    }

    public OptionalDependencyResolverPolicy(Type type)
      : this(type, (string) null)
    {
    }

    public Type DependencyType => this.type;

    public string Name => this.name;

    public object Resolve(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      NamedTypeBuildKey newBuildKey = new NamedTypeBuildKey(this.type, this.name);
      try
      {
        return context.NewBuildUp(newBuildKey);
      }
      catch (Exception ex)
      {
        return (object) null;
      }
    }
  }
}
