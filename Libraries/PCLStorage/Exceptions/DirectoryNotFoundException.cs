// Decompiled with JetBrains decompiler
// Type: PCLStorage.Exceptions.DirectoryNotFoundException
// Assembly: PCLStorage, Version=1.0.2.0, Culture=neutral, PublicKeyToken=286fe515a2c35b64
// MVID: 7C72F032-7D19-49C3-B4FA-67DAADE24971
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\PCLStorage.dll

using System;
using System.IO;

namespace PCLStorage.Exceptions
{
  public class DirectoryNotFoundException : IOException
  {
    public DirectoryNotFoundException(string message)
      : base(message)
    {
    }

    public DirectoryNotFoundException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
