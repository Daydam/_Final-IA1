using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellingAgent : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    public SteeringBehaviour[] steeringBehaviours;

    void Start()
    {
        //EventManager.Instance.Subscribe(EventID.SCENE_CHANGED, OnSceneChanged);

        for (int i = 0; i < steeringBehaviours.Length; i++)
        {
            steeringBehaviours[i].RegisterEntity(transform);
        }
    }

    public void Walk()
    {
        var nextNode = path.Peek();
        var nextPos = new Vector3(nextNode.Position.x, transform.position.y, nextNode.Position.z);

        float presenceFactor = path.Count > 1 ? 0.4f : 0.1f;

        if ((nextPos - transform.position).magnitude < Mathf.Min(DungeonGenerator.Instance.prefab.size.x, DungeonGenerator.Instance.prefab.size.y) * presenceFactor)
        {
            //transform.position = nextPos;
            Current = path.Pop();
        }
        else
        {
            Vector3 movementModifier = new Vector3();
            float rotationModifier = 0;

            //movementModifier += (nextPos - transform.position).normalized * mainMovementPriority;
            //movementModifier += transform.forward * mainMovementPriority;
            //rotationModifier += Vector3.SignedAngle(transform.forward, (nextPos - transform.position).normalized, Vector3.up);
            int rotationModAmount = 0;

            for (int i = 0; i < steeringBehaviours.Length; i++)
            {
                movementModifier += steeringBehaviours[i].CalculateMovement(new Vector3(nextPos.x, transform.position.y, nextPos.z));
                float rotationAmount = steeringBehaviours[i].CalculateRotation(nextPos, rotationSpeed);
                if (Mathf.Abs(rotationAmount) != 0)
                {
                    rotationModifier += rotationAmount;
                    rotationModAmount++;
                }
            }
            //Saco promedio o no? mmm...
            //rotationModifier /= rotationModAmount;

            movementModifier.Normalize();
            transform.RotateAround(transform.position, Vector3.up, Mathf.Sign(rotationModifier) * Mathf.Min(Mathf.Abs(rotationModifier), rotationSpeed) * rotationSpeed * Time.deltaTime);
            transform.position += movementModifier * movementSpeed * Time.deltaTime;
        }
    }
}
