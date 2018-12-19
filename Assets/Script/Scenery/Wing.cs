using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing : MonoBehaviour
{
    public GameObject[] connectionActive;
    public GameObject[] connectionInactive;

    public void Activate(bool state)
    {
        foreach (var o in connectionInactive) o.SetActive(!state);
        foreach (var o in connectionActive) o.SetActive(state);
    }
}
