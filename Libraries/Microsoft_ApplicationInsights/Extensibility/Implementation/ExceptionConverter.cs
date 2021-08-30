// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.ExceptionConverter
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
  internal static class ExceptionConverter
  {
    public const int MaxParsedStackLength = 32768;

    internal static ExceptionDetails ConvertToExceptionDetails(
      Exception exception,
      ExceptionDetails parentExceptionDetails)
    {
      ExceptionDetails withoutStackInfo = ExceptionDetails.CreateWithoutStackInfo(exception, parentExceptionDetails);
      Tuple<List<Microsoft.ApplicationInsights.Extensibility.Implementation.External.StackFrame>, bool> tuple = ExceptionConverter.SanitizeStackFrame<System.Diagnostics.StackFrame, Microsoft.ApplicationInsights.Extensibility.Implementation.External.StackFrame>((IList<System.Diagnostics.StackFrame>) new StackTrace(exception, true).GetFrames(), new Func<System.Diagnostics.StackFrame, int, Microsoft.ApplicationInsights.Extensibility.Implementation.External.StackFrame>(ExceptionConverter.GetStackFrame), new Func<Microsoft.ApplicationInsights.Extensibility.Implementation.External.StackFrame, int>(ExceptionConverter.GetStackFrameLength));
      withoutStackInfo.parsedStack = (IList<Microsoft.ApplicationInsights.Extensibility.Implementation.External.StackFrame>) tuple.Item1;
      withoutStackInfo.hasFullStack = tuple.Item2;
      return withoutStackInfo;
    }

    private static Tuple<List<TOutput>, bool> SanitizeStackFrame<TInput, TOutput>(
      IList<TInput> inputList,
      Func<TInput, int, TOutput> converter,
      Func<TOutput, int> lengthGetter)
    {
      List<TOutput> outputList = new List<TOutput>();
      bool flag = true;
      if (inputList != null && inputList.Count > 0)
      {
        int num = 0;
        for (int index1 = 0; index1 < inputList.Count; ++index1)
        {
          int index2 = index1 % 2 == 0 ? inputList.Count - 1 - index1 / 2 : index1 / 2;
          TOutput output = converter(inputList[index2], index2);
          num += lengthGetter(output);
          if (num > 32768)
          {
            flag = false;
            break;
          }
          outputList.Insert(outputList.Count / 2, output);
        }
      }
      return new Tuple<List<TOutput>, bool>(outputList, flag);
    }

    private static Microsoft.ApplicationInsights.Extensibility.Implementation.External.StackFrame GetStackFrame(
      System.Diagnostics.StackFrame stackFrame,
      int frameId)
    {
      Microsoft.ApplicationInsights.Extensibility.Implementation.External.StackFrame stackFrame1 = new Microsoft.ApplicationInsights.Extensibility.Implementation.External.StackFrame()
      {
        level = frameId
      };
      MethodBase method = stackFrame.GetMethod();
      string str = !(method.DeclaringType != (Type) null) ? method.Name : method.DeclaringType.FullName + "." + method.Name;
      stackFrame1.method = str;
      stackFrame1.assembly = method.Module.Assembly.FullName;
      stackFrame1.fileName = stackFrame.GetFileName();
      int fileLineNumber = stackFrame.GetFileLineNumber();
      if (fileLineNumber != 0)
        stackFrame1.line = fileLineNumber;
      return stackFrame1;
    }

    private static int GetStackFrameLength(Microsoft.ApplicationInsights.Extensibility.Implementation.External.StackFrame stackFrame) => (stackFrame.method == null ? 0 : stackFrame.method.Length) + (stackFrame.assembly == null ? 0 : stackFrame.assembly.Length) + (stackFrame.fileName == null ? 0 : stackFrame.fileName.Length);
  }
}
