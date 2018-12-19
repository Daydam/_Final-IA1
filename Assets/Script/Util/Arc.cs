using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Arc<T>
{
    public T element;
    public float weight;

    public T Element { get { return element; } }
    public float Weight { get { return weight; } }

    public Arc(T element, float weight)
    {
        this.element = element;
        this.weight = weight;
    }
}
