// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.AgeUtilities
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class AgeUtilities
  {
    public static int GetAge(DateTime birthDate)
    {
      DateTime today = DateTime.Today;
      if (!(birthDate < today))
        return 0;
      int num = today.Year - birthDate.Year;
      if (today.Month < birthDate.Month || today.Month == birthDate.Month && today.Day < birthDate.Day)
        --num;
      return num;
    }
  }
}
