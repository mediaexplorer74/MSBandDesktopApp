// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Exceptions.MvxIoCResolveException
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;

namespace Cirrious.CrossCore.Exceptions
{
  public class MvxIoCResolveException : MvxException
  {
    public MvxIoCResolveException()
    {
    }

    public MvxIoCResolveException(string message)
      : base(message)
    {
    }

    public MvxIoCResolveException(string messageFormat, params object[] messageFormatArguments)
      : base(messageFormat, messageFormatArguments)
    {
    }

    public MvxIoCResolveException(
      Exception innerException,
      string messageFormat,
      params object[] formatArguments)
      : base(innerException, messageFormat, formatArguments)
    {
    }
  }
}
