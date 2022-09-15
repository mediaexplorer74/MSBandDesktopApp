// Decompiled with JetBrains decompiler
// Type: Cirrious.MvvmCross.ViewModels.MvxCommandCollection
// Assembly: Cirrious.MvvmCross, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: 74A3CCFA-A313-4770-9E45-4A087CFD7385
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_MvvmCross.dll

using Cirrious.CrossCore.Platform;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Cirrious.MvvmCross.ViewModels
{
  public class MvxCommandCollection : IMvxCommandCollection
  {
    private readonly object _owner;
    private readonly Dictionary<string, IMvxCommand> _commandLookup = new Dictionary<string, IMvxCommand>();
    private readonly Dictionary<string, List<IMvxCommand>> _canExecuteLookup = new Dictionary<string, List<IMvxCommand>>();

    public MvxCommandCollection(object owner)
    {
      this._owner = owner;
      this.SubscribeToNotifyPropertyChanged();
    }

    private void SubscribeToNotifyPropertyChanged()
    {
      if (!(this._owner is INotifyPropertyChanged owner))
        return;
      owner.PropertyChanged += new PropertyChangedEventHandler(this.OnPropertyChanged);
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
      if (string.IsNullOrEmpty(args.PropertyName))
      {
        this.RaiseAllCanExecuteChanged();
      }
      else
      {
        List<IMvxCommand> mvxCommandList;
        if (!this._canExecuteLookup.TryGetValue(args.PropertyName, out mvxCommandList))
          return;
        foreach (IMvxCommand mvxCommand in mvxCommandList)
          mvxCommand.RaiseCanExecuteChanged();
      }
    }

    private void RaiseAllCanExecuteChanged()
    {
      foreach (KeyValuePair<string, IMvxCommand> keyValuePair in this._commandLookup)
        keyValuePair.Value.RaiseCanExecuteChanged();
    }

    public IMvxCommand this[string name]
    {
      get
      {
        if (!this._commandLookup.Any<KeyValuePair<string, IMvxCommand>>())
        {
          MvxTrace.Trace("MvxCommandCollection is empty - did you forget to add your commands?");
          return (IMvxCommand) null;
        }
        IMvxCommand mvxCommand;
        this._commandLookup.TryGetValue(name, out mvxCommand);
        return mvxCommand;
      }
    }

    public void Add(IMvxCommand command, string name, string canExecuteName)
    {
      MvxCommandCollection.AddToLookup((IDictionary<string, IMvxCommand>) this._commandLookup, command, name);
      MvxCommandCollection.AddToLookup((IDictionary<string, List<IMvxCommand>>) this._canExecuteLookup, command, canExecuteName);
    }

    private static void AddToLookup(
      IDictionary<string, IMvxCommand> lookup,
      IMvxCommand command,
      string name)
    {
      if (string.IsNullOrEmpty(name))
        return;
      if (lookup.ContainsKey(name))
        MvxTrace.Warning("Ignoring Commmand - it would overwrite the existing Command, name {0}", (object) name);
      else
        lookup[name] = command;
    }

    private static void AddToLookup(
      IDictionary<string, List<IMvxCommand>> lookup,
      IMvxCommand command,
      string name)
    {
      if (string.IsNullOrEmpty(name))
        return;
      List<IMvxCommand> mvxCommandList;
      if (!lookup.TryGetValue(name, out mvxCommandList))
      {
        mvxCommandList = new List<IMvxCommand>();
        lookup[name] = mvxCommandList;
      }
      if (mvxCommandList.Contains(command))
        return;
      mvxCommandList.Add(command);
    }
  }
}
