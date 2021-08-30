// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.DynamicBuildPlanGenerationContext
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class DynamicBuildPlanGenerationContext
  {
    private readonly Type typeToBuild;
    private readonly ParameterExpression contextParameter;
    private readonly Queue<Expression> buildPlanExpressions;
    private static readonly MethodInfo ResolveDependencyMethod = StaticReflection.GetMethodInfo<IDependencyResolverPolicy>((Expression<Action<IDependencyResolverPolicy>>) (r => r.Resolve(default (IBuilderContext))));
    private static readonly MethodInfo GetResolverMethod = StaticReflection.GetMethodInfo((Expression<Action>) (() => DynamicBuildPlanGenerationContext.GetResolver(default (IBuilderContext), default (Type), default (IDependencyResolverPolicy))));
    private static readonly MemberInfo GetBuildContextExistingObjectProperty = StaticReflection.GetMemberInfo<IBuilderContext, object>((Expression<Func<IBuilderContext, object>>) (c => c.Existing));

    public DynamicBuildPlanGenerationContext(Type typeToBuild)
    {
      this.typeToBuild = typeToBuild;
      this.contextParameter = Expression.Parameter(typeof (IBuilderContext), "context");
      this.buildPlanExpressions = new Queue<Expression>();
    }

    public Type TypeToBuild => this.typeToBuild;

    public ParameterExpression ContextParameter => this.contextParameter;

    public void AddToBuildPlan(Expression expression) => this.buildPlanExpressions.Enqueue(expression);

    public Expression CreateParameterExpression(
      IDependencyResolverPolicy resolver,
      Type parameterType,
      Expression setOperationExpression)
    {
      ParameterExpression parameterExpression1 = Expression.Parameter(typeof (object));
      ParameterExpression parameterExpression2 = Expression.Parameter(parameterType);
      return (Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[2]
      {
        parameterExpression1,
        parameterExpression2
      }, this.SaveCurrentOperationExpression(parameterExpression1), setOperationExpression, (Expression) Expression.Assign((Expression) parameterExpression2, this.GetResolveDependencyExpression(parameterType, resolver)), this.RestoreCurrentOperationExpression(parameterExpression1), (Expression) parameterExpression2);
    }

    internal Expression GetExistingObjectExpression() => (Expression) Expression.MakeMemberAccess((Expression) this.ContextParameter, DynamicBuildPlanGenerationContext.GetBuildContextExistingObjectProperty);

    internal Expression GetClearCurrentOperationExpression() => (Expression) Expression.Assign((Expression) Expression.Property((Expression) this.ContextParameter, typeof (IBuilderContext).GetTypeInfo().GetDeclaredProperty("CurrentOperation")), (Expression) Expression.Constant((object) null));

    internal Expression GetResolveDependencyExpression(
      Type dependencyType,
      IDependencyResolverPolicy resolver)
    {
      return (Expression) Expression.Convert((Expression) Expression.Call((Expression) Expression.Call((Expression) null, DynamicBuildPlanGenerationContext.GetResolverMethod, (Expression) this.ContextParameter, (Expression) Expression.Constant((object) dependencyType, typeof (Type)), (Expression) Expression.Constant((object) resolver, typeof (IDependencyResolverPolicy))), DynamicBuildPlanGenerationContext.ResolveDependencyMethod, (Expression) this.ContextParameter), dependencyType);
    }

    internal DynamicBuildPlanMethod GetBuildMethod()
    {
      Func<IBuilderContext, object> planDelegate = (Func<IBuilderContext, object>) Expression.Lambda((Expression) Expression.Block(this.buildPlanExpressions.Concat<Expression>((IEnumerable<Expression>) new Expression[1]
      {
        this.GetExistingObjectExpression()
      })), this.ContextParameter).Compile();
      return (DynamicBuildPlanMethod) (context =>
      {
        try
        {
          context.Existing = planDelegate(context);
        }
        catch (TargetInvocationException ex)
        {
          throw ex.InnerException;
        }
      });
    }

    private Expression RestoreCurrentOperationExpression(
      ParameterExpression savedOperationExpression)
    {
      return (Expression) Expression.Assign((Expression) Expression.MakeMemberAccess((Expression) this.ContextParameter, (MemberInfo) typeof (IBuilderContext).GetTypeInfo().GetDeclaredProperty("CurrentOperation")), (Expression) savedOperationExpression);
    }

    private Expression SaveCurrentOperationExpression(ParameterExpression saveExpression) => (Expression) Expression.Assign((Expression) saveExpression, (Expression) Expression.MakeMemberAccess((Expression) this.ContextParameter, (MemberInfo) typeof (IBuilderContext).GetTypeInfo().GetDeclaredProperty("CurrentOperation")));

    [Obsolete("Resolvers are no longer stored as policies.")]
    public static IDependencyResolverPolicy GetResolver(
      IBuilderContext context,
      Type dependencyType,
      string resolverKey)
    {
      throw new NotSupportedException("This method is no longer used");
    }

    public static IDependencyResolverPolicy GetResolver(
      IBuilderContext context,
      Type dependencyType,
      IDependencyResolverPolicy resolver)
    {
      Guard.ArgumentNotNull((object) context, nameof (context));
      return context.GetOverriddenResolver(dependencyType) ?? resolver;
    }
  }
}
