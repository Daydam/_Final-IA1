using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellingAgent : MonoBehaviour
{
    Group group;
    public bool followLeader;
    public float movementSpeed;
    public float rotationSpeed;
    //Por cuestiones de hardcodeo cósmico, el primero de estos tiene que ser el steering behaviour principal.
    public BehaviourBase[] behaviours;

    void Start()
    {
        for (int i = 0; i < behaviours.Length; i++)
        {
            behaviours[i] = Instantiate(behaviours[i]);
            behaviours[i].RegisterEntity(transform);
        }
    }

    void FixedUpdate()
    {
        if (followLeader)
        {
            if ((group.Leader.transform.position - transform.position).magnitude > Mathf.Min(DungeonGenerator.Instance.prefab.size.x, DungeonGenerator.Instance.prefab.size.y) * 0.2f)
            {
                Walk(group.Leader.transform.position);
            }
        }
        else if (group.Leader.Path != default(Stack<RoomNode>) && group.Leader.Path.Count > 0)
        {
            var nextNode = group.Leader.Path.Peek();
            var nextPos = new Vector3(nextNode.Position.x, transform.position.y, nextNode.Position.z);
            float presenceFactor = group.Leader.Path.Count > 1 ? 0.4f : 0.2f;

            if ((nextPos - transform.position).magnitude > Mathf.Min(DungeonGenerator.Instance.prefab.size.x, DungeonGenerator.Instance.prefab.size.y) * presenceFactor)
            {
                Walk(nextPos);
            }
        }
    }

    public void Walk(Vector3 target)
    {
        Vector3 movementModifier = new Vector3();
        float rotationModifier = 0;

        //movementModifier += (nextPos - transform.position).normalized * mainMovementPriority;
        //movementModifier += transform.forward * mainMovementPriority;
        //rotationModifier += Vector3.SignedAngle(transform.forward, (nextPos - transform.position).normalized, Vector3.up);
        int rotationModAmount = 0;

        for (int i = 0; i < behaviours.Length; i++)
        {
            try
            {
                SteeringBehaviour sb = (SteeringBehaviour)behaviours[i];
                movementModifier += sb.CalculateMovement(new Vector3(target.x, transform.position.y, target.z));
                float rotationAmount = sb.CalculateRotation(target, rotationSpeed);
                if (Mathf.Abs(rotationAmount) != 0)
                {
                    rotationModifier += rotationAmount;
                    rotationModAmount++;
                }
            }
            catch
            {
                GroupBehaviour gb = (GroupBehaviour)behaviours[i];
                movementModifier += gb.CalculateMovement(group.Agents, this, new Vector3(target.x, transform.position.y, target.z));
                float rotationAmount = gb.CalculateRotation(group.Agents, rotationSpeed);
                if (Mathf.Abs(rotationAmount) != 0)
                {
                    rotationModifier += rotationAmount;
                    rotationModAmount++;
                }
            }
        }

        //Saco promedio o no? mmm...
        //rotationModifier /= rotationModAmount;

        movementModifier.Normalize();
        transform.RotateAround(transform.position, Vector3.up, Mathf.Sign(rotationModifier) * Mathf.Min(Mathf.Abs(rotationModifier), rotationSpeed) * rotationSpeed * Time.deltaTime);
        transform.position += movementModifier * movementSpeed * Time.deltaTime;
    }

    public void SetGroup(Group g)
    {
        group = g;
    }
}
