// Decompiled with JetBrains decompiler
// Type: NodaTime.Utility.Preconditions
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using System;

namespace NodaTime.Utility
{
  internal static class Preconditions
  {
    [ContractAnnotation("argument:null => halt")]
    internal static T CheckNotNull<T>(T argument, [InvokerParameterName] string paramName) where T : class => (object) argument != null ? argument : throw new ArgumentNullException(paramName);

    internal static void CheckArgumentRange(
      [InvokerParameterName] string paramName,
      long value,
      long minInclusive,
      long maxInclusive)
    {
      if (value < minInclusive || value > maxInclusive)
        throw new ArgumentOutOfRangeException(paramName, "Value should be in range [" + (object) minInclusive + "-" + (object) maxInclusive + "]");
    }

    internal static void CheckArgumentRange(
      [InvokerParameterName] string paramName,
      int value,
      int minInclusive,
      int maxInclusive)
    {
      if (value < minInclusive || value > maxInclusive)
        throw new ArgumentOutOfRangeException(paramName, "Value should be in range [" + (object) minInclusive + "-" + (object) maxInclusive + "]");
    }

    [ContractAnnotation("expression:false => halt")]
    internal static void CheckArgument(bool expression, [InvokerParameterName] string parameter, string message)
    {
      if (!expression)
        throw new ArgumentException(message, parameter);
    }

    [ContractAnnotation("expression:false => halt")]
    [StringFormatMethod("messageFormat")]
    internal static void CheckArgument<T>(
      bool expression,
      [InvokerParameterName] string parameter,
      string messageFormat,
      T messageArg)
    {
      if (!expression)
        throw new ArgumentException(string.Format(messageFormat, new object[1]
        {
          (object) messageArg
        }), parameter);
    }

    [ContractAnnotation("expression:false => halt")]
    [StringFormatMethod("messageFormat")]
    internal static void CheckArgument<T1, T2>(
      bool expression,
      string parameter,
      string messageFormat,
      T1 messageArg1,
      T2 messageArg2)
    {
      if (!expression)
        throw new ArgumentException(string.Format(messageFormat, new object[2]
        {
          (object) messageArg1,
          (object) messageArg2
        }), parameter);
    }

    [ContractAnnotation("expression:false => halt")]
    [StringFormatMethod("messageFormat")]
    internal static void CheckArgument(
      bool expression,
      string parameter,
      string messageFormat,
      params object[] messageArgs)
    {
      if (!expression)
        throw new ArgumentException(string.Format(messageFormat, messageArgs), parameter);
    }
  }
}
