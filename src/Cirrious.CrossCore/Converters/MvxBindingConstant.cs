// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Converters.MvxBindingConstant
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

namespace Cirrious.CrossCore.Converters
{
  public class MvxBindingConstant
  {
    public static readonly MvxBindingConstant DoNothing = new MvxBindingConstant(nameof (DoNothing));
    public static readonly MvxBindingConstant UnsetValue = new MvxBindingConstant(nameof (UnsetValue));
    private readonly string _debug;

    private MvxBindingConstant(string debug) => this._debug = debug;

    public override string ToString() => "Binding:" + this._debug;
  }
}
