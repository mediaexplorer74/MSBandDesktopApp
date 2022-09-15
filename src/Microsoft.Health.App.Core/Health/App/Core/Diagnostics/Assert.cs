// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Diagnostics.Assert
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Diagnostics
{
  public static class Assert
  {
    public static void ParamIsNotNull(object value, string parameterName)
    {
      Assert.LocalParameterIsNotNullOrWhiteSpace(parameterName, nameof (parameterName));
      if (value == null)
      {
        string message = string.Format("{0} should not be null.", new object[1]
        {
          (object) parameterName
        });
        DebugUtilities.Fail(message);
        throw new ArgumentNullException(parameterName, message);
      }
    }

    public static void ParamIsNotNullOrEmpty(string value, string parameterName)
    {
      Assert.LocalParameterIsNotNullOrWhiteSpace(parameterName, nameof (parameterName));
      if (value == null)
      {
        string str = string.Format("{0} should not be null.", new object[1]
        {
          (object) parameterName
        });
        DebugUtilities.Fail(str);
        throw new ArgumentException(parameterName, str);
      }
      if (string.IsNullOrEmpty(value))
      {
        string str = string.Format("{0} should not be empty.", new object[1]
        {
          (object) parameterName
        });
        DebugUtilities.Fail(str);
        throw new ArgumentException(parameterName, str);
      }
    }

    public static void EnumIsDefined<TEnum>(TEnum value, string parameterName)
    {
      Assert.LocalParameterIsNotNullOrWhiteSpace(parameterName, nameof (parameterName));
      Assert.ParamIsNotNull((object) value, nameof (value));
      Type enumType = typeof (TEnum);
      if (!Enum.IsDefined(enumType, (object) value))
      {
        string message = string.Format("{0} has an invalid value '{1}' for type '{2}'.", new object[3]
        {
          (object) parameterName,
          (object) value,
          (object) enumType.Name
        });
        DebugUtilities.Fail(message);
        throw new ArgumentException(message, parameterName);
      }
    }

    public static void ParameterConditionIsTrue(
      bool condition,
      string parameterName,
      string message)
    {
      Assert.LocalParameterIsNotNullOrWhiteSpace(parameterName, nameof (parameterName));
      Assert.LocalParameterIsNotNullOrWhiteSpace(message, nameof (message));
      if (!condition)
      {
        DebugUtilities.Fail(message);
        throw new ArgumentException(message, parameterName);
      }
    }

    public static void IsTrue(bool condition, string message)
    {
      if (condition)
        return;
      Assert.Fail(message);
    }

    public static void Fail(string message)
    {
      Assert.LocalParameterIsNotNullOrWhiteSpace(message, nameof (message));
      DebugUtilities.Fail(message);
      throw new InvalidOperationException(message);
    }

    private static void LocalParameterIsNotNullOrWhiteSpace(string value, string parameterName)
    {
      if (string.IsNullOrWhiteSpace(value))
        throw new ArgumentException(string.Format("{0} should not be null or empty", new object[1]
        {
          (object) parameterName
        }), parameterName);
    }
  }
}
