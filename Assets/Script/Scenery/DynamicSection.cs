using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSection : MonoBehaviour
{
    Vector3 previousPosition;
    Quaternion previousRotation;

	void Start ()
    {
        previousPosition = transform.position;
        previousRotation = transform.rotation;
	}
	
	void Update ()
    {
        if (previousRotation != transform.rotation || previousPosition != transform.position) EventManager.Instance.DispatchEvent(EventID.SCENE_CHANGED);
        previousPosition = transform.position;
        previousRotation = transform.rotation;
    }

    private void OnEnable()
    {
        EventManager.Instance.DispatchEvent(EventID.SCENE_CHANGED);
    }

    private void OnDisable()
    {
        EventManager.Instance.DispatchEvent(EventID.SCENE_CHANGED);
    }
}
