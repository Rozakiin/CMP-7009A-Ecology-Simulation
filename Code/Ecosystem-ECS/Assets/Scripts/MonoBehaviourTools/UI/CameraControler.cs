using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public float CanSpeed = 20f;
    public Vector2 canLimit;
    public float moveSpeed = 10f;

    public float scrollSpeed = 20f;
    public float minY = -100f;
    public float maxY = 300; 
    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if (Input.GetKey("w"))
        {
            pos.z += CanSpeed * moveSpeed;
        }
        if (Input.GetKey("s"))
        {
            pos.z -= CanSpeed * moveSpeed;
        }
        if (Input.GetKey("d"))
        {
            pos.x += CanSpeed * moveSpeed;
        }
        if (Input.GetKey("a"))
        {
            pos.x -= CanSpeed * moveSpeed;
        }
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y += scroll * scrollSpeed * 100f * moveSpeed;

        pos.x = Mathf.Clamp(pos.x, -canLimit.x, canLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -canLimit.y, canLimit.y);
        transform.position = pos;
    }
}
