// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.DataContracts.InnerExceptionCountExceededException
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;
using System.Runtime.Serialization;

namespace Microsoft.ApplicationInsights.DataContracts
{
  [Serializable]
  internal class InnerExceptionCountExceededException : Exception
  {
    public InnerExceptionCountExceededException()
    {
    }

    public InnerExceptionCountExceededException(string message)
      : base(message)
    {
    }

    public InnerExceptionCountExceededException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected InnerExceptionCountExceededException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
