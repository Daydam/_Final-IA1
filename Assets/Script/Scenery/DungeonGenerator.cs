using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private static DungeonGenerator instance;
    public static DungeonGenerator Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DungeonGenerator>();
                if (instance == null)
                {
                    instance = new GameObject("new DungeonGenerator Object").AddComponent<DungeonGenerator>().GetComponent<DungeonGenerator>();
                }
            }
            return instance;
        }
    }

    public Guide guide;
    public Group followerGroup;
    public Room prefab;
    public int width;
    public int length;
    public bool generate;

    IEnumerable<Room> rooms;
    GameObject pathWalker;

    void Awake()
    {
        Generate();
    }

    void Update()
    {
        if (generate)
        {
            generate = false;
            DestroyRooms();
            Generate();
        }
    }

    void Generate()
    {
        var startX = Random.Range(0, width);
        var startY = Random.Range(0, length);
        var cellSize = prefab.size;
        var basePosition = -new Vector3(width * cellSize.x, 0, length * cellSize.y) * 0.5f;
        var visited = new bool[width, length];
        var initialRoom = Instantiate(prefab);

        initialRoom.Configure(startX, startY, basePosition, null, prefab, 0, visited, transform);
        rooms = Algorithms.DFS(initialRoom, Room.ProcessRoom, Room.Explode);
        guide.Current = initialRoom.Node;
        guide.transform.position = initialRoom.transform.position + Vector3.up;
        followerGroup.transform.position = initialRoom.transform.position + Vector3.up;
    }

    void DestroyRooms()
    {
        foreach (Room r in rooms)
        {
            Destroy(r.gameObject);
        }
    }
}
