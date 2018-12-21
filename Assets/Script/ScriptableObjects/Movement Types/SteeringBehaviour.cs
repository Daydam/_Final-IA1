using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviour : BehaviourBase
{
	public virtual Vector3 CalculateMovement(Vector3 target)
	{
		return Vector3.zero;
	}

    public virtual float CalculateRotation(Vector3 target, float maxTurningSpeed)
	{
		return 0;
	}
}
