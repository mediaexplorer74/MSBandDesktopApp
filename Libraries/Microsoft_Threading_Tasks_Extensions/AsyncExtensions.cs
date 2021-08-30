// Decompiled with JetBrains decompiler
// Type: AsyncExtensions
// Assembly: Microsoft.Threading.Tasks.Extensions, Version=1.0.12.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 32DE64FC-4C1B-4762-B488-E8BFE502A0BB
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Threading_Tasks_Extensions.dll

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

public static class AsyncExtensions
{
  public static Task<int> ReadAsync(this Stream source, byte[] buffer, int offset, int count) => AsyncExtensions.ReadAsync(source, buffer, offset, count, CancellationToken.None);

  public static Task<int> ReadAsync(
    this Stream source,
    byte[] buffer,
    int offset,
    int count,
    CancellationToken cancellationToken)
  {
    return cancellationToken.IsCancellationRequested ? TaskServices.FromCancellation<int>(cancellationToken) : Task<int>.Factory.FromAsync<byte[], int, int>(new Func<byte[], int, int, AsyncCallback, object, IAsyncResult>(source.BeginRead), new Func<IAsyncResult, int>(source.EndRead), buffer, offset, count, (object) null);
  }

  public static Task WriteAsync(this Stream source, byte[] buffer, int offset, int count) => AsyncExtensions.WriteAsync(source, buffer, offset, count, CancellationToken.None);

  public static Task WriteAsync(
    this Stream source,
    byte[] buffer,
    int offset,
    int count,
    CancellationToken cancellationToken)
  {
    return cancellationToken.IsCancellationRequested ? TaskServices.FromCancellation(cancellationToken) : Task.Factory.FromAsync<byte[], int, int>(new Func<byte[], int, int, AsyncCallback, object, IAsyncResult>(source.BeginWrite), new Action<IAsyncResult>(source.EndWrite), buffer, offset, count, (object) null);
  }

  public static Task FlushAsync(this Stream source) => AsyncExtensions.FlushAsync(source, CancellationToken.None);

  public static Task FlushAsync(this Stream source, CancellationToken cancellationToken) => cancellationToken.IsCancellationRequested ? TaskServices.FromCancellation(cancellationToken) : Task.Factory.StartNew((Action<object>) (s => ((Stream) s).Flush()), (object) source, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);

  public static Task CopyToAsync(this Stream source, Stream destination) => AsyncExtensions.CopyToAsync(source, destination, 4096);

  public static Task CopyToAsync(this Stream source, Stream destination, int bufferSize) => AsyncExtensions.CopyToAsync(source, destination, bufferSize, CancellationToken.None);

  public static Task CopyToAsync(
    this Stream source,
    Stream destination,
    int bufferSize,
    CancellationToken cancellationToken)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    if (destination == null)
      throw new ArgumentNullException(nameof (destination));
    if (bufferSize <= 0)
      throw new ArgumentOutOfRangeException(nameof (bufferSize));
    if (!source.CanRead && !source.CanWrite)
      throw new ObjectDisposedException(nameof (source));
    if (!destination.CanRead && !destination.CanWrite)
      throw new ObjectDisposedException(nameof (destination));
    if (!source.CanRead)
      throw new NotSupportedException();
    if (!destination.CanWrite)
      throw new NotSupportedException();
    return source.CopyToAsyncInternal(destination, bufferSize, cancellationToken);
  }

  private static async Task CopyToAsyncInternal(
    this Stream source,
    Stream destination,
    int bufferSize,
    CancellationToken cancellationToken)
  {
    byte[] buffer = new byte[bufferSize];
    while (true)
    {
      int num = await AsyncExtensions.ReadAsync(source, buffer, 0, buffer.Length, cancellationToken).ConfigureAwait<int>(false);
      int bytesRead;
      if ((bytesRead = num) > 0)
        await AwaitExtensions.ConfigureAwait(AsyncExtensions.WriteAsync(destination, buffer, 0, bytesRead, cancellationToken), false);
      else
        break;
    }
  }

  public static Task<int> ReadAsync(
    this TextReader source,
    char[] buffer,
    int index,
    int count)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    return Task.Factory.StartNew<int>((Func<int>) (() => source.Read(buffer, index, count)), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
  }

  public static Task<int> ReadBlockAsync(
    this TextReader source,
    char[] buffer,
    int index,
    int count)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    return Task.Factory.StartNew<int>((Func<int>) (() => source.ReadBlock(buffer, index, count)), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
  }

  public static Task<string> ReadLineAsync(this TextReader source)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    return Task.Factory.StartNew<string>((Func<string>) (() => source.ReadLine()), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
  }

  public static Task<string> ReadToEndAsync(this TextReader source)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    return Task.Factory.StartNew<string>((Func<string>) (() => source.ReadToEnd()), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
  }

  public static Task WriteAsync(this TextWriter target, string value)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    return Task.Factory.StartNew((Action) (() => target.Write(value)), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
  }

  public static Task WriteAsync(this TextWriter target, char value)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    return Task.Factory.StartNew((Action) (() => target.Write(value)), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
  }

  public static Task WriteAsync(this TextWriter target, char[] buffer) => AsyncExtensions.WriteAsync(target, buffer, 0, buffer.Length);

  public static Task WriteAsync(this TextWriter target, char[] buffer, int index, int count)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    return Task.Factory.StartNew((Action) (() => target.Write(buffer, index, count)), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
  }

  public static Task WriteLineAsync(this TextWriter target)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    return Task.Factory.StartNew((Action) (() => target.WriteLine()), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
  }

  public static Task WriteLineAsync(this TextWriter target, string value)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    return Task.Factory.StartNew((Action) (() => target.WriteLine(value)), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
  }

  public static Task WriteLineAsync(this TextWriter target, char value)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    return Task.Factory.StartNew((Action) (() => target.WriteLine(value)), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
  }

  public static Task WriteLineAsync(this TextWriter target, char[] buffer) => AsyncExtensions.WriteLineAsync(target, buffer, 0, buffer.Length);

  public static Task WriteLineAsync(
    this TextWriter target,
    char[] buffer,
    int index,
    int count)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    return Task.Factory.StartNew((Action) (() => target.WriteLine(buffer, index, count)), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
  }

  public static Task FlushAsync(this TextWriter target)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    return Task.Factory.StartNew((Action) (() => target.Flush()), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
  }

  public static Task<WebResponse> GetResponseAsync(this WebRequest source) => Task.Factory.StartNew<Task<WebResponse>>((Func<Task<WebResponse>>) (() => Task<WebResponse>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(source.BeginGetResponse), new Func<IAsyncResult, WebResponse>(source.EndGetResponse), (object) null)), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap<WebResponse>();

  public static Task<Stream> GetRequestStreamAsync(this WebRequest source) => Task.Factory.StartNew<Task<Stream>>((Func<Task<Stream>>) (() => Task<Stream>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(source.BeginGetRequestStream), new Func<IAsyncResult, Stream>(source.EndGetRequestStream), (object) null)), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap<Stream>();
}
