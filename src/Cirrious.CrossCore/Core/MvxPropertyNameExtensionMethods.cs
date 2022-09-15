// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Core.MvxPropertyNameExtensionMethods
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Cirrious.CrossCore.Core
{
  public static class MvxPropertyNameExtensionMethods
  {
    private const string WrongExpressionMessage = "Wrong expression\nshould be called with expression like\n() => PropertyName";
    private const string WrongUnaryExpressionMessage = "Wrong unary expression\nshould be called with expression like\n() => PropertyName";

    public static string GetPropertyNameFromExpression<T>(
      this object target,
      Expression<Func<T>> expression)
    {
      PropertyInfo member = ((expression != null ? MvxPropertyNameExtensionMethods.FindMemberExpression<T>(expression) : throw new ArgumentNullException(nameof (expression))) ?? throw new ArgumentException("Wrong expression\nshould be called with expression like\n() => PropertyName", nameof (expression))).Member as PropertyInfo;
      if ((object) member == null)
        throw new ArgumentException("Wrong expression\nshould be called with expression like\n() => PropertyName", nameof (expression));
      if ((object) member.DeclaringType == null)
        throw new ArgumentException("Wrong expression\nshould be called with expression like\n() => PropertyName", nameof (expression));
      if (target != null && !ReflectionExtensions.IsAssignableFrom(member.DeclaringType, target.GetType()))
        throw new ArgumentException("Wrong expression\nshould be called with expression like\n() => PropertyName", nameof (expression));
      return !ReflectionExtensions.GetGetMethod(member, true).IsStatic ? member.Name : throw new ArgumentException("Wrong expression\nshould be called with expression like\n() => PropertyName", nameof (expression));
    }

    private static MemberExpression FindMemberExpression<T>(
      Expression<Func<T>> expression)
    {
      if (!(expression.Body is UnaryExpression))
        return expression.Body as MemberExpression;
      if (!(((UnaryExpression) expression.Body).Operand is MemberExpression operand))
        throw new ArgumentException("Wrong unary expression\nshould be called with expression like\n() => PropertyName", nameof (expression));
      return operand;
    }
  }
}
