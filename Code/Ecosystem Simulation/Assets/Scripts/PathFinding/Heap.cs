using System; // for IComparable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Heap represented using array
public class Heap<T> where T : IHeapItem<T> 
{
    
    protected T[] items;
    protected int currentItemCount;
    public int Count { get { return currentItemCount; } }//number of items currently in heap

    // difficult to resize array so determine max size at creation
    public Heap(int maxHeapSize) 
    {
        items = new T[maxHeapSize];
    }

    //currently only should be used if increased priority (if to be made more generic SortDown should be called)
    public void UpdateItem(T item) 
    {
        SortUp(item);
    }

    //check heap contains specific item
    public bool Contains(T item) 
    {
        return Equals(items[item.HeapIndex], item);
    }

    // adds new item to a heap
    public void Add(T item) 
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;// add to end of array
        SortUp(item); 
        currentItemCount++;
    }

    // removes first item from the heap
    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount]; //take item at end, put at start
        items[0].HeapIndex = 0;
        SortDown(items[0]); //sort heap
        return firstItem;
    }

    //keeps comparing with parent and swaping until in correct place
    void SortUp(T item) 
    {
        int parentIndex = (item.HeapIndex-1)/2;
        
        while (true) 
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0) //CompareTo higher 1, equal 0, lower -1
            {
                Swap(item,parentItem);
            }
            else 
            {
                break;
            }

            parentIndex = (item.HeapIndex-1)/2;
        }
    }

    // sorts heap down
    void SortDown(T item) 
    {
        while (true) 
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            // check if has a child
            if (childIndexLeft < currentItemCount) 
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount) 
                {
                    //check which child has higher priority
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) 
                    {
                        swapIndex = childIndexRight;
                    }
                }

                //check if parent has lower priority
                if (item.CompareTo(items[swapIndex]) < 0) 
                {
                    Swap(item,items[swapIndex]);
                }
                else// parent has higher priority so correct position
                {
                    return;
                }
            }
            else //parent has no children so correct position
            {
                return;
            }
        }
    }

    // swaps item positions in array
    void Swap(T itemA, T itemB) 
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex; //temp variable for swapping
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}


public interface IHeapItem<T> : IComparable<T> 
{
    int HeapIndex { get; set; }
}
