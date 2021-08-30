// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.DynamicMethodPropertySetterStrategy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Properties;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class DynamicMethodPropertySetterStrategy : BuilderStrategy
  {
    private static readonly MethodInfo SetCurrentOperationToResolvingPropertyValueMethod = StaticReflection.GetMethodInfo((Expression<Action>) (() => DynamicMethodPropertySetterStrategy.SetCurrentOperationToResolvingPropertyValue(default (string), default (IBuilderContext))));
    private static readonly MethodInfo SetCurrentOperationToSettingPropertyMethod = StaticReflection.GetMethodInfo((Expression<Action>) (() => DynamicMethodPropertySetterStrategy.SetCurrentOperationToSettingProperty(default (string), default (IBuilderContext))));

    public override void PreBuildUp(IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      DynamicBuildPlanGenerationContext existing = (DynamicBuildPlanGenerationContext) context.Existing;
      IPolicyList containingPolicyList;
      IPropertySelectorPolicy propertySelectorPolicy = context.Policies.Get<IPropertySelectorPolicy>((object) context.BuildKey, out containingPolicyList);
      bool flag = false;
      foreach (SelectedProperty selectProperty in propertySelectorPolicy.SelectProperties(context, containingPolicyList))
      {
        flag = true;
        ParameterExpression parameterExpression = Expression.Parameter(selectProperty.Property.PropertyType);
        existing.AddToBuildPlan((Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[1]
        {
          parameterExpression
        }, (Expression) Expression.Call((Expression) null, DynamicMethodPropertySetterStrategy.SetCurrentOperationToResolvingPropertyValueMethod, (Expression) Expression.Constant((object) selectProperty.Property.Name), (Expression) existing.ContextParameter), (Expression) Expression.Assign((Expression) parameterExpression, existing.GetResolveDependencyExpression(selectProperty.Property.PropertyType, selectProperty.Resolver)), (Expression) Expression.Call((Expression) null, DynamicMethodPropertySetterStrategy.SetCurrentOperationToSettingPropertyMethod, (Expression) Expression.Constant((object) selectProperty.Property.Name), (Expression) existing.ContextParameter), (Expression) Expression.Call((Expression) Expression.Convert(existing.GetExistingObjectExpression(), existing.TypeToBuild), DynamicMethodPropertySetterStrategy.GetValidatedPropertySetter(selectProperty.Property), (Expression) parameterExpression)));
      }
      if (!flag)
        return;
      existing.AddToBuildPlan(existing.GetClearCurrentOperationExpression());
    }

    private static MethodInfo GetValidatedPropertySetter(PropertyInfo property)
    {
      MethodInfo setMethod = property.SetMethod;
      return (object) setMethod != null && !setMethod.IsPrivate ? setMethod : throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.PropertyNotSettable, new object[2]
      {
        (object) property.Name,
        (object) property.DeclaringType.FullName
      }));
    }

    public static void SetCurrentOperationToResolvingPropertyValue(
      string propertyName,
      IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      context.CurrentOperation = (object) new ResolvingPropertyValueOperation(context.BuildKey.Type, propertyName);
    }

    public static void SetCurrentOperationToSettingProperty(
      string propertyName,
      IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      context.CurrentOperation = (object) new SettingPropertyOperation(context.BuildKey.Type, propertyName);
    }
  }
}
