// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Mocks.MockBandInfo
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using System;

namespace Microsoft.Health.App.Core.Mocks
{
  public class MockBandInfo : IBandInfo
  {
    public string Name { get; set; }

    public Guid Id { get; set; }

    public BandConnectionType ConnectionType { get; set; }

    public MockBandInfo(string name, Guid id, BandConnectionType connectionType)
    {
      this.Name = name;
      this.Id = id;
      this.ConnectionType = BandConnectionType.Bluetooth;
    }
  }
}
