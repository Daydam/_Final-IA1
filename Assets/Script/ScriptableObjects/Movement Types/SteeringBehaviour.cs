using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviour : ScriptableObject
{
	protected Transform entityTransform;
    public float priority;
    public float Priority { get { return priority; } }
	public bool allowsRotation;

	public virtual void RegisterEntity(Transform t)
	{
		entityTransform = t;
	}

	public virtual Vector3 CalculateMovement(Vector3 target)
	{
		return Vector3.zero;
	}

	public virtual float CalculateRotation(Vector3 target, float maxTurningSpeed)
	{
		return 0;
	}

	public virtual void GizmoDraw()
	{

	}
}
