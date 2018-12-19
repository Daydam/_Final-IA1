using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2 size;
    public Wing[] wings;

    public int WallCount { get { return wings.Length; } }

    static int[] xMovementVector = new int[] { 0, 1, 0, -1 };
    static int[] yMovementVector = new int[] { 1, 0, -1, 0 };

    int x;
    int y;
    Vector3 basePosition;
    int creatingMovement;
    Room parent;
    bool[,] visited;
    Room prefab;
    RoomNode node;
    public RoomNode Node { get { return node; } }

    void ActivateWing(int index, bool value)
    {
        wings[index].Activate(value);
    }

    public void Configure(int x, int y, Vector3 basePosition, Room parent, Room prefab, int creatingMovement, bool[,] visited, Transform container)
    {
        gameObject.name = gameObject.name.Replace("(Clone)", "");
        transform.position = basePosition + new Vector3(x * size.x, 0, y * size.y);
        visited[x, y] = true;
        this.x = x;
        this.y = y;
        this.basePosition = basePosition;
        this.creatingMovement = creatingMovement;
        this.parent = parent;
        this.visited = visited;
        this.prefab = prefab;
        transform.parent = container;
        node = new RoomNode(transform.position);
    }

    public void Connect(int index, Room room)
    {
        ActivateWing(index, true);
        node.Add(room.node);
    }

    public static bool ProcessRoom(Room room)
    {
        if (room.parent == null) return false;
        room.parent.Connect(room.creatingMovement, room);
        room.Connect((room.creatingMovement + 2) % 4, room.parent);
        return false;
    }

    static public IEnumerable<Room> Explode(Room room)
    {
        var toVisit = new List<Room>();

        for (int i = 0; i < 4; i++)
        {
            var nextRoomX = room.x + xMovementVector[i];
            var nextRoomY = room.y + yMovementVector[i];

            if (0 <= nextRoomX && nextRoomX < room.visited.GetLength(0) &&
                0 <= nextRoomY && nextRoomY < room.visited.GetLength(1) &&
                !room.visited[nextRoomX, nextRoomY])
            {
                var child = Instantiate(room.prefab);
                child.Configure(nextRoomX, nextRoomY, room.basePosition, room, room.prefab, i, room.visited, room.transform.parent);
                toVisit.Add(child);
            }
        }

        return ListHandling.RandomizeList(toVisit);
    }

    void OnMouseDown()
    {
        GameManager.Instance.TargetFound(Node);
    }

    void OnDrawGizmos()
    {
        if (node != null) node.Draw();
    }
}
