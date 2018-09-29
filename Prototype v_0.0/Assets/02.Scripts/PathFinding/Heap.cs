using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    T[] heap;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        heap = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        heap[currentItemCount] = item;

        SortUp(item);
        currentItemCount++;
    }
    public T RemoveFirst()
    {
        T firstItem = heap[0];
        currentItemCount--;
        heap[0] = heap[currentItemCount];
        heap[0].HeapIndex = 0;

        SortDown(heap[0]);

        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }
    public int Count
    {
        get { return currentItemCount; }
        
    }
    public bool Contains(T item)
    {
        return Equals(heap[item.HeapIndex], item);
    }
    


    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount)
                    if (heap[childIndexLeft].CompareTo(heap[childIndexRight])<0)//0보다 작으면 앞에 순서, 0이면 동일, 0보다 크면 뒤에순서임
                        swapIndex = childIndexRight;

                if (item.CompareTo(heap[swapIndex]) < 0)
                    Swap(item, heap[swapIndex]);
                else
                    return;
            }
            else
                return;

        }
    }
    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while(true)
        {
            T parentItem = heap[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
                break;

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        heap[itemA.HeapIndex] = itemB;
        heap[itemB.HeapIndex] = itemA;

        int itemAIndex = itemA.HeapIndex;

        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}