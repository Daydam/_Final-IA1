using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RayConfig
{
    public Vector3 angle;
    public float maxDistance;
    public AnimationCurve intensity;
    public AnimationCurve rotationIntensity;

    float currentDistance;
    public float CurrentDistance { get { return currentDistance; } }

    public Vector3 GetEndingPoint(Transform t)
    {
        return t.position + GetAngle(t) * currentDistance;
    }

    public Vector3 GetAngle(Transform t)
    {
        Vector3 globalAngle = t.forward * angle.z + t.right * angle.x + t.up * angle.y;
        return globalAngle.normalized;
    }

    public float GetIntensity(float dist)
    {
        return Mathf.Clamp01(intensity.Evaluate(dist));
    }

    public float GetRotationIntensity(float dist)
    {
        return Mathf.Clamp01(rotationIntensity.Evaluate(dist));
    }

    public void SetDistance(float currentSpeed)
    {
        currentDistance = maxDistance * currentSpeed;
    }
}

[CreateAssetMenu(fileName = "OA Settings File", menuName = "Scriptable Objects/IA/Obstacle Avoidance/OA Settings File")]
public class ObstacleAvoidance : SteeringBehaviour
{
    public float minimumTension;
    public RayConfig[] rayConfigs;

    override public Vector3 CalculateMovement(Vector3 target)
    {
        Vector3 movementToAdd = new Vector3();
        for (int i = 0; i < rayConfigs.Length; i++)
        {
            //This is set like this to make it less clumsy while I work on other stuff, it should really be currentSpeed instead of 1
            rayConfigs[i].SetDistance(1);

            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(entityTransform.position, rayConfigs[i].GetAngle(entityTransform), out hit, rayConfigs[i].CurrentDistance))
            {
                float dist = hit.distance / rayConfigs[i].CurrentDistance;
                movementToAdd -= rayConfigs[i].angle.normalized * rayConfigs[i].GetIntensity(dist);
            }
        }
        return movementToAdd;
    }

    public override float CalculateRotation()
    {
        float rotation = 0;
        for (int i = 0; i < rayConfigs.Length; i++)
        {
            //This is set like this to make it less clumsy while I work on other stuff, it should really be currentSpeed instead of 1
            rayConfigs[i].SetDistance(1);

            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(entityTransform.position, rayConfigs[i].GetAngle(entityTransform), out hit, rayConfigs[i].CurrentDistance))
            {
                float dist = hit.distance / rayConfigs[i].CurrentDistance;
                rotation += Vector3.SignedAngle(rayConfigs[i].GetAngle(entityTransform).normalized, entityTransform.forward, Vector3.up) * rayConfigs[i].GetRotationIntensity(dist);
            }
        }
        return rotation;
    }

    override public void GizmoDraw()
    {
        if (Application.isPlaying)
        {
            for (int i = 0; i < rayConfigs.Length; i++)
            {
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(entityTransform.position, rayConfigs[i].GetAngle(entityTransform), out hit, rayConfigs[i].CurrentDistance))
                {
                    float dist = hit.distance / rayConfigs[i].CurrentDistance;
                    Gizmos.color = Color.Lerp(Color.green, Color.red, rayConfigs[i].GetIntensity(dist));
                }
                else
                {
                    Gizmos.color = Color.green;
                }
                Gizmos.DrawLine(entityTransform.position, rayConfigs[i].GetEndingPoint(entityTransform));
            }
        }
    }
}
