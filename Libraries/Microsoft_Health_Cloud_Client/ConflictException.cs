// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.ConflictException
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Exceptions;
using System;

namespace Microsoft.Health.Cloud.Client
{
  public class ConflictException : HealthCloudServerException
  {
    public ConflictException()
    {
    }

    public ConflictException(string message)
      : base(message)
    {
    }

    public ConflictException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
