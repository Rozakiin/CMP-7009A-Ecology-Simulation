using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 cameraFollowPosition;
    private Vector3 cameraFollowRotation;
    // Start is called before the first frame update
    public void SetUpPosition(Vector3 cameraFollowPosition)
    {
        this.cameraFollowPosition = cameraFollowPosition;
    }
    public void SetUpEulerAngles(Vector3 cameraFollowRotation)
    {
        this.cameraFollowRotation = cameraFollowRotation;
    }
    public Vector3 GetCameraPostion()
    {
        return transform.position;
    }
    public Vector3 GetCameraEulerAngles()
    {
        return transform.eulerAngles;
    }
    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = cameraFollowRotation;
        transform.position = cameraFollowPosition;
    }
}
