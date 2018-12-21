using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cohesion Settings File", menuName = "Scriptable Objects/IA/Group Behaviours/Cohesion")]
public class SO_Cohesion : GroupBehaviour
{
    public override Vector3 CalculateMovement(TravellingAgent[] partners, TravellingAgent owner, Vector3 target)
    {
        Vector3 center = new Vector3(0, owner.transform.position.y, 0);

        foreach (TravellingAgent a in partners)
        {
            center += new Vector3(a.transform.position.x, 0, a.transform.position.z);
        }

        center /= partners.Length;
        var dist = target - center;

        return  new Vector3(dist.x, 0, dist.z)* priority;
    }
}
