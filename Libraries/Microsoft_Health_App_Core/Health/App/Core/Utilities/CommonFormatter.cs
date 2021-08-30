// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.CommonFormatter
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class CommonFormatter
  {
    public static string FormatBytes(ulong bytes)
    {
      if (bytes < 1024UL)
        return bytes.ToString() + " bytes";
      if (bytes < 1048576UL)
      {
        double size = (double) bytes / 1024.0;
        return size.ToString(CommonFormatter.GetFormatForFilesize(size)) + " KB";
      }
      if (bytes < 1073741824UL)
      {
        double size = (double) bytes / 1048576.0;
        return size.ToString(CommonFormatter.GetFormatForFilesize(size)) + " MB";
      }
      double size1 = (double) bytes / 1073741824.0;
      return size1.ToString(CommonFormatter.GetFormatForFilesize(size1)) + " GB";
    }

    private static string GetFormatForFilesize(double size)
    {
      int num1 = 0;
      double num2 = size;
      while (num2 > 1.0)
      {
        num2 /= 10.0;
        ++num1;
      }
      return "F" + (object) Math.Min(2, 3 - num1);
    }
  }
}
