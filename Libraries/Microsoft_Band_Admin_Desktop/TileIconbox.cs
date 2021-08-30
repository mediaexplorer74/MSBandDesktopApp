// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.TileIconbox
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;

namespace Microsoft.Band.Admin
{
  internal sealed class TileIconbox : TilePageElement, ITileIconbox, ITilePageElement
  {
    private ushort iconIndex;

    internal TileIconbox(ushort elementId, ushort iconIndex)
      : base(elementId)
    {
      this.IconIndex = iconIndex;
    }

    public ushort IconIndex
    {
      get => this.iconIndex;
      set => this.iconIndex = value < (ushort) 10 ? value : throw new ArgumentOutOfRangeException(nameof (IconIndex));
    }

    internal override ushort ElementType => 3101;
  }
}
