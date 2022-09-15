// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.BufferPool
// Assembly: Microsoft.Band, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: AFCBFE03-E2CF-481D-86F4-92C60C36D26A
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Band
{
  internal sealed class BufferPool
  {
    private Queue<PooledBuffer> pool;

    internal BufferPool(int bufferSize, int maxBuffers)
    {
      if (bufferSize < 2)
        throw new ArgumentOutOfRangeException(nameof (bufferSize));
      if (maxBuffers < 1)
        throw new ArgumentOutOfRangeException(nameof (maxBuffers));
      this.BufferSize = bufferSize;
      this.MaxBuffers = maxBuffers;
      this.pool = new Queue<PooledBuffer>();
    }

    internal int BufferSize { get; private set; }

    internal int MaxBuffers { get; private set; }

    internal int Count => this.pool.Count;

    internal PooledBuffer GetBuffer(bool zero = false) => this.GetBuffer(this.BufferSize);

    internal PooledBuffer GetBuffer(int size, bool zero = false)
    {
      if (size < 0 || size > this.BufferSize)
        throw new ArgumentOutOfRangeException(nameof (size));
      PooledBuffer pooledBuffer = (PooledBuffer) null;
      if (this.pool.Count == 0)
      {
        pooledBuffer = new PooledBuffer(this, new byte[this.BufferSize], size);
      }
      else
      {
        lock (this.pool)
        {
          if (this.pool.Count == 0)
          {
            pooledBuffer = new PooledBuffer(this, new byte[this.BufferSize], size);
          }
          else
          {
            pooledBuffer = this.pool.Dequeue();
            pooledBuffer.Undispose(size);
            if (zero)
              Array.Clear((Array) pooledBuffer.Buffer, 0, pooledBuffer.Buffer.Length);
          }
        }
      }
      return pooledBuffer;
    }

    internal void ReleaseBuffer(PooledBuffer buffer)
    {
      if (buffer.Buffer.Length != this.BufferSize)
        throw new ArgumentException("The provided buffer does not belong to this pool", nameof (buffer));
      lock (this.pool)
      {
        if (this.pool.Count >= this.MaxBuffers)
          return;
        this.pool.Enqueue(buffer);
      }
    }
  }
}
