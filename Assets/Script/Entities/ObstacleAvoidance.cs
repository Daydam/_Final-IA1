using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : MonoBehaviour
{
    public float rayDistance;
    public AnimationCurve intensityCurve;
    public float maxTurningSpeed;
    Ray leftRay;
    Ray rightRay;

    void Start()
    {
        leftRay = new Ray(transform.position + transform.right * (GetComponent<Collider>().bounds.size.x / 2),
            transform.position + transform.right * (GetComponent<Collider>().bounds.size.x / 2) + transform.forward);
        rightRay = new Ray(transform.position - transform.right * (GetComponent<Collider>().bounds.size.x / 2),
            transform.position + transform.right * (GetComponent<Collider>().bounds.size.x / 2) + transform.forward);
    }

    void Update()
    {
        RaycastHit leftHit = new RaycastHit();
        RaycastHit rightHit = new RaycastHit();
        if (Physics.Raycast(leftRay, out leftHit, rayDistance) && Physics.Raycast(rightRay, out rightHit, rayDistance))
        {
            if(Vector3.Distance(transform.position, leftHit.point) / rayDistance > Vector3.Distance(transform.position, rightHit.point) / rayDistance)
            {
                transform.Rotate(new Vector3(0, maxTurningSpeed * Time.deltaTime * intensityCurve.Evaluate(Vector3.Distance(transform.position, leftHit.point) / rayDistance), 0));
            }
            else
            {
                transform.Rotate(new Vector3(0, -maxTurningSpeed * Time.deltaTime * intensityCurve.Evaluate(Vector3.Distance(transform.position, rightHit.point) / rayDistance), 0));
            }
        }
        else if (Physics.Raycast(leftRay, out leftHit, rayDistance))
        {
            transform.Rotate(new Vector3(0, maxTurningSpeed * Time.deltaTime * intensityCurve.Evaluate(Vector3.Distance(transform.position, leftHit.point)/rayDistance), 0));
        }
        else if (Physics.Raycast(rightRay, out rightHit, rayDistance))
        {
            transform.Rotate(new Vector3(0, -maxTurningSpeed * Time.deltaTime * intensityCurve.Evaluate(Vector3.Distance(transform.position, rightHit.point) / rayDistance), 0));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + transform.right * (GetComponent<Collider>().bounds.size.x / 2),
            transform.position + transform.right * (GetComponent<Collider>().bounds.size.x / 2) + transform.forward * rayDistance);
        Gizmos.DrawLine(transform.position - transform.right * (GetComponent<Collider>().bounds.size.x / 2),
            transform.position - transform.right * (GetComponent<Collider>().bounds.size.x / 2) + transform.forward * rayDistance);
    }
}
