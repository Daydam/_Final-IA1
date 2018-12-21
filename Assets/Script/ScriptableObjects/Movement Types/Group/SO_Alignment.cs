using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Alignment Settings File", menuName = "Scriptable Objects/IA/Group Behaviours/Alignment")]
public class SO_Alignment : GroupBehaviour
{
    public override Vector3 CalculateMovement(TravellingAgent[] partners, TravellingAgent owner, Vector3 target)
    {
        Vector3 result = Vector3.zero;
        if (partners.Length - 1 <= 0) return result;

        for (int i = 0; i < partners.Length; i++)
        {
            if(partners[i] != owner) result += partners[i].transform.position;
        }

        result /= partners.Length - 1;
        var distToMove = result - owner.transform.position;
        return new Vector3(distToMove.x, 0, distToMove.z) * priority;
    }
}
