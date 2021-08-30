// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.ExceptionExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class ExceptionExtensions
  {
    public static T Find<T>(this Exception exception) where T : Exception
    {
      switch (exception)
      {
        case null:
          return default (T);
        case T obj:
          return obj;
        case AggregateException aggregateException:
          return ExceptionExtensions.FindRecursive<T>(aggregateException);
        default:
          return default (T);
      }
    }

    public static bool Contains<T>(this Exception exception) where T : Exception => (object) exception.Find<T>() != null;

    private static T FindRecursive<T>(AggregateException aggregateException) where T : Exception
    {
      if (aggregateException.InnerExceptions == null)
        return default (T);
      foreach (Exception innerException in aggregateException.InnerExceptions)
      {
        if (innerException is T obj2)
          return obj2;
        if (innerException is AggregateException aggregateException3)
        {
          T recursive = ExceptionExtensions.FindRecursive<T>(aggregateException3);
          if ((object) recursive != null)
            return recursive;
        }
      }
      return default (T);
    }

    public static bool CatchWhen(this Exception exception, Func<bool> func) => func();
  }
}
