// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.MessageBoxServiceExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Resources;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public static class MessageBoxServiceExtensions
  {
    public static async Task ShowUnexpectedErrorAsync(
      this IMessageBoxService service,
      string messageBoxText)
    {
      int num = (int) await service.ShowAsync(messageBoxText, AppResources.OopsCaption, PortableMessageBoxButton.OK);
    }

    public static async Task<PortableMessageBoxResult?> TryShowAsync(
      this IMessageBoxService messageBoxService,
      string message,
      string title,
      PortableMessageBoxButton buttons = PortableMessageBoxButton.OK,
      params string[] buttonNames)
    {
      try
      {
        return new PortableMessageBoxResult?(await messageBoxService.ShowAsync(message, title, buttons, buttonNames));
      }
      catch (HeadlessException ex)
      {
      }
      return new PortableMessageBoxResult?();
    }
  }
}
