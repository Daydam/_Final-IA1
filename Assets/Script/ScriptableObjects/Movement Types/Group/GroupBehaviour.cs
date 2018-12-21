using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupBehaviour : BehaviourBase
{
    public virtual Vector3 CalculateMovement(TravellingAgent[] partners, TravellingAgent owner, Vector3 target)
    {
        return Vector3.zero;
    }

    public virtual float CalculateRotation(TravellingAgent[] partners, float maxTurningSpeed)
    {
        return 0;
    }
}
