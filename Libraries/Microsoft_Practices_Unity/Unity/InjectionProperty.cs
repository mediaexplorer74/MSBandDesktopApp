// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.InjectionProperty
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.ObjectBuilder;
using Microsoft.Practices.Unity.Properties;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Microsoft.Practices.Unity
{
  public class InjectionProperty : InjectionMember
  {
    private readonly string propertyName;
    private InjectionParameterValue parameterValue;

    public InjectionProperty(string propertyName) => this.propertyName = propertyName;

    public InjectionProperty(string propertyName, object propertyValue)
    {
      this.propertyName = propertyName;
      this.parameterValue = InjectionParameterValue.ToParameter(propertyValue);
    }

    public override void AddPolicies(
      Type serviceType,
      Type implementationType,
      string name,
      IPolicyList policies)
    {
      Guard.ArgumentNotNull((object) implementationType, nameof (implementationType));
      PropertyInfo propertyInfo = implementationType.GetPropertiesHierarchical().FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.Name == this.propertyName && !p.SetMethod.IsStatic));
      InjectionProperty.GuardPropertyExists(propertyInfo, implementationType, this.propertyName);
      InjectionProperty.GuardPropertyIsSettable(propertyInfo);
      InjectionProperty.GuardPropertyIsNotIndexer(propertyInfo);
      this.InitializeParameterValue(propertyInfo);
      InjectionProperty.GuardPropertyValueIsCompatible(propertyInfo, this.parameterValue);
      InjectionProperty.GetSelectorPolicy(policies, implementationType, name).AddPropertyAndValue(propertyInfo, this.parameterValue);
    }

    private InjectionParameterValue InitializeParameterValue(
      PropertyInfo propInfo)
    {
      if (this.parameterValue == null)
        this.parameterValue = (InjectionParameterValue) new ResolvedParameter(propInfo.PropertyType);
      return this.parameterValue;
    }

    private static SpecifiedPropertiesSelectorPolicy GetSelectorPolicy(
      IPolicyList policies,
      Type typeToInject,
      string name)
    {
      NamedTypeBuildKey namedTypeBuildKey = new NamedTypeBuildKey(typeToInject, name);
      IPropertySelectorPolicy policy = policies.GetNoDefault<IPropertySelectorPolicy>((object) namedTypeBuildKey, false);
      if (policy == null || !(policy is SpecifiedPropertiesSelectorPolicy))
      {
        policy = (IPropertySelectorPolicy) new SpecifiedPropertiesSelectorPolicy();
        policies.Set<IPropertySelectorPolicy>(policy, (object) namedTypeBuildKey);
      }
      return (SpecifiedPropertiesSelectorPolicy) policy;
    }

    private static void GuardPropertyExists(
      PropertyInfo propInfo,
      Type typeToCreate,
      string propertyName)
    {
      if ((object) propInfo == null)
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.NoSuchProperty, new object[2]
        {
          (object) typeToCreate.GetTypeInfo().Name,
          (object) propertyName
        }));
    }

    private static void GuardPropertyIsSettable(PropertyInfo propInfo)
    {
      if (!propInfo.CanWrite)
        throw new InvalidOperationException(InjectionProperty.ExceptionMessage(Resources.PropertyNotSettable, (object) propInfo.Name, (object) propInfo.DeclaringType));
    }

    private static void GuardPropertyIsNotIndexer(PropertyInfo property)
    {
      if (property.GetIndexParameters().Length > 0)
        throw new InvalidOperationException(InjectionProperty.ExceptionMessage(Resources.CannotInjectIndexer, (object) property.Name, (object) property.DeclaringType));
    }

    private static void GuardPropertyValueIsCompatible(
      PropertyInfo property,
      InjectionParameterValue value)
    {
      if (!value.MatchesType(property.PropertyType))
        throw new InvalidOperationException(InjectionProperty.ExceptionMessage(Resources.PropertyTypeMismatch, (object) property.Name, (object) property.DeclaringType, (object) property.PropertyType, (object) value.ParameterTypeName));
    }

    private static string ExceptionMessage(string format, params object[] args)
    {
      for (int index = 0; index < args.Length; ++index)
      {
        if ((object) (args[index] as Type) != null)
          args[index] = (object) ((Type) args[index]).GetTypeInfo().Name;
      }
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args);
    }
  }
}
