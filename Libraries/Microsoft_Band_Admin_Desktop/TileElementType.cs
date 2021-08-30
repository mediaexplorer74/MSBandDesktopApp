// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.TileElementType
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

namespace Microsoft.Band.Admin
{
  internal enum TileElementType : ushort
  {
    PageHeader = 1,
    Flowlist = 1001, // 0x03E9
    ScrollFlowlist = 1002, // 0x03EA
    FilledQuad = 1101, // 0x044D
    Text = 3001, // 0x0BB9
    WrappableText = 3002, // 0x0BBA
    Icon = 3101, // 0x0C1D
    BarcodeCode39 = 3201, // 0x0C81
    BarcodePDF417 = 3202, // 0x0C82
    Button = 3301, // 0x0CE5
    Invalid = 65535, // 0xFFFF
  }
}
