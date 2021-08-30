// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.ViewModelBase
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GalaSoft.MvvmLight
{
  public abstract class ViewModelBase : ObservableObject, ICleanup
  {
    private static bool? _isInDesignMode;
    private IMessenger _messengerInstance;

    public ViewModelBase()
      : this((IMessenger) null)
    {
    }

    public ViewModelBase(IMessenger messenger) => this.MessengerInstance = messenger;

    public bool IsInDesignMode => ViewModelBase.IsInDesignModeStatic;

    public static bool IsInDesignModeStatic
    {
      get
      {
        if (!ViewModelBase._isInDesignMode.HasValue)
          ViewModelBase._isInDesignMode = new bool?(ViewModelBase.IsInDesignModePortable());
        return ViewModelBase._isInDesignMode.Value;
      }
    }

    private static bool IsInDesignModePortable()
    {
      switch (DesignerLibrary.DetectedDesignerLibrary)
      {
        case DesignerPlatformLibrary.Net:
          return ViewModelBase.IsInDesignModeNet();
        case DesignerPlatformLibrary.WinRt:
          return ViewModelBase.IsInDesignModeMetro();
        case DesignerPlatformLibrary.Silverlight:
          bool flag = ViewModelBase.IsInDesignModeSilverlight();
          if (!flag)
            flag = ViewModelBase.IsInDesignModeNet();
          return flag;
        default:
          return false;
      }
    }

    private static bool IsInDesignModeSilverlight()
    {
      try
      {
        return (bool) Type.GetType("System.ComponentModel.DesignerProperties, System.Windows, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e").GetTypeInfo().GetDeclaredProperty("IsInDesignTool").GetValue((object) null, (object[]) null);
      }
      catch
      {
        return false;
      }
    }

    private static bool IsInDesignModeMetro()
    {
      try
      {
        return (bool) Type.GetType("Windows.ApplicationModel.DesignMode, Windows, ContentType=WindowsRuntime").GetTypeInfo().GetDeclaredProperty("DesignModeEnabled").GetValue((object) null, (object[]) null);
      }
      catch
      {
        return false;
      }
    }

    private static bool IsInDesignModeNet()
    {
      try
      {
        object obj1 = Type.GetType("System.ComponentModel.DesignerProperties, PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35").GetTypeInfo().GetDeclaredField("IsInDesignModeProperty").GetValue((object) null);
        Type type1 = Type.GetType("System.ComponentModel.DependencyPropertyDescriptor, WindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
        Type type2 = Type.GetType("System.Windows.FrameworkElement, PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
        MethodInfo methodInfo = type1.GetTypeInfo().GetDeclaredMethods("FromProperty").FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>) (mi => mi.IsPublic && mi.IsStatic && mi.GetParameters().Length == 2));
        if ((object) methodInfo == null)
          return false;
        object obj2 = methodInfo.Invoke((object) null, new object[2]
        {
          obj1,
          (object) type2
        });
        object obj3 = type1.GetTypeInfo().GetDeclaredProperty("Metadata").GetValue(obj2, (object[]) null);
        return (bool) Type.GetType("System.Windows.PropertyMetadata, WindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35").GetTypeInfo().GetDeclaredProperty("DefaultValue").GetValue(obj3, (object[]) null);
      }
      catch
      {
        return false;
      }
    }

    protected IMessenger MessengerInstance
    {
      get => this._messengerInstance ?? Messenger.Default;
      set => this._messengerInstance = value;
    }

    public virtual void Cleanup() => this.MessengerInstance.Unregister((object) this);

    protected virtual void Broadcast<T>(T oldValue, T newValue, string propertyName) => this.MessengerInstance.Send<PropertyChangedMessage<T>>(new PropertyChangedMessage<T>((object) this, oldValue, newValue, propertyName));

    protected virtual void RaisePropertyChanged<T>(
      [CallerMemberName] string propertyName = null,
      T oldValue = null,
      T newValue = null,
      bool broadcast = false)
    {
      if (string.IsNullOrEmpty(propertyName))
        throw new ArgumentException("This method cannot be called with an empty string", nameof (propertyName));
      this.RaisePropertyChanged(propertyName);
      if (!broadcast)
        return;
      this.Broadcast<T>(oldValue, newValue, propertyName);
    }

    protected virtual void RaisePropertyChanged<T>(
      Expression<Func<T>> propertyExpression,
      T oldValue,
      T newValue,
      bool broadcast)
    {
      PropertyChangedEventHandler propertyChangedHandler = this.PropertyChangedHandler;
      if (propertyChangedHandler == null && !broadcast)
        return;
      string propertyName = ObservableObject.GetPropertyName<T>(propertyExpression);
      if (propertyChangedHandler != null)
        propertyChangedHandler((object) this, new PropertyChangedEventArgs(propertyName));
      if (!broadcast)
        return;
      this.Broadcast<T>(oldValue, newValue, propertyName);
    }

    protected bool Set<T>(
      Expression<Func<T>> propertyExpression,
      ref T field,
      T newValue,
      bool broadcast)
    {
      if (EqualityComparer<T>.Default.Equals(field, newValue))
        return false;
      T oldValue = field;
      field = newValue;
      this.RaisePropertyChanged<T>(propertyExpression, oldValue, field, broadcast);
      return true;
    }

    protected bool Set<T>(string propertyName, ref T field, T newValue = null, bool broadcast = false)
    {
      if (EqualityComparer<T>.Default.Equals(field, newValue))
        return false;
      T oldValue = field;
      field = newValue;
      this.RaisePropertyChanged<T>(propertyName, oldValue, field, broadcast);
      return true;
    }

    protected bool Set<T>(ref T field, T newValue = null, bool broadcast = false, [CallerMemberName] string propertyName = null)
    {
      if (EqualityComparer<T>.Default.Equals(field, newValue))
        return false;
      T oldValue = field;
      field = newValue;
      this.RaisePropertyChanged<T>(propertyName, oldValue, field, broadcast);
      return true;
    }
  }
}
