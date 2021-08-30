// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Debug.MultipleSelectionConfigurationValueViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Microsoft.Health.App.Core.ViewModels.Debug
{
  public class MultipleSelectionConfigurationValueViewModel : ConfigurationValueViewModel
  {
    private readonly ObservableCollection<SelectableItem<Enum, string>> itemsSource;

    public MultipleSelectionConfigurationValueViewModel(
      IConfigurationService configurationService,
      IConfigurationValue value)
      : base(configurationService, value)
    {
      MultipleSelectionConfigurationValue configurationValue = (MultipleSelectionConfigurationValue) this.Value;
      this.itemsSource = (ObservableCollection<SelectableItem<Enum, string>>) Enum.GetValues(configurationValue.EnumType).OfType<Enum>().ToList<Enum>().ToSelectableItemCollection(configurationService.GetValue<IList<Enum>>((ConfigurationValue<IList<Enum>>) configurationValue));
      this.itemsSource.CollectionChanged += new NotifyCollectionChangedEventHandler(this.ItemsSourceOnCollectionChanged);
    }

    private void ItemsSourceOnCollectionChanged(
      object sender,
      NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
    {
      this.Value.SetValue(this.ConfigurationService, (object) this.itemsSource.GetSelected());
    }

    public IList<SelectableItem<Enum, string>> ItemsSource => (IList<SelectableItem<Enum, string>>) this.itemsSource;
  }
}
