// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Themes.BandBackgroundStyle
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Themes
{
  public sealed class BandBackgroundStyle
  {
    public BandBackgroundStyle(ushort id, string name)
    {
      switch (name)
      {
        case "":
          throw new ArgumentException("a valid non-whitespace name must be provided");
        case null:
          throw new ArgumentNullException(nameof (name));
        default:
          if (name.Trim().Length != 0)
          {
            this.Id = id;
            this.Name = name;
            break;
          }
          goto case "";
      }
    }

    public ushort Id { get; private set; }

    public string Name { get; private set; }
  }
}
