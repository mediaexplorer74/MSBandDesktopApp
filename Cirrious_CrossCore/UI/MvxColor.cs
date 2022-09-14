// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.UI.MvxColor
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

namespace Cirrious.CrossCore.UI
{
  public class MvxColor
  {
    public int ARGB { get; set; }

    private static int MaskAndShiftRight(int value, uint mask, int shift) => (int) (((long) value & (long) mask) >> shift);

    private static int ShiftOverwrite(int original, uint mask, int value, int shift) => (int) ((long) original & (long) mask | (long) (value << shift));

    public int R
    {
      get => MvxColor.MaskAndShiftRight(this.ARGB, 16711680U, 16);
      set => this.ARGB = MvxColor.ShiftOverwrite(this.ARGB, 4278255615U, value, 16);
    }

    public int G
    {
      get => MvxColor.MaskAndShiftRight(this.ARGB, 65280U, 8);
      set => this.ARGB = MvxColor.ShiftOverwrite(this.ARGB, 4294902015U, value, 8);
    }

    public int B
    {
      get => MvxColor.MaskAndShiftRight(this.ARGB, (uint) byte.MaxValue, 0);
      set => this.ARGB = MvxColor.ShiftOverwrite(this.ARGB, 4294967040U, value, 0);
    }

    public int A
    {
      get => MvxColor.MaskAndShiftRight(this.ARGB, 4278190080U, 24);
      set => this.ARGB = MvxColor.ShiftOverwrite(this.ARGB, 16777215U, value, 24);
    }

    public MvxColor(uint argb)
      : this((int) argb)
    {
    }

    public MvxColor(int argb) => this.ARGB = argb;

    public MvxColor(uint rgb, int alpha)
      : this((int) rgb, alpha)
    {
    }

    public MvxColor(int rgb, int alpha)
    {
      this.ARGB = rgb;
      this.A = alpha;
    }

    public MvxColor(int red, int green, int blue, int alpha = 255)
    {
      this.R = red;
      this.G = green;
      this.B = blue;
      this.A = alpha;
    }

    public override string ToString() => string.Format("argb: #{0:X2}{1:X2}{2:X2}{3:X2}", (object) this.A, (object) this.R, (object) this.G, (object) this.B);
  }
}
