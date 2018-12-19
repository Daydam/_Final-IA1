using UnityEngine;
using System.Collections.Generic;

public class RoomNode
{
    Vector3 position;
    public Vector3 Position { get { return position; } }
    
    List<RoomNode> neighbors = new List<RoomNode>();
    public IEnumerable<RoomNode> Neighbors { get { return neighbors; } }
    
    public RoomNode(Vector3 position)
    {
        this.position = position;
    }

    public void Add(RoomNode neighbor)
    {
        neighbors.Add(neighbor);
    }

    public void Draw()
    {
        Gizmos.color = Color.green;
        var up = Vector3.up;

        foreach (var room in neighbors)
            Gizmos.DrawLine(position + up, position + (room.position - position) * 0.5f + up);

        Gizmos.DrawWireSphere(position + up, 1f);
    }
}