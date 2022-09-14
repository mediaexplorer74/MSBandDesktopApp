// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Band.WeatherDay
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Band
{
  public class WeatherDay
  {
    public DateTimeOffset Date { get; set; }

    public short High { get; set; }

    public short Low { get; set; }

    public uint IconId { get; set; }

    public string Caption { get; set; }
  }
}
