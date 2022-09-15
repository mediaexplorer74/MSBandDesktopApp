// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Band.BandMms
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Band
{
  public class BandMms : BandMessage
  {
    public BandMms(
      string displayName,
      string body,
      DateTimeOffset timestamp,
      MmsType mmsType,
      IList<string> participants,
      int id)
      : base(displayName, body, timestamp, id)
    {
      this.MmsType = mmsType;
      this.Participants = (IList<string>) new List<string>();
      if (participants != null)
        this.Participants = (IList<string>) new List<string>((IEnumerable<string>) participants);
      else
        this.Participants = (IList<string>) new List<string>();
    }

    public MmsType MmsType { get; }

    public IList<string> Participants { get; }
  }
}
