// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.LogProcessing.LogProcessingUpdatedEventArgs
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Band.Admin.LogProcessing
{
  public sealed class LogProcessingUpdatedEventArgs : EventArgs
  {
    public List<LogProcessingStatus> CompletedFiles;
    public List<LogProcessingStatus> ProcessingFiles;
    public List<LogProcessingStatus> NotRecognizedFiles;

    public LogProcessingUpdatedEventArgs(
      IEnumerable<LogProcessingStatus> Completed,
      IEnumerable<LogProcessingStatus> Processing,
      IEnumerable<LogProcessingStatus> NotRecognized)
    {
      this.CompletedFiles = Completed == null ? new List<LogProcessingStatus>() : new List<LogProcessingStatus>(Completed);
      this.ProcessingFiles = Processing == null ? new List<LogProcessingStatus>() : new List<LogProcessingStatus>(Processing);
      this.NotRecognizedFiles = NotRecognized == null ? new List<LogProcessingStatus>() : new List<LogProcessingStatus>(NotRecognized);
    }
  }
}
