// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.StubErrorHandlingService
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services
{
  public class StubErrorHandlingService : IErrorHandlingService
  {
    public void HandleExceptions(Action action)
    {
    }

    public Task HandleExceptionsAsync(Func<Task> func) => (Task) Task.FromResult<object>((object) null);

    public Task HandleExceptionAsync(Exception exception) => (Task) Task.FromResult<object>((object) null);

    public Task<bool> HandleExceptionWithRetryAsync(Exception exception, bool showCancel = true) => Task.FromResult<bool>(false);

    public Task ShowBandErrorMessageAsync() => (Task) Task.FromResult<object>((object) null);

    public ErrorHandlingServiceBase.MessageInfo GetMessageInfo(
      Exception exception)
    {
      return (ErrorHandlingServiceBase.MessageInfo) null;
    }
  }
}
