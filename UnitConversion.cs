// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.UnitConversion
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;

namespace DesktopSyncApp
{
  public static class UnitConversion
  {
    public static void MMToFeetAndInches(int mm, out int feet, out int inches)
    {
      inches = (int) Math.Round((double) mm / 25.4, 0, MidpointRounding.AwayFromZero);
      feet = (int) Math.Floor((double) inches / 12.0);
      inches -= feet * 12;
    }

    public static int MMToFeetPortion(int mm)
    {
      int feet;
      UnitConversion.MMToFeetAndInches(mm, out feet, out int _);
      return feet;
    }

    public static int MMToInchesPortion(int mm)
    {
      int inches;
      UnitConversion.MMToFeetAndInches(mm, out int _, out inches);
      return inches;
    }

    public static int FeetAndInchesToMM(int feet, int inches) => (int) Math.Round((double) (feet * 12 + inches) * 25.4, 0, MidpointRounding.AwayFromZero);

    public static int MMToCM(int mm) => (int) Math.Round((double) mm / 10.0, 0, MidpointRounding.AwayFromZero);

    public static int CMToMM(int cm) => cm * 10;

    public static int GramsToPounds(int grams) => (int) Math.Round((double) grams / 453.592, 0);

    public static int GramsToKilograms(int grams) => (int) Math.Round((double) grams / 1000.0, 0, MidpointRounding.AwayFromZero);

    public static int PoundsToGrams(int pounds) => (int) Math.Round((double) pounds * 453.592, 0, MidpointRounding.AwayFromZero);

    public static int KilogramsToGrams(int kilograms) => kilograms * 1000;
  }
}
