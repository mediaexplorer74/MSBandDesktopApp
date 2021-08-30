// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Logging.Framework.ILog
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Services.Logging.Framework
{
  public interface ILog
  {
    bool IsDebugEnabled { get; }

    bool IsErrorEnabled { get; }

    bool IsFatalEnabled { get; }

    bool IsInfoEnabled { get; }

    bool IsWarnEnabled { get; }

    void Debug(object message);

    void Debug(object message, Exception exception);

    void DebugFormat(IFormatProvider provider, string format, params object[] args);

    void DebugFormat(string format, params object[] args);

    void Error(object message);

    void Error(object message, Exception exception);

    void ErrorFormat(IFormatProvider provider, string format, params object[] args);

    void ErrorFormat(string format, params object[] args);

    void Fatal(object message);

    void Fatal(object message, Exception exception);

    void FatalFormat(IFormatProvider provider, string format, params object[] args);

    void FatalFormat(string format, params object[] args);

    void Info(object message);

    void Info(object message, Exception exception);

    void InfoFormat(IFormatProvider provider, string format, params object[] args);

    void InfoFormat(string format, params object[] args);

    void Warn(object message);

    void Warn(object message, Exception exception);

    void WarnFormat(IFormatProvider provider, string format, params object[] args);

    void WarnFormat(string format, params object[] args);
  }
}
