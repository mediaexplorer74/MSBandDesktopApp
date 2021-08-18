// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.RegistrySettingsProvider
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;

namespace DesktopSyncApp
{
  internal class RegistrySettingsProvider : SettingsProvider
  {
    public override string ApplicationName
    {
      get => Globals.ApplicationName;
      set
      {
      }
    }

    public override void Initialize(string name, NameValueCollection col) => base.Initialize(this.ApplicationName, col);

    public override void SetPropertyValues(
      SettingsContext Context,
      SettingsPropertyValueCollection PropValues)
    {
      RegistryKey registryKey1 = (RegistryKey) null;
      RegistryKey registryKey2 = (RegistryKey) null;
      try
      {
        foreach (SettingsPropertyValue propValue in PropValues)
        {
          if (propValue.IsDirty)
          {
            RegistryKey registryKey3;
            if (this.IsUserScoped(propValue.Property))
            {
              if (registryKey1 == null)
                registryKey1 = Registry.CurrentUser.CreateSubKey(Globals.RegistrySoftwareAppRootPath);
              registryKey3 = registryKey1;
            }
            else
            {
              if (registryKey2 == null)
                registryKey2 = Registry.CurrentUser.CreateSubKey(Globals.RegistrySoftwareAppRootPath);
              registryKey3 = registryKey2;
            }
            registryKey3.SetValue(propValue.Name, propValue.SerializedValue);
            propValue.IsDirty = false;
          }
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        registryKey1?.Dispose();
        registryKey2?.Dispose();
      }
    }

    public override SettingsPropertyValueCollection GetPropertyValues(
      SettingsContext Context,
      SettingsPropertyCollection PropValues)
    {
      SettingsPropertyValueCollection propertyValueCollection = new SettingsPropertyValueCollection();
      RegistryKey registryKey1 = (RegistryKey) null;
      bool flag1 = false;
      RegistryKey registryKey2 = (RegistryKey) null;
      bool flag2 = false;
      try
      {
        foreach (SettingsProperty propValue in PropValues)
        {
          SettingsPropertyValue property = new SettingsPropertyValue(propValue);
          RegistryKey registryKey3;
          if (this.IsUserScoped(propValue))
          {
            if (registryKey1 == null && !flag1)
            {
              flag1 = true;
              try
              {
                registryKey1 = Registry.CurrentUser.OpenSubKey(Globals.RegistrySoftwareAppRootPath);
              }
              catch
              {
              }
            }
            registryKey3 = registryKey1;
          }
          else
          {
            if (registryKey2 == null && !flag2)
            {
              flag2 = true;
              try
              {
                registryKey2 = Registry.LocalMachine.OpenSubKey(Globals.RegistrySoftwareAppRootPath);
              }
              catch
              {
              }
            }
            registryKey3 = registryKey2;
          }
          property.IsDirty = true;
          if (registryKey3 != null)
          {
            property.SerializedValue = registryKey3.GetValue(propValue.Name);
            if (property.SerializedValue != null)
              property.IsDirty = false;
          }
          if (property.IsDirty)
          {
            if (property.PropertyValue != null)
              property.PropertyValue = property.PropertyValue;
            else
              property.IsDirty = false;
          }
          propertyValueCollection.Add(property);
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        registryKey1?.Dispose();
        registryKey2?.Dispose();
      }
      return propertyValueCollection;
    }

    private bool IsUserScoped(SettingsProperty prop)
    {
      foreach (DictionaryEntry attribute in (Hashtable) prop.Attributes)
      {
        if (((Attribute) attribute.Value).GetType() == typeof (UserScopedSettingAttribute))
          return true;
      }
      return false;
    }
  }
}
