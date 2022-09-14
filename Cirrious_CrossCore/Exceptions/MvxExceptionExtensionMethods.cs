// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Exceptions.MvxExceptionExtensionMethods
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;

namespace Cirrious.CrossCore.Exceptions
{
  public static class MvxExceptionExtensionMethods
  {
    public static string ToLongString(this Exception exception)
    {
      if (exception == null)
        return "null exception";
      if (exception.InnerException != null)
      {
        string longString = exception.InnerException.ToLongString();
        return string.Format("{0}: {1}\n\t{2}\nInnerException was {3}", (object) ((object) exception).GetType().Name, (object) (exception.Message ?? "-"), (object) exception.StackTrace, (object) longString);
      }
      return string.Format("{0}: {1}\n\t{2}", new object[3]
      {
        (object) ((object) exception).GetType().Name,
        (object) (exception.Message ?? "-"),
        (object) exception.StackTrace
      });
    }

    public static Exception MvxWrap(this Exception exception) => exception is MvxException ? exception : exception.MvxWrap(exception.Message);

    public static Exception MvxWrap(this Exception exception, string message) => (Exception) new MvxException(exception, message, new object[0]);

    public static Exception MvxWrap(
      this Exception exception,
      string messageFormat,
      params object[] formatArguments)
    {
      return (Exception) new MvxException(exception, messageFormat, formatArguments);
    }
  }
}
