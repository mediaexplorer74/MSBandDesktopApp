// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.SplitLocation
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;

namespace Microsoft.Health.App.Core.Models
{
  public class SplitLocation
  {
    public SplitLocation(Location location, int splitNumber)
    {
      this.SplitNumber = splitNumber;
      this.Location = location;
    }

    public int SplitNumber { get; private set; }

    public Location Location { get; private set; }
  }
}
