// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.Era
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Annotations;
using System;

namespace NodaTime.Calendars
{
  [Immutable]
  public sealed class Era
  {
    public static readonly Era Common = new Era("CE", "Eras_Common");
    public static readonly Era BeforeCommon = new Era("BCE", "Eras_BeforeCommon");
    public static readonly Era AnnoMartyrum = new Era("AM", "Eras_AnnoMartyrum");
    [Obsolete("Use AnnoMartyrum instead. This field's name was a typo, and it will be removed in a future release.")]
    public static readonly Era AnnoMartyrm = Era.AnnoMartyrum;
    public static readonly Era AnnoHegirae = new Era("EH", "Eras_AnnoHegirae");
    public static readonly Era AnnoMundi = new Era("AM", "Eras_AnnoMundi");
    public static readonly Era AnnoPersico = new Era("AP", "Eras_AnnoPersico");
    private readonly string name;
    private readonly string resourceIdentifier;

    internal string ResourceIdentifier => this.resourceIdentifier;

    internal Era(string name, string resourceIdentifier)
    {
      this.name = name;
      this.resourceIdentifier = resourceIdentifier;
    }

    public string Name => this.name;

    public override string ToString() => this.name;
  }
}
