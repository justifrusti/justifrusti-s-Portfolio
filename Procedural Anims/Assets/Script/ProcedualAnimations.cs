using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedualAnimations : MonoBehaviour
{
    static Vector3[] CastOnSurface(Vector3 point, float halfRange, Vector3 up)
    {
        Vector3[] res = new Vector3[2];

        RaycastHit hit;

        Ray ray = new Ray(new Vector3(point.x, point.y + halfRange, point.z), -up);

        if (Physics.Raycast(ray, out hit, 2f * halfRange))
        {
            res[0] = hit.point;
            res[1] = hit.normal;
        }else
        {
            res[0] = point;
        }

        return res;
    }
    bool IsInEllipse(Vector2 point, Vector2 center, float minorRadius, float majorRadius)
    {
        return Mathf.Pow((point.x - center.x) / majorRadius, 2f) + Mathf.Pow((point.y - center.y) / minorRadius, 2f) <= 1f;
    }


    public Transform leftFootTarget;
    public Transform rightFootTarget;

    private Vector3 initLeftFootPos;
    private Vector3 initRightFootPos;
    private Vector3 initBodyPos;

    private Vector3 lastLeftFootPos;
    private Vector3 lastRightFootPos;
    private Vector3 lastBodyPos;

    // Start is called before the first frame update
    void Start()
    {
        initLeftFootPos = leftFootTarget.localPosition;
        initRightFootPos = -rightFootTarget.localPosition;

        lastLeftFootPos = leftFootTarget.position;
        lastRightFootPos = rightFootTarget.position;

        lastBodyPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = transform.position - lastBodyPos;
        Vector3 centerOfMass = transform.position;

        leftFootTarget.position = lastLeftFootPos;
        rightFootTarget.position = lastRightFootPos;

        lastLeftFootPos = leftFootTarget.position;
        lastRightFootPos = rightFootTarget.position;
    }

    bool IsBalanced()
    {
        Vector3 ellipseCenter = (leftFootTarget.position + rightFootTarget.position) / 2f;
        ellipseCenter = Vector3.ProjectOnPlane(ellipseCenter, transform.up);
        Vector2 ellipseCenter2D = new Vector2(ellipseCenter.x, ellipseCenter.z);

        Vector3 point = Vector3.ProjectOnPlane(transform.position, transform.up);
        Vector2 point2D = new Vector2(point.x, point.z);

        Vector3 feetAxis = Vector3.ProjectOnPlane((rightFootTarget.position - leftFootTarget.position), transform.up).normalized;
        Vector2 feetAxis2D = new Vector2(feetAxis.x, feetAxis.z);

        

        //return IsInEllipse(point, ellipseCenter2D, minor)
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(leftFootTarget.position, 0.2f);
        Gizmos.DrawWireSphere(rightFootTarget.position, 0.2f);

        Debug.DrawLine(transform.position, transform.position + transform.up * 2f);
    }
}
