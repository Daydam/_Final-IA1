using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Seek Settings File", menuName = "Scriptable Objects/IA/Steering Behaviours/Seek")]
public class SO_Seek : SteeringBehaviour
{
    public override Vector3 CalculateMovement(Vector3 target)
    {
        return entityTransform.forward * priority;
    }

    public override float CalculateRotation(Vector3 target, float maxTurningSpeed)
    {
        return Vector3.SignedAngle(entityTransform.forward, target - entityTransform.position, Vector3.up) * priority / 360;
    }
}
