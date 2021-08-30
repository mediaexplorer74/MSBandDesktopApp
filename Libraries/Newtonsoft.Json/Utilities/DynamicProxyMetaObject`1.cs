// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.DynamicProxyMetaObject`1
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace Newtonsoft.Json.Utilities
{
  internal sealed class DynamicProxyMetaObject<T> : DynamicMetaObject
  {
    private readonly DynamicProxy<T> _proxy;
    private readonly bool _dontFallbackFirst;
    private static readonly Expression[] NoArgs = new Expression[0];

    internal DynamicProxyMetaObject(
      Expression expression,
      T value,
      DynamicProxy<T> proxy,
      bool dontFallbackFirst)
      : base(expression, BindingRestrictions.Empty, (object) value)
    {
      this._proxy = proxy;
      this._dontFallbackFirst = dontFallbackFirst;
    }

    private T Value => (T) base.Value;

    private bool IsOverridden(string method) => ReflectionUtils.IsMethodOverridden(this._proxy.GetType(), typeof (DynamicProxy<T>), method);

    public override DynamicMetaObject BindGetMember(GetMemberBinder binder) => !this.IsOverridden("TryGetMember") ? base.BindGetMember(binder) : this.CallMethodWithResult("TryGetMember", (DynamicMetaObjectBinder) binder, DynamicProxyMetaObject<T>.NoArgs, (DynamicProxyMetaObject<T>.Fallback) (e => binder.FallbackGetMember((DynamicMetaObject) this, e)));

    public override DynamicMetaObject BindSetMember(
      SetMemberBinder binder,
      DynamicMetaObject value)
    {
      if (!this.IsOverridden("TrySetMember"))
        return base.BindSetMember(binder, value);
      return this.CallMethodReturnLast("TrySetMember", (DynamicMetaObjectBinder) binder, DynamicProxyMetaObject<T>.GetArgs(value), (DynamicProxyMetaObject<T>.Fallback) (e => binder.FallbackSetMember((DynamicMetaObject) this, value, e)));
    }

    public override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder) => !this.IsOverridden("TryDeleteMember") ? base.BindDeleteMember(binder) : this.CallMethodNoResult("TryDeleteMember", (DynamicMetaObjectBinder) binder, DynamicProxyMetaObject<T>.NoArgs, (DynamicProxyMetaObject<T>.Fallback) (e => binder.FallbackDeleteMember((DynamicMetaObject) this, e)));

    public override DynamicMetaObject BindConvert(ConvertBinder binder) => !this.IsOverridden("TryConvert") ? base.BindConvert(binder) : this.CallMethodWithResult("TryConvert", (DynamicMetaObjectBinder) binder, DynamicProxyMetaObject<T>.NoArgs, (DynamicProxyMetaObject<T>.Fallback) (e => binder.FallbackConvert((DynamicMetaObject) this, e)));

    public override DynamicMetaObject BindInvokeMember(
      InvokeMemberBinder binder,
      DynamicMetaObject[] args)
    {
      if (!this.IsOverridden("TryInvokeMember"))
        return base.BindInvokeMember(binder, args);
      DynamicProxyMetaObject<T>.Fallback fallback = (DynamicProxyMetaObject<T>.Fallback) (e => binder.FallbackInvokeMember((DynamicMetaObject) this, args, e));
      DynamicMetaObject errorSuggestion = this.BuildCallMethodWithResult("TryInvokeMember", (DynamicMetaObjectBinder) binder, DynamicProxyMetaObject<T>.GetArgArray(args), this.BuildCallMethodWithResult("TryGetMember", (DynamicMetaObjectBinder) new DynamicProxyMetaObject<T>.GetBinderAdapter(binder), DynamicProxyMetaObject<T>.NoArgs, fallback((DynamicMetaObject) null), (DynamicProxyMetaObject<T>.Fallback) (e => binder.FallbackInvoke(e, args, (DynamicMetaObject) null))), (DynamicProxyMetaObject<T>.Fallback) null);
      return !this._dontFallbackFirst ? fallback(errorSuggestion) : errorSuggestion;
    }

    public override DynamicMetaObject BindCreateInstance(
      CreateInstanceBinder binder,
      DynamicMetaObject[] args)
    {
      return !this.IsOverridden("TryCreateInstance") ? base.BindCreateInstance(binder, args) : this.CallMethodWithResult("TryCreateInstance", (DynamicMetaObjectBinder) binder, DynamicProxyMetaObject<T>.GetArgArray(args), (DynamicProxyMetaObject<T>.Fallback) (e => binder.FallbackCreateInstance((DynamicMetaObject) this, args, e)));
    }

    public override DynamicMetaObject BindInvoke(
      InvokeBinder binder,
      DynamicMetaObject[] args)
    {
      return !this.IsOverridden("TryInvoke") ? base.BindInvoke(binder, args) : this.CallMethodWithResult("TryInvoke", (DynamicMetaObjectBinder) binder, DynamicProxyMetaObject<T>.GetArgArray(args), (DynamicProxyMetaObject<T>.Fallback) (e => binder.FallbackInvoke((DynamicMetaObject) this, args, e)));
    }

    public override DynamicMetaObject BindBinaryOperation(
      BinaryOperationBinder binder,
      DynamicMetaObject arg)
    {
      if (!this.IsOverridden("TryBinaryOperation"))
        return base.BindBinaryOperation(binder, arg);
      return this.CallMethodWithResult("TryBinaryOperation", (DynamicMetaObjectBinder) binder, DynamicProxyMetaObject<T>.GetArgs(arg), (DynamicProxyMetaObject<T>.Fallback) (e => binder.FallbackBinaryOperation((DynamicMetaObject) this, arg, e)));
    }

    public override DynamicMetaObject BindUnaryOperation(
      UnaryOperationBinder binder)
    {
      return !this.IsOverridden("TryUnaryOperation") ? base.BindUnaryOperation(binder) : this.CallMethodWithResult("TryUnaryOperation", (DynamicMetaObjectBinder) binder, DynamicProxyMetaObject<T>.NoArgs, (DynamicProxyMetaObject<T>.Fallback) (e => binder.FallbackUnaryOperation((DynamicMetaObject) this, e)));
    }

    public override DynamicMetaObject BindGetIndex(
      GetIndexBinder binder,
      DynamicMetaObject[] indexes)
    {
      return !this.IsOverridden("TryGetIndex") ? base.BindGetIndex(binder, indexes) : this.CallMethodWithResult("TryGetIndex", (DynamicMetaObjectBinder) binder, DynamicProxyMetaObject<T>.GetArgArray(indexes), (DynamicProxyMetaObject<T>.Fallback) (e => binder.FallbackGetIndex((DynamicMetaObject) this, indexes, e)));
    }

    public override DynamicMetaObject BindSetIndex(
      SetIndexBinder binder,
      DynamicMetaObject[] indexes,
      DynamicMetaObject value)
    {
      return !this.IsOverridden("TrySetIndex") ? base.BindSetIndex(binder, indexes, value) : this.CallMethodReturnLast("TrySetIndex", (DynamicMetaObjectBinder) binder, DynamicProxyMetaObject<T>.GetArgArray(indexes, value), (DynamicProxyMetaObject<T>.Fallback) (e => binder.FallbackSetIndex((DynamicMetaObject) this, indexes, value, e)));
    }

    public override DynamicMetaObject BindDeleteIndex(
      DeleteIndexBinder binder,
      DynamicMetaObject[] indexes)
    {
      return !this.IsOverridden("TryDeleteIndex") ? base.BindDeleteIndex(binder, indexes) : this.CallMethodNoResult("TryDeleteIndex", (DynamicMetaObjectBinder) binder, DynamicProxyMetaObject<T>.GetArgArray(indexes), (DynamicProxyMetaObject<T>.Fallback) (e => binder.FallbackDeleteIndex((DynamicMetaObject) this, indexes, e)));
    }

    private static Expression[] GetArgs(params DynamicMetaObject[] args) => (Expression[]) ((IEnumerable<DynamicMetaObject>) args).Select<DynamicMetaObject, UnaryExpression>((Func<DynamicMetaObject, UnaryExpression>) (arg => Expression.Convert(arg.Expression, typeof (object)))).ToArray<UnaryExpression>();

    private static Expression[] GetArgArray(DynamicMetaObject[] args) => (Expression[]) new NewArrayExpression[1]
    {
      Expression.NewArrayInit(typeof (object), DynamicProxyMetaObject<T>.GetArgs(args))
    };

    private static Expression[] GetArgArray(
      DynamicMetaObject[] args,
      DynamicMetaObject value)
    {
      return new Expression[2]
      {
        (Expression) Expression.NewArrayInit(typeof (object), DynamicProxyMetaObject<T>.GetArgs(args)),
        (Expression) Expression.Convert(value.Expression, typeof (object))
      };
    }

    private static ConstantExpression Constant(DynamicMetaObjectBinder binder)
    {
      Type type = binder.GetType();
      while (!type.IsVisible())
        type = type.BaseType();
      return Expression.Constant((object) binder, type);
    }

    private DynamicMetaObject CallMethodWithResult(
      string methodName,
      DynamicMetaObjectBinder binder,
      Expression[] args,
      DynamicProxyMetaObject<T>.Fallback fallback,
      DynamicProxyMetaObject<T>.Fallback fallbackInvoke = null)
    {
      DynamicMetaObject fallbackResult = fallback((DynamicMetaObject) null);
      DynamicMetaObject errorSuggestion = this.BuildCallMethodWithResult(methodName, binder, args, fallbackResult, fallbackInvoke);
      return !this._dontFallbackFirst ? fallback(errorSuggestion) : errorSuggestion;
    }

    private DynamicMetaObject BuildCallMethodWithResult(
      string methodName,
      DynamicMetaObjectBinder binder,
      Expression[] args,
      DynamicMetaObject fallbackResult,
      DynamicProxyMetaObject<T>.Fallback fallbackInvoke)
    {
      ParameterExpression parameterExpression = Expression.Parameter(typeof (object), (string) null);
      IList<Expression> initial = (IList<Expression>) new List<Expression>();
      initial.Add((Expression) Expression.Convert(this.Expression, typeof (T)));
      initial.Add((Expression) DynamicProxyMetaObject<T>.Constant(binder));
      initial.AddRange<Expression>((IEnumerable<Expression>) args);
      initial.Add((Expression) parameterExpression);
      DynamicMetaObject errorSuggestion = new DynamicMetaObject((Expression) parameterExpression, BindingRestrictions.Empty);
      if (binder.ReturnType != typeof (object))
        errorSuggestion = new DynamicMetaObject((Expression) Expression.Convert(errorSuggestion.Expression, binder.ReturnType), errorSuggestion.Restrictions);
      if (fallbackInvoke != null)
        errorSuggestion = fallbackInvoke(errorSuggestion);
      return new DynamicMetaObject((Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[1]
      {
        parameterExpression
      }, (Expression) Expression.Condition((Expression) Expression.Call((Expression) Expression.Constant((object) this._proxy), typeof (DynamicProxy<T>).GetMethod(methodName), (IEnumerable<Expression>) initial), errorSuggestion.Expression, fallbackResult.Expression, binder.ReturnType)), this.GetRestrictions().Merge(errorSuggestion.Restrictions).Merge(fallbackResult.Restrictions));
    }

    private DynamicMetaObject CallMethodReturnLast(
      string methodName,
      DynamicMetaObjectBinder binder,
      Expression[] args,
      DynamicProxyMetaObject<T>.Fallback fallback)
    {
      DynamicMetaObject dynamicMetaObject = fallback((DynamicMetaObject) null);
      ParameterExpression parameterExpression = Expression.Parameter(typeof (object), (string) null);
      IList<Expression> initial = (IList<Expression>) new List<Expression>();
      initial.Add((Expression) Expression.Convert(this.Expression, typeof (T)));
      initial.Add((Expression) DynamicProxyMetaObject<T>.Constant(binder));
      initial.AddRange<Expression>((IEnumerable<Expression>) args);
      initial[args.Length + 1] = (Expression) Expression.Assign((Expression) parameterExpression, initial[args.Length + 1]);
      DynamicMetaObject errorSuggestion = new DynamicMetaObject((Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[1]
      {
        parameterExpression
      }, (Expression) Expression.Condition((Expression) Expression.Call((Expression) Expression.Constant((object) this._proxy), typeof (DynamicProxy<T>).GetMethod(methodName), (IEnumerable<Expression>) initial), (Expression) parameterExpression, dynamicMetaObject.Expression, typeof (object))), this.GetRestrictions().Merge(dynamicMetaObject.Restrictions));
      return !this._dontFallbackFirst ? fallback(errorSuggestion) : errorSuggestion;
    }

    private DynamicMetaObject CallMethodNoResult(
      string methodName,
      DynamicMetaObjectBinder binder,
      Expression[] args,
      DynamicProxyMetaObject<T>.Fallback fallback)
    {
      DynamicMetaObject dynamicMetaObject = fallback((DynamicMetaObject) null);
      IList<Expression> initial = (IList<Expression>) new List<Expression>();
      initial.Add((Expression) Expression.Convert(this.Expression, typeof (T)));
      initial.Add((Expression) DynamicProxyMetaObject<T>.Constant(binder));
      initial.AddRange<Expression>((IEnumerable<Expression>) args);
      DynamicMetaObject errorSuggestion = new DynamicMetaObject((Expression) Expression.Condition((Expression) Expression.Call((Expression) Expression.Constant((object) this._proxy), typeof (DynamicProxy<T>).GetMethod(methodName), (IEnumerable<Expression>) initial), (Expression) Expression.Empty(), dynamicMetaObject.Expression, typeof (void)), this.GetRestrictions().Merge(dynamicMetaObject.Restrictions));
      return !this._dontFallbackFirst ? fallback(errorSuggestion) : errorSuggestion;
    }

    private BindingRestrictions GetRestrictions() => (object) this.Value != null || !this.HasValue ? BindingRestrictions.GetTypeRestriction(this.Expression, this.LimitType) : BindingRestrictions.GetInstanceRestriction(this.Expression, (object) null);

    public override IEnumerable<string> GetDynamicMemberNames() => this._proxy.GetDynamicMemberNames(this.Value);

    private delegate DynamicMetaObject Fallback(DynamicMetaObject errorSuggestion);

    private sealed class GetBinderAdapter : GetMemberBinder
    {
      internal GetBinderAdapter(InvokeMemberBinder binder)
        : base(binder.Name, binder.IgnoreCase)
      {
      }

      public override DynamicMetaObject FallbackGetMember(
        DynamicMetaObject target,
        DynamicMetaObject errorSuggestion)
      {
        throw new NotSupportedException();
      }
    }
  }
}
