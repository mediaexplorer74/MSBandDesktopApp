// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.SelectableItemExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Utilities.ObservableCollection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class SelectableItemExtensions
  {
    public static ItemsChangeObservableCollection<SelectableItem<Enum, string>> ToSelectableItemCollection(
      this IList<Enum> selectableValues,
      IList<Enum> selectedValues)
    {
      ItemsChangeObservableCollection<SelectableItem<Enum, string>> observableCollection = new ItemsChangeObservableCollection<SelectableItem<Enum, string>>();
      foreach (Enum selectableValue in (IEnumerable<Enum>) selectableValues)
        observableCollection.Add(new SelectableItem<Enum, string>(selectableValue, selectableValue.ToString(), selectedValues.Contains(selectableValue)));
      return observableCollection;
    }

    public static IList<Enum> GetSelected(
      this ICollection<SelectableItem<Enum, string>> selectablesCollection)
    {
      return (IList<Enum>) selectablesCollection.Where<SelectableItem<Enum, string>>((Func<SelectableItem<Enum, string>, bool>) (item => item.IsSelected)).Select<SelectableItem<Enum, string>, Enum>((Func<SelectableItem<Enum, string>, Enum>) (item => item.Item)).ToList<Enum>();
    }
  }
}
