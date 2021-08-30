// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.ExceptionDetails
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  [Microsoft.Diagnostics.Tracing.EventData]
  [GeneratedCode("gbc", "3.02")]
  internal class ExceptionDetails
  {
    public int id { get; set; }

    public int outerId { get; set; }

    public string typeName { get; set; }

    public string message { get; set; }

    public bool hasFullStack { get; set; }

    public string stack { get; set; }

    public IList<StackFrame> parsedStack { get; set; }

    public ExceptionDetails()
      : this("AI.ExceptionDetails", nameof (ExceptionDetails))
    {
    }

    protected ExceptionDetails(string fullName, string name)
    {
      this.typeName = string.Empty;
      this.message = string.Empty;
      this.hasFullStack = true;
      this.stack = string.Empty;
      this.parsedStack = (IList<StackFrame>) new List<StackFrame>();
    }

    internal static ExceptionDetails CreateWithoutStackInfo(
      Exception exception,
      ExceptionDetails parentExceptionDetails)
    {
      if (exception == null)
        throw new ArgumentNullException(nameof (exception));
      ExceptionDetails exceptionDetails = new ExceptionDetails()
      {
        id = exception.GetHashCode(),
        typeName = exception.GetType().FullName,
        message = exception.Message
      };
      if (parentExceptionDetails != null)
        exceptionDetails.outerId = parentExceptionDetails.id;
      return exceptionDetails;
    }
  }
}
