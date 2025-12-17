using System;
using System.Collections;
using System.Collections.Generic;

public class ObservableList<T> : IList<T>
{
    private readonly List<T> innerList = new();

    public event Action<T> OnItemAdded;
    public event Action<T> OnItemRemoved;
    public event Action OnListCleared;
    public event Action OnListChanged;

    public T this[int index]
    {
        get => innerList[index];
        set
        {
            innerList[index] = value;
            OnListChanged?.Invoke();
        }
    }

    public int Count => innerList.Count;
    public bool IsReadOnly => false;

    public void Add(T item)
    {
        innerList.Add(item);
        OnItemAdded?.Invoke(item);
        OnListChanged?.Invoke();
    }

    public bool Remove(T item)
    {
        bool removed = innerList.Remove(item);
        if (removed)
        {
            OnItemRemoved?.Invoke(item);
            OnListChanged?.Invoke();
        }
        return removed;
    }

    public void Clear()
    {
        innerList.Clear();
        OnListCleared?.Invoke();
        OnListChanged?.Invoke();
    }

    // Forward other List methods
    public bool Contains(T item) => innerList.Contains(item);
    public void CopyTo(T[] array, int arrayIndex) => innerList.CopyTo(array, arrayIndex);
    public IEnumerator<T> GetEnumerator() => innerList.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => innerList.GetEnumerator();
    public int IndexOf(T item) => innerList.IndexOf(item);
    public void Insert(int index, T item)
    {
        innerList.Insert(index, item);
        OnItemAdded?.Invoke(item);
        OnListChanged?.Invoke();
    }

    public void RemoveAt(int index)
    {
        T removed = innerList[index];
        innerList.RemoveAt(index);
        OnItemRemoved?.Invoke(removed);
        OnListChanged?.Invoke();
    }
}