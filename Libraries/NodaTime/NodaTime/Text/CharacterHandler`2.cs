﻿// Decompiled with JetBrains decompiler
// Type: NodaTime.Text.CharacterHandler`2
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Text.Patterns;

namespace NodaTime.Text
{
  internal delegate void CharacterHandler<TResult, TBucket>(
    PatternCursor patternCursor,
    SteppedPatternBuilder<TResult, TBucket> patternBuilder)
    where TBucket : ParseBucket<TResult>;
}