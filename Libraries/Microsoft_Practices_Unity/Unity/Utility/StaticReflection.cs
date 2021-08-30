// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.Utility.StaticReflection
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Practices.Unity.Utility
{
  public static class StaticReflection
  {
    public static MethodInfo GetMethodInfo(Expression<Action> expression) => StaticReflection.GetMethodInfo((LambdaExpression) expression);

    public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression) => StaticReflection.GetMethodInfo((LambdaExpression) expression);

    private static MethodInfo GetMethodInfo(LambdaExpression lambda)
    {
      StaticReflection.GuardProperExpressionForm(lambda.Body);
      return ((MethodCallExpression) lambda.Body).Method;
    }

    public static MethodInfo GetPropertyGetMethodInfo<T, TProperty>(
      Expression<Func<T, TProperty>> expression)
    {
      return StaticReflection.GetPropertyInfo<T, TProperty>((LambdaExpression) expression).GetMethod ?? throw new InvalidOperationException("Invalid expression form passed");
    }

    public static MethodInfo GetPropertySetMethodInfo<T, TProperty>(
      Expression<Func<T, TProperty>> expression)
    {
      return StaticReflection.GetPropertyInfo<T, TProperty>((LambdaExpression) expression).SetMethod ?? throw new InvalidOperationException("Invalid expression form passed");
    }

    private static PropertyInfo GetPropertyInfo<T, TProperty>(LambdaExpression lambda)
    {
      if (!(lambda.Body is MemberExpression body))
        throw new InvalidOperationException("Invalid expression form passed");
      return body.Member as PropertyInfo ?? throw new InvalidOperationException("Invalid expression form passed");
    }

    public static MemberInfo GetMemberInfo<T, TProperty>(
      Expression<Func<T, TProperty>> expression)
    {
      Guard.ArgumentNotNull((object) expression, nameof (expression));
      if (!(expression.Body is MemberExpression body))
        throw new InvalidOperationException("invalid expression form passed");
      return body.Member ?? throw new InvalidOperationException("Invalid expression form passed");
    }

    public static ConstructorInfo GetConstructorInfo<T>(
      Expression<Func<T>> expression)
    {
      Guard.ArgumentNotNull((object) expression, nameof (expression));
      if (!(expression.Body is NewExpression body))
        throw new InvalidOperationException("Invalid expression form passed");
      return body.Constructor;
    }

    private static void GuardProperExpressionForm(Expression expression)
    {
      if (expression.NodeType != ExpressionType.Call)
        throw new InvalidOperationException("Invalid expression form passed");
    }
  }
}
