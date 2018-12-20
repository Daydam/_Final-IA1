using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OA Settings File", menuName = "Scriptable Objects/IA/Obstacle Avoidance/OA Settings File")]
public class SO_ObstacleAvoidance : SteeringBehaviour
{
    public float rayDistance;
    public AnimationCurve intensityCurve;
    public float maxTurningSpeed;
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

    public override float CalculateRotation()
    {
        //For some reason this works every 2 launches. Print the rays to check them!
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
                turningAngle = maxTurningSpeed * intensityCurve.Evaluate(Vector3.Distance(entityTransform.position, leftHit.point) / rayDistance);
            }
            else
            {
                turningAngle = -maxTurningSpeed * intensityCurve.Evaluate(Vector3.Distance(entityTransform.position, rightHit.point) / rayDistance);
            }
            Debug.Log("hit both");
        }
        else if (Physics.Raycast(leftRay, out leftHit, rayDistance, layerMask))
        {
            turningAngle = maxTurningSpeed * intensityCurve.Evaluate(Vector3.Distance(entityTransform.position, leftHit.point) / rayDistance);
            Debug.Log("hit left");
        }
        else if (Physics.Raycast(rightRay, out rightHit, rayDistance, layerMask))
        {
            turningAngle = -maxTurningSpeed * intensityCurve.Evaluate(Vector3.Distance(entityTransform.position, rightHit.point) / rayDistance);
            Debug.Log("hit right");
        }
        Debug.Log(turningAngle);
        return turningAngle;
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
