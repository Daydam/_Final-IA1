using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourBase : ScriptableObject
{
    protected Transform entityTransform;
    public float priority = 1;
    public float Priority { get { return priority; } }
    public bool allowsRotation;

    public virtual void RegisterEntity(Transform t)
    {
        entityTransform = t;
    }

    public virtual void GizmoDraw()
    {

    }
}
