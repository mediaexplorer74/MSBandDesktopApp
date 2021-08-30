// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.Transition
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System;

namespace NodaTime.TimeZones
{
  internal struct Transition : IEquatable<Transition>
  {
    private readonly Instant instant;
    private readonly Offset oldOffset;
    private readonly Offset newOffset;

    internal Instant Instant => this.instant;

    internal Offset NewOffset => this.newOffset;

    internal Transition(Instant instant, Offset oldOffset, Offset newOffset)
    {
      this.instant = instant;
      this.oldOffset = oldOffset;
      this.newOffset = newOffset;
    }

    public bool Equals(Transition other) => this.instant == other.Instant && this.oldOffset == other.oldOffset && this.newOffset == other.NewOffset;

    public static bool operator ==(Transition left, Transition right) => left.Equals(right);

    public static bool operator !=(Transition left, Transition right) => !(left == right);

    public override bool Equals(object obj) => obj is Transition other && this.Equals(other);

    public override int GetHashCode() => checked (((23 * 31 + this.instant.GetHashCode()) * 31 + this.oldOffset.GetHashCode()) * 31 + this.newOffset.GetHashCode());

    public override string ToString() => "Transition from " + (object) this.oldOffset + " to " + (object) this.newOffset + " at " + (object) this.instant;
  }
}
