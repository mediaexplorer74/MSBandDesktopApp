// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.AllowedResponseTypes
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;

namespace Microsoft.Band.Admin
{
  [Flags]
  internal enum AllowedResponseTypes
  {
    None = 0,
    Keyboard = 1,
    Dictation = 2,
    Voice = 4,
    Smart = 8,
    Canned = 16, // 0x00000010
  }
}
