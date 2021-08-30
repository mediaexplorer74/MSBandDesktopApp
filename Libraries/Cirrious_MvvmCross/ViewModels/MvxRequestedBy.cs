// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxRequestedBy
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxRequestedBy
  {
    public static MvxRequestedBy Unknown = new MvxRequestedBy(MvxRequestedByType.Unknown);
    public static MvxRequestedBy Bookmark = new MvxRequestedBy(MvxRequestedByType.Bookmark);
    public static MvxRequestedBy UserAction = new MvxRequestedBy(MvxRequestedByType.UserAction);

    public MvxRequestedBy()
      : this(MvxRequestedByType.Unknown)
    {
    }

    public MvxRequestedBy(MvxRequestedByType requestedByType)
      : this(requestedByType, (string) null)
    {
    }

    public MvxRequestedBy(MvxRequestedByType requestedByType, string additionalInfo)
    {
      this.Type = requestedByType;
      this.AdditionalInfo = additionalInfo;
    }

    public MvxRequestedByType Type { get; set; }

    public string AdditionalInfo { get; set; }
  }
}
