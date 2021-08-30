// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.TileBarcode
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;

namespace Microsoft.Band.Admin
{
  internal sealed class TileBarcode : TilePageElement, ITileBarcode, ITilePageElement
  {
    private string barcodeValue;

    internal TileBarcode(ushort elementId, BarcodeType codeType, string barcodeValue)
      : base(elementId)
    {
      this.CodeType = codeType;
      this.BarcodeValue = barcodeValue;
    }

    public BarcodeType CodeType { get; set; }

    public string BarcodeValue
    {
      get => this.barcodeValue;
      set => this.barcodeValue = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    internal override ushort ElementType
    {
      get
      {
        switch (this.CodeType)
        {
          case BarcodeType.Code39:
            return 3201;
          case BarcodeType.Pdf417:
            return 3202;
          default:
            throw new BandException("CodeType");
        }
      }
    }
  }
}
