using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnTargetFound(RoomNode a);

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    instance = new GameObject("new GameManager Object").AddComponent<GameManager>().GetComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    OnTargetFound seekPath = (a) => { };

    public void RegisterTargetSeeker(OnTargetFound seeker)
    {
        seekPath += seeker;
    }

    public void TargetFound(RoomNode room)
    {
        seekPath(room);
    }
}
