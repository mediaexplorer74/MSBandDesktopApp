// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.GenericTypeBuildKeyMappingPolicy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Properties;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class GenericTypeBuildKeyMappingPolicy : IBuildKeyMappingPolicy, IBuilderPolicy
  {
    private readonly NamedTypeBuildKey destinationKey;

    public GenericTypeBuildKeyMappingPolicy(NamedTypeBuildKey destinationKey)
    {
      Guard.ArgumentNotNull((object) destinationKey, nameof (destinationKey));
      if (!destinationKey.Type.GetTypeInfo().IsGenericTypeDefinition)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.MustHaveOpenGenericType, new object[1]
        {
          (object) destinationKey.Type.GetTypeInfo().Name
        }));
      this.destinationKey = destinationKey;
    }

    public NamedTypeBuildKey Map(
      NamedTypeBuildKey buildKey,
      IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) buildKey, nameof (buildKey));
      TypeInfo typeInfo = buildKey.Type.GetTypeInfo();
      if (typeInfo.IsGenericTypeDefinition)
        return this.destinationKey;
      this.GuardSameNumberOfGenericArguments(typeInfo);
      return new NamedTypeBuildKey(this.destinationKey.Type.MakeGenericType(typeInfo.GenericTypeArguments), this.destinationKey.Name);
    }

    private void GuardSameNumberOfGenericArguments(TypeInfo sourceTypeInfo)
    {
      if (sourceTypeInfo.GenericTypeArguments.Length != this.DestinationType.GetTypeInfo().GenericTypeParameters.Length)
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.MustHaveSameNumberOfGenericArguments, new object[2]
        {
          (object) sourceTypeInfo.Name,
          (object) this.DestinationType.Name
        }), nameof (sourceTypeInfo));
    }

    private Type DestinationType => this.destinationKey.Type;
  }
}
