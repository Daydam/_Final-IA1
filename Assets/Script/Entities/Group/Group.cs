using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    public Guide leader;
    public Guide Leader { get { return leader; } }

    TravellingAgent[] agents;
    public TravellingAgent[] Agents { get { return agents; } }

    void Awake()
    {
        agents = GetComponentsInChildren<TravellingAgent>();

        foreach (var agent in agents)
        {
            agent.SetGroup(this);
        }
    }
}
