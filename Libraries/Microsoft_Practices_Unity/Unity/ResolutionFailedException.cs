// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.ResolutionFailedException
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity.Properties;
using Microsoft.Practices.Unity.Utility;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Microsoft.Practices.Unity
{
  public class ResolutionFailedException : Exception
  {
    private string typeRequested;
    private string nameRequested;

    public ResolutionFailedException(
      Type typeRequested,
      string nameRequested,
      Exception innerException,
      IBuilderContext context)
      : base(ResolutionFailedException.CreateMessage(typeRequested, nameRequested, innerException, context), innerException)
    {
      Guard.ArgumentNotNull((object) typeRequested, nameof (typeRequested));
      if ((object) typeRequested != null)
        this.typeRequested = typeRequested.GetTypeInfo().Name;
      this.nameRequested = nameRequested;
    }

    public string TypeRequested => this.typeRequested;

    public string NameRequested => this.nameRequested;

    private static string CreateMessage(
      Type typeRequested,
      string nameRequested,
      Exception innerException,
      IBuilderContext context)
    {
      Guard.ArgumentNotNull((object) typeRequested, nameof (typeRequested));
      Guard.ArgumentNotNull((object) context, nameof (context));
      StringBuilder builder = new StringBuilder();
      builder.AppendFormat((IFormatProvider) CultureInfo.CurrentCulture, Resources.ResolutionFailed, (object) typeRequested, (object) ResolutionFailedException.FormatName(nameRequested), (object) ResolutionFailedException.ExceptionReason(context), innerException != null ? (object) ((object) innerException).GetType().GetTypeInfo().Name : (object) nameof (ResolutionFailedException), (object) innerException?.Message);
      builder.AppendLine();
      ResolutionFailedException.AddContextDetails(builder, context, 1);
      return builder.ToString();
    }

    private static void AddContextDetails(
      StringBuilder builder,
      IBuilderContext context,
      int depth)
    {
      if (context == null)
        return;
      string str = new string(' ', depth * 2);
      NamedTypeBuildKey buildKey = context.BuildKey;
      NamedTypeBuildKey originalBuildKey = context.OriginalBuildKey;
      builder.Append(str);
      if (buildKey == originalBuildKey)
        builder.AppendFormat((IFormatProvider) CultureInfo.CurrentCulture, Resources.ResolutionTraceDetail, new object[2]
        {
          (object) buildKey.Type,
          (object) ResolutionFailedException.FormatName(buildKey.Name)
        });
      else
        builder.AppendFormat((IFormatProvider) CultureInfo.CurrentCulture, Resources.ResolutionWithMappingTraceDetail, (object) buildKey.Type, (object) ResolutionFailedException.FormatName(buildKey.Name), (object) originalBuildKey.Type, (object) ResolutionFailedException.FormatName(originalBuildKey.Name));
      builder.AppendLine();
      if (context.CurrentOperation != null)
      {
        builder.Append(str);
        builder.AppendFormat((IFormatProvider) CultureInfo.CurrentCulture, context.CurrentOperation.ToString());
        builder.AppendLine();
      }
      ResolutionFailedException.AddContextDetails(builder, context.ChildContext, depth + 1);
    }

    private static string FormatName(string name) => !string.IsNullOrEmpty(name) ? name : "(none)";

    private static string ExceptionReason(IBuilderContext context)
    {
      IBuilderContext builderContext = context;
      while (builderContext.ChildContext != null)
        builderContext = builderContext.ChildContext;
      return builderContext.CurrentOperation != null ? builderContext.CurrentOperation.ToString() : Resources.NoOperationExceptionReason;
    }
  }
}
