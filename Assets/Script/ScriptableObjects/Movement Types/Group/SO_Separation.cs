using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Separation Settings File", menuName = "Scriptable Objects/IA/Group Behaviours/Separation")]
public class SO_Separation : GroupBehaviour
{
    public float maxDistance = 1;

    public override Vector3 CalculateMovement(TravellingAgent[] partners, TravellingAgent owner, Vector3 target)
    {
        Vector3 result = Vector3.zero;
        if (partners.Length - 1 <= 0) return result;

        for (int i = 0; i < partners.Length; i++)
        {
            if (partners[i] != owner)
            {
                var dist = owner.transform.position - partners[i].transform.position;
                if (dist.magnitude > maxDistance) result += Vector3.zero;
                else result += dist;
            }
        }

        result /= partners.Length - 1;
        return new Vector3(result.x, 0, result.z) * priority;
    }
}
