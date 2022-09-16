// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.ErrorInfo
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;

namespace DesktopSyncApp
{
  public class ErrorInfo
  {
    public string Function { get; private set; }

    public string Description { get; private set; }

    public Exception Exception { get; private set; }

    public ErrorInfo(string function, string description, Exception exception)
    {
      this.Function = function;
      this.Description = description;
      this.Exception = exception;
    }
  }
}
