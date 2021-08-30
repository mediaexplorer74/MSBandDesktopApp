// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.Utility.Guard
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Properties;
using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Practices.Unity.Utility
{
  public static class Guard
  {
    public static void ArgumentNotNull(object argumentValue, string argumentName)
    {
      if (argumentValue == null)
        throw new ArgumentNullException(argumentName);
    }

    public static void ArgumentNotNullOrEmpty(string argumentValue, string argumentName)
    {
      switch (argumentValue)
      {
        case "":
          throw new ArgumentException(Resources.ArgumentMustNotBeEmpty, argumentName);
        case null:
          throw new ArgumentNullException(argumentName);
      }
    }

    public static void TypeIsAssignable(
      Type assignmentTargetType,
      Type assignmentValueType,
      string argumentName)
    {
      if ((object) assignmentTargetType == null)
        throw new ArgumentNullException(nameof (assignmentTargetType));
      if ((object) assignmentValueType == null)
        throw new ArgumentNullException(nameof (assignmentValueType));
      if (!assignmentTargetType.GetTypeInfo().IsAssignableFrom(assignmentValueType.GetTypeInfo()))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.TypesAreNotAssignable, new object[2]
        {
          (object) assignmentTargetType,
          (object) assignmentValueType
        }), argumentName);
    }

    public static void InstanceIsAssignable(
      Type assignmentTargetType,
      object assignmentInstance,
      string argumentName)
    {
      if ((object) assignmentTargetType == null)
        throw new ArgumentNullException(nameof (assignmentTargetType));
      if (assignmentInstance == null)
        throw new ArgumentNullException(nameof (assignmentInstance));
      if (!assignmentTargetType.GetTypeInfo().IsAssignableFrom(assignmentInstance.GetType().GetTypeInfo()))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.TypesAreNotAssignable, new object[2]
        {
          (object) assignmentTargetType,
          (object) Guard.GetTypeName(assignmentInstance)
        }), argumentName);
    }

    private static string GetTypeName(object assignmentInstance)
    {
      try
      {
        return assignmentInstance.GetType().FullName;
      }
      catch (Exception ex)
      {
        return Resources.UnknownType;
      }
    }
  }
}
