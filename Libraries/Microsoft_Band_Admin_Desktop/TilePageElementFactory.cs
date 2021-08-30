// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.TilePageElementFactory
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

namespace Microsoft.Band.Admin
{
  public class TilePageElementFactory : ITilePageElementFactory
  {
    public ITileBarcode CreateTileBarcode(
      ushort elementId,
      BarcodeType codeType,
      string barcodeValue)
    {
      return (ITileBarcode) new TileBarcode(elementId, codeType, barcodeValue);
    }

    public ITileIconbox CreateTileIconbox(ushort elementId, ushort iconIndex) => (ITileIconbox) new TileIconbox(elementId, iconIndex);

    public ITileTextbox CreateTileTextbox(ushort elementId, string textboxValue) => (ITileTextbox) new TileTextbox(elementId, textboxValue);

    public ITileWrappableTextbox CreateTileWrappableTextbox(
      ushort elementId,
      string textboxValue)
    {
      return (ITileWrappableTextbox) new TileWrappableTextbox(elementId, textboxValue);
    }
  }
}
