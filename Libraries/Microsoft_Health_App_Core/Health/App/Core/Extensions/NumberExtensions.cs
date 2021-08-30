// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.NumberExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class NumberExtensions
  {
    public static bool IsCloseTo(this double a, double b, double tolerance) => Math.Abs(a - b) < tolerance;

    public static double RoundUp(this double toRound, double toValue = 1.0) => Math.Ceiling(toRound / toValue) * toValue;

    public static double RoundDown(this double toRound, double toValue = 1.0) => Math.Floor(toRound / toValue) * toValue;
  }
}
