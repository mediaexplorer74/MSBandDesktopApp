// Decompiled with JetBrains decompiler
// Type: NodaTime.Properties.PatternResources
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace NodaTime.Properties
{
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [CompilerGenerated]
  internal class PatternResources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal PatternResources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) PatternResources.resourceMan, (object) null))
          PatternResources.resourceMan = new ResourceManager("NodaTime.Properties.PatternResources", typeof (PatternResources).Assembly);
        return PatternResources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => PatternResources.resourceCulture;
      set => PatternResources.resourceCulture = value;
    }

    internal static string Eras_AnnoHegirae => PatternResources.ResourceManager.GetString(nameof (Eras_AnnoHegirae), PatternResources.resourceCulture);

    internal static string Eras_AnnoMartyrum => PatternResources.ResourceManager.GetString(nameof (Eras_AnnoMartyrum), PatternResources.resourceCulture);

    internal static string Eras_AnnoMundi => PatternResources.ResourceManager.GetString(nameof (Eras_AnnoMundi), PatternResources.resourceCulture);

    internal static string Eras_AnnoPersico => PatternResources.ResourceManager.GetString(nameof (Eras_AnnoPersico), PatternResources.resourceCulture);

    internal static string Eras_BeforeCommon => PatternResources.ResourceManager.GetString(nameof (Eras_BeforeCommon), PatternResources.resourceCulture);

    internal static string Eras_Common => PatternResources.ResourceManager.GetString(nameof (Eras_Common), PatternResources.resourceCulture);

    internal static string OffsetPatternFull => PatternResources.ResourceManager.GetString(nameof (OffsetPatternFull), PatternResources.resourceCulture);

    internal static string OffsetPatternLong => PatternResources.ResourceManager.GetString(nameof (OffsetPatternLong), PatternResources.resourceCulture);

    internal static string OffsetPatternMedium => PatternResources.ResourceManager.GetString(nameof (OffsetPatternMedium), PatternResources.resourceCulture);

    internal static string OffsetPatternShort => PatternResources.ResourceManager.GetString(nameof (OffsetPatternShort), PatternResources.resourceCulture);
  }
}
