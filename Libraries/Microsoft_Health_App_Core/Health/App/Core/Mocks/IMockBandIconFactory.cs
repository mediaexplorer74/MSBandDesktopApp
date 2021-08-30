// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Mocks.IMockBandIconFactory
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Tiles;

namespace Microsoft.Health.App.Core.Mocks
{
  public interface IMockBandIconFactory
  {
    BandIcon MockBandIcon(int width, int height);
  }
}
