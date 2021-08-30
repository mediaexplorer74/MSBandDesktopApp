// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.Deque`1
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Utilities
{
  public class Deque<T>
  {
    private const int DefaultInitialCapacity = 10;
    private T[] buffer;
    private int leftIndex;
    private int rightIndex;

    public Deque()
    {
      this.buffer = new T[10];
      this.leftIndex = 1;
      this.rightIndex = 0;
    }

    public int Count { get; private set; }

    public void EnqueueLeft(T item)
    {
      this.EnsureCapacity();
      this.leftIndex = this.Left(this.leftIndex);
      this.buffer[this.leftIndex] = item;
      ++this.Count;
    }

    public void EnqueueRight(T item)
    {
      this.EnsureCapacity();
      this.rightIndex = this.Right(this.rightIndex);
      this.buffer[this.rightIndex] = item;
      ++this.Count;
    }

    public T DequeueLeft()
    {
      this.ThrowIfEmpty();
      T obj = this.buffer[this.leftIndex];
      this.leftIndex = this.Right(this.leftIndex);
      --this.Count;
      return obj;
    }

    public T DequeueRight()
    {
      this.ThrowIfEmpty();
      T obj = this.buffer[this.rightIndex];
      this.rightIndex = this.Left(this.rightIndex);
      --this.Count;
      return obj;
    }

    public T PeekLeft()
    {
      this.ThrowIfEmpty();
      return this.buffer[this.leftIndex];
    }

    public T PeekRight()
    {
      this.ThrowIfEmpty();
      return this.buffer[this.rightIndex];
    }

    private int Left(int index) => index == 0 ? this.buffer.Length - 1 : index - 1;

    private int Right(int index) => index == this.buffer.Length - 1 ? 0 : index + 1;

    private void ThrowIfEmpty()
    {
      if (this.Count == 0)
        throw new InvalidOperationException("Cannot complete operation, there are no items.");
    }

    private void EnsureCapacity()
    {
      int length1 = this.buffer.Length;
      if (this.Count + 1 != length1)
        return;
      int length2 = length1 * 2;
      T[] objArray = new T[length2];
      if (this.rightIndex >= this.leftIndex)
      {
        Array.Copy((Array) this.buffer, this.leftIndex, (Array) objArray, this.leftIndex, this.rightIndex - this.leftIndex + 1);
      }
      else
      {
        Array.Copy((Array) this.buffer, 0, (Array) objArray, 0, this.rightIndex + 1);
        int length3 = length1 - this.leftIndex;
        int destinationIndex = length2 - length3;
        Array.Copy((Array) this.buffer, this.leftIndex, (Array) objArray, destinationIndex, length3);
        this.leftIndex = destinationIndex;
      }
      this.buffer = objArray;
    }
  }
}
