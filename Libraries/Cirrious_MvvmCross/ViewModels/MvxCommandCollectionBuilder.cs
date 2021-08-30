// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxCommandCollectionBuilder
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxCommandCollectionBuilder : IMvxCommandCollectionBuilder
  {
    private const string DefaultCommandSuffix = "Command";
    private const string DefaultCanExecutePrefix = "CanExecute";

    public string CommandSuffix { get; set; }

    public IEnumerable<string> AdditionalCommandSuffixes { get; set; }

    public string CanExecutePrefix { get; set; }

    public MvxCommandCollectionBuilder()
    {
      this.CanExecutePrefix = "CanExecute";
      this.CommandSuffix = "Command";
      this.AdditionalCommandSuffixes = (IEnumerable<string>) null;
    }

    public virtual IMvxCommandCollection BuildCollectionFor(object owner)
    {
      MvxCommandCollection toReturn = new MvxCommandCollection(owner);
      this.CreateCommands(owner, toReturn);
      return (IMvxCommandCollection) toReturn;
    }

    protected virtual void CreateCommands(object owner, MvxCommandCollection toReturn)
    {
      foreach (var data in owner.GetType().GetMethods(Cirrious.CrossCore.BindingFlags.Instance | Cirrious.CrossCore.BindingFlags.Public | Cirrious.CrossCore.BindingFlags.FlattenHierarchy).Select(method => new
      {
        method = method,
        parameterCount = ((IEnumerable<ParameterInfo>) method.GetParameters()).Count<ParameterInfo>()
      }).Where(_param0 => _param0.parameterCount <= 1).Select(_param1 => new
      {
        \u003C\u003Eh__TransparentIdentifier0 = _param1,
        commandName = this.GetCommandNameOrNull(_param1.method)
      }).Where(_param0 => !string.IsNullOrEmpty(_param0.commandName)).Select(_param0 => new
      {
        Method = _param0.\u003C\u003Eh__TransparentIdentifier0.method,
        CommandName = _param0.commandName,
        HasParameter = _param0.\u003C\u003Eh__TransparentIdentifier0.parameterCount > 0
      }))
        this.CreateCommand(owner, toReturn, data.Method, data.CommandName, data.HasParameter);
    }

    protected virtual void CreateCommand(
      object owner,
      MvxCommandCollection collection,
      MethodInfo commandMethod,
      string commandName,
      bool hasParameter)
    {
      PropertyInfo canExecutePropertyInfo = this.CanExecutePropertyInfo(owner.GetType(), commandMethod);
      MvxCommandCollectionBuilder.IMvxCommandBuilder mvxCommandBuilder = hasParameter ? (MvxCommandCollectionBuilder.IMvxCommandBuilder) new MvxCommandCollectionBuilder.MvxParameterizedCommandBuilder(commandMethod, canExecutePropertyInfo) : (MvxCommandCollectionBuilder.IMvxCommandBuilder) new MvxCommandCollectionBuilder.MvxCommandBuilder(commandMethod, canExecutePropertyInfo);
      IMvxCommand command = mvxCommandBuilder.ToCommand(owner);
      collection.Add(command, commandName, mvxCommandBuilder.CanExecutePropertyName);
    }

    protected virtual PropertyInfo CanExecutePropertyInfo(
      Type type,
      MethodInfo commandMethod)
    {
      string name = this.CanExecuteProperyName(commandMethod);
      if (string.IsNullOrEmpty(name))
        return (PropertyInfo) null;
      PropertyInfo property = type.GetProperty(name, Cirrious.CrossCore.BindingFlags.Instance | Cirrious.CrossCore.BindingFlags.Public);
      if ((object) property == null)
        return (PropertyInfo) null;
      if ((object) property.PropertyType != (object) typeof (bool))
        return (PropertyInfo) null;
      return !property.CanRead ? (PropertyInfo) null : property;
    }

    protected virtual string GetCommandNameOrNull(MethodInfo method)
    {
      MvxCommandAttribute commandAttribute = this.CommandAttribute(method);
      if (commandAttribute != null)
        return commandAttribute.CommandName;
      string commandNameOrNull1 = this.GetConventionalCommandNameOrNull(method, this.CommandSuffix);
      if (commandNameOrNull1 != null)
        return commandNameOrNull1;
      if (this.AdditionalCommandSuffixes != null)
      {
        foreach (string additionalCommandSuffix in this.AdditionalCommandSuffixes)
        {
          string commandNameOrNull2 = this.GetConventionalCommandNameOrNull(method, additionalCommandSuffix);
          if (commandNameOrNull2 != null)
            return commandNameOrNull2;
        }
      }
      return (string) null;
    }

    protected virtual string GetConventionalCommandNameOrNull(MethodInfo method, string suffix)
    {
      if (!method.Name.EndsWith(suffix))
        return (string) null;
      int length = method.Name.Length - suffix.Length;
      return length <= 0 ? (string) null : method.Name.Substring(0, length);
    }

    protected MvxCommandAttribute CommandAttribute(MethodInfo method) => (MvxCommandAttribute) CustomAttributeExtensions.GetCustomAttributes(method, typeof (MvxCommandAttribute), true).FirstOrDefault<Attribute>();

    protected virtual string CanExecuteProperyName(MethodInfo method)
    {
      MvxCommandAttribute commandAttribute = this.CommandAttribute(method);
      return commandAttribute != null ? commandAttribute.CanExecutePropertyName : this.CanExecutePrefix + method.Name;
    }

    public interface IMvxCommandBuilder
    {
      IMvxCommand ToCommand(object owner);

      string CanExecutePropertyName { get; }
    }

    public abstract class MvxBaseCommandBuilder : MvxCommandCollectionBuilder.IMvxCommandBuilder
    {
      private readonly MethodInfo _executeMethodInfo;
      private readonly PropertyInfo _canExecutePropertyInfo;

      protected MethodInfo ExecuteMethodInfo => this._executeMethodInfo;

      protected PropertyInfo CanExecutePropertyInfo => this._canExecutePropertyInfo;

      protected MvxBaseCommandBuilder(
        MethodInfo executeMethodInfo,
        PropertyInfo canExecutePropertyInfo)
      {
        this._executeMethodInfo = executeMethodInfo;
        this._canExecutePropertyInfo = canExecutePropertyInfo;
      }

      public abstract IMvxCommand ToCommand(object owner);

      public string CanExecutePropertyName => (object) this._canExecutePropertyInfo != null ? this._canExecutePropertyInfo.Name : (string) null;
    }

    public class MvxCommandBuilder : MvxCommandCollectionBuilder.MvxBaseCommandBuilder
    {
      public MvxCommandBuilder(MethodInfo executeMethodInfo, PropertyInfo canExecutePropertyInfo)
        : base(executeMethodInfo, canExecutePropertyInfo)
      {
      }

      public override IMvxCommand ToCommand(object owner)
      {
        Action execute = (Action) (() => this.ExecuteMethodInfo.Invoke(owner, new object[0]));
        Func<bool> canExecute = (Func<bool>) null;
        if ((object) this.CanExecutePropertyInfo != null)
          canExecute = (Func<bool>) (() => (bool) this.CanExecutePropertyInfo.GetValue(owner, (object[]) null));
        return (IMvxCommand) new MvxCommand(execute, canExecute);
      }
    }

    public class MvxParameterizedCommandBuilder : MvxCommandCollectionBuilder.MvxBaseCommandBuilder
    {
      public MvxParameterizedCommandBuilder(
        MethodInfo executeMethodInfo,
        PropertyInfo canExecutePropertyInfo)
        : base(executeMethodInfo, canExecutePropertyInfo)
      {
      }

      public override IMvxCommand ToCommand(object owner)
      {
        Action<object> execute = (Action<object>) (obj => this.ExecuteMethodInfo.Invoke(owner, new object[1]
        {
          obj
        }));
        Func<object, bool> canExecute = (Func<object, bool>) null;
        if ((object) this.CanExecutePropertyInfo != null)
          canExecute = (Func<object, bool>) (ignored => (bool) this.CanExecutePropertyInfo.GetValue(owner, (object[]) null));
        return (IMvxCommand) new MvxCommand<object>(execute, canExecute);
      }
    }
  }
}
