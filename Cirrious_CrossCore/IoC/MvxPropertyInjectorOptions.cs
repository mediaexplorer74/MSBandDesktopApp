// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.IoC.MvxPropertyInjectorOptions
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

namespace Cirrious.CrossCore.IoC
{
  public class MvxPropertyInjectorOptions : IMvxPropertyInjectorOptions
  {
    private static IMvxPropertyInjectorOptions _mvxInjectProperties;
    private static IMvxPropertyInjectorOptions _allProperties;

    public MvxPropertyInjectorOptions()
    {
      this.InjectIntoProperties = MvxPropertyInjection.None;
      this.ThrowIfPropertyInjectionFails = false;
    }

    public MvxPropertyInjection InjectIntoProperties { get; set; }

    public bool ThrowIfPropertyInjectionFails { get; set; }

    public static IMvxPropertyInjectorOptions MvxInject
    {
      get
      {
        IMvxPropertyInjectorOptions propertyInjectorOptions = MvxPropertyInjectorOptions._mvxInjectProperties;
        if (propertyInjectorOptions == null)
          propertyInjectorOptions = (IMvxPropertyInjectorOptions) new MvxPropertyInjectorOptions()
          {
            InjectIntoProperties = MvxPropertyInjection.MvxInjectInterfaceProperties,
            ThrowIfPropertyInjectionFails = false
          };
        MvxPropertyInjectorOptions._mvxInjectProperties = propertyInjectorOptions;
        return MvxPropertyInjectorOptions._mvxInjectProperties;
      }
    }

    public static IMvxPropertyInjectorOptions All
    {
      get
      {
        IMvxPropertyInjectorOptions propertyInjectorOptions = MvxPropertyInjectorOptions._allProperties;
        if (propertyInjectorOptions == null)
          propertyInjectorOptions = (IMvxPropertyInjectorOptions) new MvxPropertyInjectorOptions()
          {
            InjectIntoProperties = MvxPropertyInjection.AllInterfaceProperties,
            ThrowIfPropertyInjectionFails = false
          };
        MvxPropertyInjectorOptions._allProperties = propertyInjectorOptions;
        return MvxPropertyInjectorOptions._allProperties;
      }
    }
  }
}
