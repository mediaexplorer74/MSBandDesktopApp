// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.IProfileFieldsViewModel
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Models.Validation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.ViewModels
{
  public interface IProfileFieldsViewModel : INotifyPropertyChanged
  {
    BandUserProfile Profile { get; }

    ProfileStatus Status { get; set; }

    Property<string> FirstName { get; }

    bool HasErrors { get; }

    bool ShowWelcomeMessage { get; set; }

    bool IsDirty { get; set; }

    void MarkSaved();

    bool Validate();

    void RefreshProfileStatus();

    Task LoadDataAsync(IDictionary<string, string> parameters = null);

    void Revert();
  }
}
