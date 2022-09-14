// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.TileWrappableTextbox
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;

namespace Microsoft.Band.Admin
{
  internal sealed class TileWrappableTextbox : 
    TilePageElement,
    ITileWrappableTextbox,
    ITilePageElement
  {
    private string textboxValue;

    internal TileWrappableTextbox(ushort elementId, string textboxValue)
      : base(elementId)
    {
      this.TextboxValue = textboxValue;
    }

    public string TextboxValue
    {
      get => this.textboxValue;
      set => this.textboxValue = value != null ? value : throw new ArgumentNullException(nameof (value));
    }

    internal override ushort ElementType => 3002;
  }
}
