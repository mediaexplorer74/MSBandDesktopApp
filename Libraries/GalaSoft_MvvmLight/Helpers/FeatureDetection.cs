// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Helpers.FeatureDetection
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

using System.Reflection;

namespace GalaSoft.MvvmLight.Helpers
{
  internal static class FeatureDetection
  {
    private static bool? _isPrivateReflectionSupported;

    public static bool IsPrivateReflectionSupported
    {
      get
      {
        if (!FeatureDetection._isPrivateReflectionSupported.HasValue)
          FeatureDetection._isPrivateReflectionSupported = new bool?(FeatureDetection.ResolveIsPrivateReflectionSupported());
        return FeatureDetection._isPrivateReflectionSupported.Value;
      }
    }

    private static bool ResolveIsPrivateReflectionSupported()
    {
      FeatureDetection.ReflectionDetectionClass reflectionDetectionClass = new FeatureDetection.ReflectionDetectionClass();
      try
      {
        typeof (FeatureDetection.ReflectionDetectionClass).GetTypeInfo().GetDeclaredMethod("Method").Invoke((object) reflectionDetectionClass, (object[]) null);
      }
      catch
      {
        return false;
      }
      return true;
    }

    private class ReflectionDetectionClass
    {
      private void Method()
      {
      }
    }
  }
}
