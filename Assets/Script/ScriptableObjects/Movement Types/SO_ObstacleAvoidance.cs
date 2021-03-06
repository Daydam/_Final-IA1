using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OA Settings File", menuName = "Scriptable Objects/IA/Steering Behaviours/Obstacle Avoidance")]
public class SO_ObstacleAvoidance : SteeringBehaviour
{
    public float rayDistance;
    public AnimationCurve intensityCurve;
    Ray leftRay;
    Ray rightRay;
    int layerMask = 1 << 10;
    Collider col;

    public override void RegisterEntity(Transform t)
    {
        base.RegisterEntity(t);
        col = entityTransform.GetComponent<Collider>();

        layerMask = ~layerMask;
        //Print this mask to check it works,maybe? Idk, something's wrong with the activation of this script, it seems.
    }

    override public Vector3 CalculateMovement(Vector3 target)
    {
        return Vector3.zero;
    }

    public override float CalculateRotation(Vector3 target, float maxTurningSpeed)
    {
        float turningAngle = 0;

        leftRay = new Ray(entityTransform.position - entityTransform.right * (col.bounds.size.x / 2),
            entityTransform.forward);
        rightRay = new Ray(entityTransform.position + entityTransform.right * (col.bounds.size.x / 2),
            entityTransform.forward);

        RaycastHit leftHit = new RaycastHit();
        RaycastHit rightHit = new RaycastHit();
        if (Physics.Raycast(leftRay, out leftHit, rayDistance, layerMask) && Physics.Raycast(rightRay, out rightHit, rayDistance, layerMask))
        {
            if (Vector3.Distance(entityTransform.position, leftHit.point) / rayDistance < Vector3.Distance(entityTransform.position, rightHit.point) / rayDistance)
            {
                turningAngle = intensityCurve.Evaluate(Vector3.Distance(entityTransform.position, leftHit.point) / rayDistance);
            }
            else
            {
                turningAngle = -intensityCurve.Evaluate(Vector3.Distance(entityTransform.position, rightHit.point) / rayDistance);
            }
        }
        else if (Physics.Raycast(leftRay, out leftHit, rayDistance, layerMask))
        {
            turningAngle = intensityCurve.Evaluate(Vector3.Distance(entityTransform.position, leftHit.point) / rayDistance);
        }
        else if (Physics.Raycast(rightRay, out rightHit, rayDistance, layerMask))
        {
            turningAngle = -intensityCurve.Evaluate(Vector3.Distance(entityTransform.position, rightHit.point) / rayDistance);
        }
        return turningAngle * priority;
    }

    override public void GizmoDraw()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Physics.Raycast(rightRay, rayDistance) ? Color.red : Color.green;
            Gizmos.DrawLine(entityTransform.position + entityTransform.right * (col.bounds.size.x / 2),
                entityTransform.position + entityTransform.right * (col.bounds.size.x / 2) + entityTransform.forward * rayDistance);

            Gizmos.color = Physics.Raycast(leftRay, rayDistance) ? Color.red : Color.green;
            Gizmos.DrawLine(entityTransform.position - entityTransform.right * (col.bounds.size.x / 2),
                entityTransform.position - entityTransform.right * (col.bounds.size.x / 2) + entityTransform.forward * rayDistance);
        }
    }
}
