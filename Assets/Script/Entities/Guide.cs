using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    public float mainMovementPriority;
    public SteeringBehaviour[] steeringBehaviours;
    Rigidbody rb;

    public RoomNode Current { get; set; }
    RoomNode target;
    Stack<RoomNode> path;
    public Stack<RoomNode> Path { get { return path; } }

    void Start()
    {
        //EventManager.Instance.Subscribe(EventID.SCENE_CHANGED, OnSceneChanged);

        for (int i = 0; i < steeringBehaviours.Length; i++)
        {
            steeringBehaviours[i] = Instantiate(steeringBehaviours[i]);
            steeringBehaviours[i].RegisterEntity(transform);
        }
        GameManager.Instance.RegisterTargetSeeker(SetTarget);

        rb = GetComponent<Rigidbody>();
    }

    private void OnSceneChanged(object[] parameterContainer)
    {
        if (target != null) SetTarget(target);
    }

    void FixedUpdate()
    {
        rb.velocity = Vector3.zero;
        if(path != null && path.Count > 0)
        {
            Walk();
        }
    }

    void Walk()
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
                if(Mathf.Abs(rotationAmount) != 0)
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
            //rb.MovePosition(transform.position + movementModifier * movementSpeed * Time.deltaTime);
        }
    }

    void SetTarget(RoomNode room)
    {
        target = room;
        path = Algorithms.ThetaStar(Current, a => a == target, a => (target.Position - a.Position).magnitude,
            (a, b) => Physics.Raycast(a.Position + Vector3.up, b.Position - a.Position, (b.Position - a.Position).magnitude),
            (a) =>
            {
                var arcs = new List<Arc<RoomNode>>();
                var rooms = a.Neighbors;
                foreach (RoomNode r in rooms)
                {
                    arcs.Add(new Arc<RoomNode>(r, (r.Position - a.Position).magnitude));
                }
                return arcs;
            });

        transform.LookAt(new Vector3(path.Peek().Position.x, transform.position.y, path.Peek().Position.z), Vector3.up);
    }

    void OnDrawGizmos()
    {
        if (Current != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Current.Position + Vector3.up * 0.5f, 2);
        }
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(target.Position + Vector3.up * 0.5f, 2);
        }
        if(path != null)
        {
            var p = new Stack<RoomNode>(path);
            while(p.Count > 0)
            {
                RoomNode r = p.Pop();
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(r.Position + Vector3.up * 0.5f, 1.5f);
            }
        }

        for (int i = 0; i < steeringBehaviours.Length; i++)
        {
            steeringBehaviours[i].GizmoDraw();
        }
    }
}
