// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JContainer
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Newtonsoft.Json.Linq
{
  public abstract class JContainer : 
    JToken,
    IList<JToken>,
    ICollection<JToken>,
    IEnumerable<JToken>,
    ITypedList,
    IBindingList,
    IList,
    ICollection,
    IEnumerable,
    INotifyCollectionChanged
  {
    internal ListChangedEventHandler _listChanged;
    internal AddingNewEventHandler _addingNew;
    internal NotifyCollectionChangedEventHandler _collectionChanged;
    private object _syncRoot;
    private bool _busy;

    public event ListChangedEventHandler ListChanged
    {
      add => this._listChanged += value;
      remove => this._listChanged -= value;
    }

    public event AddingNewEventHandler AddingNew
    {
      add => this._addingNew += value;
      remove => this._addingNew -= value;
    }

    public event NotifyCollectionChangedEventHandler CollectionChanged
    {
      add => this._collectionChanged += value;
      remove => this._collectionChanged -= value;
    }

    protected abstract IList<JToken> ChildrenTokens { get; }

    internal JContainer()
    {
    }

    internal JContainer(JContainer other)
      : this()
    {
      ValidationUtils.ArgumentNotNull((object) other, "c");
      int index = 0;
      foreach (JToken jtoken in (IEnumerable<JToken>) other)
      {
        this.AddInternal(index, (object) jtoken, false);
        ++index;
      }
    }

    internal void CheckReentrancy()
    {
      if (this._busy)
        throw new InvalidOperationException("Cannot change {0} during a collection change event.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
    }

    internal virtual IList<JToken> CreateChildrenCollection() => (IList<JToken>) new List<JToken>();

    protected virtual void OnAddingNew(AddingNewEventArgs e)
    {
      AddingNewEventHandler addingNew = this._addingNew;
      if (addingNew == null)
        return;
      addingNew((object) this, e);
    }

    protected virtual void OnListChanged(ListChangedEventArgs e)
    {
      ListChangedEventHandler listChanged = this._listChanged;
      if (listChanged == null)
        return;
      this._busy = true;
      try
      {
        listChanged((object) this, e);
      }
      finally
      {
        this._busy = false;
      }
    }

    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      NotifyCollectionChangedEventHandler collectionChanged = this._collectionChanged;
      if (collectionChanged == null)
        return;
      this._busy = true;
      try
      {
        collectionChanged((object) this, e);
      }
      finally
      {
        this._busy = false;
      }
    }

    public override bool HasValues => this.ChildrenTokens.Count > 0;

    internal bool ContentsEqual(JContainer container)
    {
      if (container == this)
        return true;
      IList<JToken> childrenTokens1 = this.ChildrenTokens;
      IList<JToken> childrenTokens2 = container.ChildrenTokens;
      if (childrenTokens1.Count != childrenTokens2.Count)
        return false;
      for (int index = 0; index < childrenTokens1.Count; ++index)
      {
        if (!childrenTokens1[index].DeepEquals(childrenTokens2[index]))
          return false;
      }
      return true;
    }

    public override JToken First => this.ChildrenTokens.FirstOrDefault<JToken>();

    public override JToken Last => this.ChildrenTokens.LastOrDefault<JToken>();

    public override JEnumerable<JToken> Children() => new JEnumerable<JToken>((IEnumerable<JToken>) this.ChildrenTokens);

    public override IEnumerable<T> Values<T>() => this.ChildrenTokens.Convert<JToken, T>();

    public IEnumerable<JToken> Descendants() => this.GetDescendants(false);

    public IEnumerable<JToken> DescendantsAndSelf() => this.GetDescendants(true);

    internal IEnumerable<JToken> GetDescendants(bool self)
    {
      if (self)
        yield return (JToken) this;
      foreach (JToken o in (IEnumerable<JToken>) this.ChildrenTokens)
      {
        yield return o;
        if (o is JContainer c2)
        {
          foreach (JToken d in c2.Descendants())
            yield return d;
        }
      }
    }

    internal bool IsMultiContent(object content) => content is IEnumerable && !(content is string) && !(content is JToken) && !(content is byte[]);

    internal JToken EnsureParentToken(JToken item, bool skipParentCheck)
    {
      if (item == null)
        return (JToken) JValue.CreateNull();
      if (skipParentCheck || item.Parent == null && item != this && (!item.HasValues || this.Root != item))
        return item;
      item = item.CloneToken();
      return item;
    }

    internal int IndexOfItem(JToken item) => this.ChildrenTokens.IndexOf<JToken>(item, (IEqualityComparer<JToken>) JContainer.JTokenReferenceEqualityComparer.Instance);

    internal virtual void InsertItem(int index, JToken item, bool skipParentCheck)
    {
      if (index > this.ChildrenTokens.Count)
        throw new ArgumentOutOfRangeException(nameof (index), "Index must be within the bounds of the List.");
      this.CheckReentrancy();
      item = this.EnsureParentToken(item, skipParentCheck);
      JToken jtoken1 = index == 0 ? (JToken) null : this.ChildrenTokens[index - 1];
      JToken jtoken2 = index == this.ChildrenTokens.Count ? (JToken) null : this.ChildrenTokens[index];
      this.ValidateToken(item, (JToken) null);
      item.Parent = this;
      item.Previous = jtoken1;
      if (jtoken1 != null)
        jtoken1.Next = item;
      item.Next = jtoken2;
      if (jtoken2 != null)
        jtoken2.Previous = item;
      this.ChildrenTokens.Insert(index, item);
      if (this._listChanged != null)
        this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
      if (this._collectionChanged == null)
        return;
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (object) item, index));
    }

    internal virtual void RemoveItemAt(int index)
    {
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index), "Index is less than 0.");
      if (index >= this.ChildrenTokens.Count)
        throw new ArgumentOutOfRangeException(nameof (index), "Index is equal to or greater than Count.");
      this.CheckReentrancy();
      JToken childrenToken = this.ChildrenTokens[index];
      JToken jtoken1 = index == 0 ? (JToken) null : this.ChildrenTokens[index - 1];
      JToken jtoken2 = index == this.ChildrenTokens.Count - 1 ? (JToken) null : this.ChildrenTokens[index + 1];
      if (jtoken1 != null)
        jtoken1.Next = jtoken2;
      if (jtoken2 != null)
        jtoken2.Previous = jtoken1;
      childrenToken.Parent = (JContainer) null;
      childrenToken.Previous = (JToken) null;
      childrenToken.Next = (JToken) null;
      this.ChildrenTokens.RemoveAt(index);
      if (this._listChanged != null)
        this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
      if (this._collectionChanged == null)
        return;
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, (object) childrenToken, index));
    }

    internal virtual bool RemoveItem(JToken item)
    {
      int index = this.IndexOfItem(item);
      if (index < 0)
        return false;
      this.RemoveItemAt(index);
      return true;
    }

    internal virtual JToken GetItem(int index) => this.ChildrenTokens[index];

    internal virtual void SetItem(int index, JToken item)
    {
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index), "Index is less than 0.");
      if (index >= this.ChildrenTokens.Count)
        throw new ArgumentOutOfRangeException(nameof (index), "Index is equal to or greater than Count.");
      JToken childrenToken = this.ChildrenTokens[index];
      if (JContainer.IsTokenUnchanged(childrenToken, item))
        return;
      this.CheckReentrancy();
      item = this.EnsureParentToken(item, false);
      this.ValidateToken(item, childrenToken);
      JToken jtoken1 = index == 0 ? (JToken) null : this.ChildrenTokens[index - 1];
      JToken jtoken2 = index == this.ChildrenTokens.Count - 1 ? (JToken) null : this.ChildrenTokens[index + 1];
      item.Parent = this;
      item.Previous = jtoken1;
      if (jtoken1 != null)
        jtoken1.Next = item;
      item.Next = jtoken2;
      if (jtoken2 != null)
        jtoken2.Previous = item;
      this.ChildrenTokens[index] = item;
      childrenToken.Parent = (JContainer) null;
      childrenToken.Previous = (JToken) null;
      childrenToken.Next = (JToken) null;
      if (this._listChanged != null)
        this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
      if (this._collectionChanged == null)
        return;
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, (object) item, (object) childrenToken, index));
    }

    internal virtual void ClearItems()
    {
      this.CheckReentrancy();
      foreach (JToken childrenToken in (IEnumerable<JToken>) this.ChildrenTokens)
      {
        childrenToken.Parent = (JContainer) null;
        childrenToken.Previous = (JToken) null;
        childrenToken.Next = (JToken) null;
      }
      this.ChildrenTokens.Clear();
      if (this._listChanged != null)
        this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
      if (this._collectionChanged == null)
        return;
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    internal virtual void ReplaceItem(JToken existing, JToken replacement)
    {
      if (existing == null || existing.Parent != this)
        return;
      this.SetItem(this.IndexOfItem(existing), replacement);
    }

    internal virtual bool ContainsItem(JToken item) => this.IndexOfItem(item) != -1;

    internal virtual void CopyItemsTo(Array array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (arrayIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (arrayIndex), "arrayIndex is less than 0.");
      if (arrayIndex >= array.Length && arrayIndex != 0)
        throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
      if (this.Count > array.Length - arrayIndex)
        throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
      int num = 0;
      foreach (JToken childrenToken in (IEnumerable<JToken>) this.ChildrenTokens)
      {
        array.SetValue((object) childrenToken, arrayIndex + num);
        ++num;
      }
    }

    internal static bool IsTokenUnchanged(JToken currentValue, JToken newValue)
    {
      if (!(currentValue is JValue jvalue))
        return false;
      return jvalue.Type == JTokenType.Null && newValue == null || jvalue.Equals((object) newValue);
    }

    internal virtual void ValidateToken(JToken o, JToken existing)
    {
      ValidationUtils.ArgumentNotNull((object) o, nameof (o));
      if (o.Type == JTokenType.Property)
        throw new ArgumentException("Can not add {0} to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) o.GetType(), (object) this.GetType()));
    }

    public virtual void Add(object content) => this.AddInternal(this.ChildrenTokens.Count, content, false);

    internal void AddAndSkipParentCheck(JToken token) => this.AddInternal(this.ChildrenTokens.Count, (object) token, true);

    public void AddFirst(object content) => this.AddInternal(0, content, false);

    internal void AddInternal(int index, object content, bool skipParentCheck)
    {
      if (this.IsMultiContent(content))
      {
        IEnumerable enumerable = (IEnumerable) content;
        int index1 = index;
        foreach (object content1 in enumerable)
        {
          this.AddInternal(index1, content1, skipParentCheck);
          ++index1;
        }
      }
      else
      {
        JToken fromContent = JContainer.CreateFromContent(content);
        this.InsertItem(index, fromContent, skipParentCheck);
      }
    }

    internal static JToken CreateFromContent(object content) => content is JToken ? (JToken) content : (JToken) new JValue(content);

    public JsonWriter CreateWriter() => (JsonWriter) new JTokenWriter(this);

    public void ReplaceAll(object content)
    {
      this.ClearItems();
      this.Add(content);
    }

    public void RemoveAll() => this.ClearItems();

    internal abstract void MergeItem(object content, JsonMergeSettings settings);

    public void Merge(object content) => this.MergeItem(content, new JsonMergeSettings());

    public void Merge(object content, JsonMergeSettings settings) => this.MergeItem(content, settings);

    internal void ReadTokenFrom(JsonReader reader)
    {
      int depth = reader.Depth;
      if (!reader.Read())
        throw JsonReaderException.Create(reader, "Error reading {0} from JsonReader.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType().Name));
      this.ReadContentFrom(reader);
      if (reader.Depth > depth)
        throw JsonReaderException.Create(reader, "Unexpected end of content while loading {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType().Name));
    }

    internal void ReadContentFrom(JsonReader r)
    {
      ValidationUtils.ArgumentNotNull((object) r, nameof (r));
      IJsonLineInfo lineInfo = r as IJsonLineInfo;
      JContainer jcontainer = this;
      do
      {
        if (jcontainer is JProperty && ((JProperty) jcontainer).Value != null)
        {
          if (jcontainer == this)
            break;
          jcontainer = jcontainer.Parent;
        }
        switch (r.TokenType)
        {
          case JsonToken.None:
            continue;
          case JsonToken.StartObject:
            JObject jobject = new JObject();
            jobject.SetLineInfo(lineInfo);
            jcontainer.Add((object) jobject);
            jcontainer = (JContainer) jobject;
            goto case JsonToken.None;
          case JsonToken.StartArray:
            JArray jarray = new JArray();
            jarray.SetLineInfo(lineInfo);
            jcontainer.Add((object) jarray);
            jcontainer = (JContainer) jarray;
            goto case JsonToken.None;
          case JsonToken.StartConstructor:
            JConstructor jconstructor = new JConstructor(r.Value.ToString());
            jconstructor.SetLineInfo(lineInfo);
            jcontainer.Add((object) jconstructor);
            jcontainer = (JContainer) jconstructor;
            goto case JsonToken.None;
          case JsonToken.PropertyName:
            string name = r.Value.ToString();
            JProperty jproperty1 = new JProperty(name);
            jproperty1.SetLineInfo(lineInfo);
            JProperty jproperty2 = ((JObject) jcontainer).Property(name);
            if (jproperty2 == null)
              jcontainer.Add((object) jproperty1);
            else
              jproperty2.Replace((JToken) jproperty1);
            jcontainer = (JContainer) jproperty1;
            goto case JsonToken.None;
          case JsonToken.Comment:
            JValue comment = JValue.CreateComment(r.Value.ToString());
            comment.SetLineInfo(lineInfo);
            jcontainer.Add((object) comment);
            goto case JsonToken.None;
          case JsonToken.Integer:
          case JsonToken.Float:
          case JsonToken.String:
          case JsonToken.Boolean:
          case JsonToken.Date:
          case JsonToken.Bytes:
            JValue jvalue1 = new JValue(r.Value);
            jvalue1.SetLineInfo(lineInfo);
            jcontainer.Add((object) jvalue1);
            goto case JsonToken.None;
          case JsonToken.Null:
            JValue jvalue2 = JValue.CreateNull();
            jvalue2.SetLineInfo(lineInfo);
            jcontainer.Add((object) jvalue2);
            goto case JsonToken.None;
          case JsonToken.Undefined:
            JValue undefined = JValue.CreateUndefined();
            undefined.SetLineInfo(lineInfo);
            jcontainer.Add((object) undefined);
            goto case JsonToken.None;
          case JsonToken.EndObject:
            if (jcontainer == this)
              return;
            jcontainer = jcontainer.Parent;
            goto case JsonToken.None;
          case JsonToken.EndArray:
            if (jcontainer == this)
              return;
            jcontainer = jcontainer.Parent;
            goto case JsonToken.None;
          case JsonToken.EndConstructor:
            if (jcontainer == this)
              return;
            jcontainer = jcontainer.Parent;
            goto case JsonToken.None;
          default:
            throw new InvalidOperationException("The JsonReader should not be on a token of type {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) r.TokenType));
        }
      }
      while (r.Read());
    }

    internal int ContentsHashCode()
    {
      int num = 0;
      foreach (JToken childrenToken in (IEnumerable<JToken>) this.ChildrenTokens)
        num ^= childrenToken.GetDeepHashCode();
      return num;
    }

    string ITypedList.GetListName(PropertyDescriptor[] listAccessors) => string.Empty;

    PropertyDescriptorCollection ITypedList.GetItemProperties(
      PropertyDescriptor[] listAccessors)
    {
      return this.First is ICustomTypeDescriptor first ? first.GetProperties() : (PropertyDescriptorCollection) null;
    }

    int IList<JToken>.IndexOf(JToken item) => this.IndexOfItem(item);

    void IList<JToken>.Insert(int index, JToken item) => this.InsertItem(index, item, false);

    void IList<JToken>.RemoveAt(int index) => this.RemoveItemAt(index);

    JToken IList<JToken>.this[int index]
    {
      get => this.GetItem(index);
      set => this.SetItem(index, value);
    }

    void ICollection<JToken>.Add(JToken item) => this.Add((object) item);

    void ICollection<JToken>.Clear() => this.ClearItems();

    bool ICollection<JToken>.Contains(JToken item) => this.ContainsItem(item);

    void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex) => this.CopyItemsTo((Array) array, arrayIndex);

    bool ICollection<JToken>.IsReadOnly => false;

    bool ICollection<JToken>.Remove(JToken item) => this.RemoveItem(item);

    private JToken EnsureValue(object value)
    {
      if (value == null)
        return (JToken) null;
      return value is JToken ? (JToken) value : throw new ArgumentException("Argument is not a JToken.");
    }

    int IList.Add(object value)
    {
      this.Add((object) this.EnsureValue(value));
      return this.Count - 1;
    }

    void IList.Clear() => this.ClearItems();

    bool IList.Contains(object value) => this.ContainsItem(this.EnsureValue(value));

    int IList.IndexOf(object value) => this.IndexOfItem(this.EnsureValue(value));

    void IList.Insert(int index, object value) => this.InsertItem(index, this.EnsureValue(value), false);

    bool IList.IsFixedSize => false;

    bool IList.IsReadOnly => false;

    void IList.Remove(object value) => this.RemoveItem(this.EnsureValue(value));

    void IList.RemoveAt(int index) => this.RemoveItemAt(index);

    object IList.this[int index]
    {
      get => (object) this.GetItem(index);
      set => this.SetItem(index, this.EnsureValue(value));
    }

    void ICollection.CopyTo(Array array, int index) => this.CopyItemsTo(array, index);

    public int Count => this.ChildrenTokens.Count;

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot
    {
      get
      {
        if (this._syncRoot == null)
          Interlocked.CompareExchange(ref this._syncRoot, new object(), (object) null);
        return this._syncRoot;
      }
    }

    void IBindingList.AddIndex(PropertyDescriptor property)
    {
    }

    object IBindingList.AddNew()
    {
      AddingNewEventArgs e = new AddingNewEventArgs();
      this.OnAddingNew(e);
      if (e.NewObject == null)
        throw new JsonException("Could not determine new value to add to '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
      JToken jtoken = e.NewObject is JToken ? (JToken) e.NewObject : throw new JsonException("New item to be added to collection must be compatible with {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (JToken)));
      this.Add((object) jtoken);
      return (object) jtoken;
    }

    bool IBindingList.AllowEdit => true;

    bool IBindingList.AllowNew => true;

    bool IBindingList.AllowRemove => true;

    void IBindingList.ApplySort(
      PropertyDescriptor property,
      ListSortDirection direction)
    {
      throw new NotSupportedException();
    }

    int IBindingList.Find(PropertyDescriptor property, object key) => throw new NotSupportedException();

    bool IBindingList.IsSorted => false;

    void IBindingList.RemoveIndex(PropertyDescriptor property)
    {
    }

    void IBindingList.RemoveSort() => throw new NotSupportedException();

    ListSortDirection IBindingList.SortDirection => ListSortDirection.Ascending;

    PropertyDescriptor IBindingList.SortProperty => (PropertyDescriptor) null;

    bool IBindingList.SupportsChangeNotification => true;

    bool IBindingList.SupportsSearching => false;

    bool IBindingList.SupportsSorting => false;

    internal static void MergeEnumerableContent(
      JContainer target,
      IEnumerable content,
      JsonMergeSettings settings)
    {
      switch (settings.MergeArrayHandling)
      {
        case MergeArrayHandling.Concat:
          IEnumerator enumerator1 = content.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              JToken current = (JToken) enumerator1.Current;
              target.Add((object) current);
            }
            break;
          }
          finally
          {
            if (enumerator1 is IDisposable disposable12)
              disposable12.Dispose();
          }
        case MergeArrayHandling.Union:
          HashSet<JToken> jtokenSet = new HashSet<JToken>((IEnumerable<JToken>) target, (IEqualityComparer<JToken>) JToken.EqualityComparer);
          IEnumerator enumerator2 = content.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              JToken current = (JToken) enumerator2.Current;
              if (jtokenSet.Add(current))
                target.Add((object) current);
            }
            break;
          }
          finally
          {
            if (enumerator2 is IDisposable disposable13)
              disposable13.Dispose();
          }
        case MergeArrayHandling.Replace:
          target.ClearItems();
          IEnumerator enumerator3 = content.GetEnumerator();
          try
          {
            while (enumerator3.MoveNext())
            {
              JToken current = (JToken) enumerator3.Current;
              target.Add((object) current);
            }
            break;
          }
          finally
          {
            if (enumerator3 is IDisposable disposable14)
              disposable14.Dispose();
          }
        case MergeArrayHandling.Merge:
          int num = 0;
          IEnumerator enumerator4 = content.GetEnumerator();
          try
          {
            while (enumerator4.MoveNext())
            {
              object current = enumerator4.Current;
              if (num < target.Count)
              {
                if (target[(object) num] is JContainer jcontainer7)
                  jcontainer7.Merge(current, settings);
                else if (current != null)
                {
                  JToken fromContent = JContainer.CreateFromContent(current);
                  if (fromContent.Type != JTokenType.Null)
                    target[(object) num] = fromContent;
                }
              }
              else
                target.Add(current);
              ++num;
            }
            break;
          }
          finally
          {
            if (enumerator4 is IDisposable disposable15)
              disposable15.Dispose();
          }
        default:
          throw new ArgumentOutOfRangeException(nameof (settings), "Unexpected merge array handling when merging JSON.");
      }
    }

    private class JTokenReferenceEqualityComparer : IEqualityComparer<JToken>
    {
      public static readonly JContainer.JTokenReferenceEqualityComparer Instance = new JContainer.JTokenReferenceEqualityComparer();

      public bool Equals(JToken x, JToken y) => object.ReferenceEquals((object) x, (object) y);

      public int GetHashCode(JToken obj) => obj == null ? 0 : obj.GetHashCode();
    }
  }
}
