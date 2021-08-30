// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.IoC.MvxConventionAttributeExtensionMethods
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;
using System.Linq;
using System.Reflection;

namespace Cirrious.CrossCore.IoC
{
  public static class MvxConventionAttributeExtensionMethods
  {
    public static bool IsConventional(this Type candidateType) => ReflectionExtensions.GetCustomAttributes(candidateType, typeof (MvxUnconventionalAttribute), true).Length <= 0 && candidateType.SatisfiesConditionalConventions();

    public static bool SatisfiesConditionalConventions(this Type candidateType)
    {
      foreach (MvxConditionalConventionalAttribute customAttribute in ReflectionExtensions.GetCustomAttributes(candidateType, typeof (MvxConditionalConventionalAttribute), true))
      {
        if (!customAttribute.IsConditionSatisfied)
          return false;
      }
      return true;
    }

    public static bool IsConventional(this PropertyInfo propertyInfo) => !CustomAttributeExtensions.GetCustomAttributes(propertyInfo, typeof (MvxUnconventionalAttribute), true).Any<Attribute>() && propertyInfo.SatisfiesConditionalConventions();

    public static bool SatisfiesConditionalConventions(this PropertyInfo propertyInfo)
    {
      foreach (MvxConditionalConventionalAttribute customAttribute in CustomAttributeExtensions.GetCustomAttributes(propertyInfo, typeof (MvxConditionalConventionalAttribute), true))
      {
        if (!customAttribute.IsConditionSatisfied)
          return false;
      }
      return true;
    }
  }
}
