using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class VRRig : MonoBehaviour
{
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    //�߰� ����
    public Transform headConstraint;
    public Vector3 headBodyOffest; //�Ӹ��� �� ������ �ʱ� ��ġ ����

    // Start is called before the first frame update
    void Start()
    {
        headBodyOffest = transform.position - headConstraint.position;
        headBodyOffest.z = -0.18f;
    }

    void FixedUpdate()
    {
        transform.position = headConstraint.position + headBodyOffest;
        //transform.forward = Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized;
        transform.forward = Vector3.Lerp(transform.forward,
            (Vector3.ProjectOnPlane(headConstraint.up, Vector3.up) * -2).normalized, Time.deltaTime);
        //Vector3.ProjectOnPlane(headConstraint.up, Vector3.up)*-1
        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
  
}
