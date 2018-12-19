using UnityEngine;
using System.Collections.Generic;
using System;

public class PriorityQueue<T>
{
    List<Arc<T>> queue = new List<Arc<T>>();

    public void Enqueue(Arc<T> element)
    {
        queue.Add(element);
    }

    public Arc<T> Dequeue()
    {
        var min = default(Arc<T>);
        var minDist = float.MaxValue;

        foreach (var element in queue)
        {
            if (element.Weight < minDist)
            {
                min = element;
                minDist = element.Weight;
            }
        }

        var newQueue = new List<Arc<T>>();

        foreach (var element in queue) { if (element != min) newQueue.Add(element); }

        queue = newQueue;

        return min;
    }

    public bool IsEmpty { get { return queue.Count == 0; } }
}
