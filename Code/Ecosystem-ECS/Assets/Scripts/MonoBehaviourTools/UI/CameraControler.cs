using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] SimulationManager simulationManager;

    private Vector2 camXLimit;
    private Vector2 camYLimit;
    private Vector2 camZLimit;
    private Vector3 pos;
    private Vector2 MapSize;

    private readonly float yMin = 30f;
    private readonly float CamSpeedMax = 2f;
    private readonly float scrollSpeed = 200f;

    private float maxMapSize;
    private float CamSpeed;
    private float xMax;
    private float yMax;
    private float zMax;
    private float zInitial;
    private float lastPosY;
    

    private void Start()
    {
        MapSize = simulationManager.MapSize();
        maxMapSize = Mathf.Max(MapSize.x, MapSize.y);
        xMax = MapSize.x * 5;
        zMax = MapSize.y * 5;
        yMax = 8.5f * maxMapSize;
        zInitial = -3 * maxMapSize;
        transform.position = new Vector3(0f, yMax, zInitial);

        camYLimit = new Vector2(yMin, yMax);

        //just set you can't move camera if user didn't zoom in
        camXLimit = new Vector2(0f, 0f);
        camZLimit = new Vector2(zInitial, zInitial);

    }
    void Update()
    {
        pos = transform.position;
        lastPosY = pos.y;

        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow)) 
        {
            pos.z += CamSpeed;
        }
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)) 
        {
            pos.z -= CamSpeed;
        }
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow)) 
        {
            pos.x += CamSpeed;
        }
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow)) 
        {
            pos.x -= CamSpeed;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed;
        pos.y = Mathf.Clamp(pos.y, camYLimit.x, camYLimit.y);

        //camspeed and X,Z limit will increase as camera zoom in
        if (pos.y != lastPosY)
        {
            float rate = (yMax - pos.y) / (yMax - yMin);
            float zLimitMax = (zMax - zInitial) * rate + zInitial;
            float zLimitMin = zInitial - (zMax + zInitial) * rate;
            float xLimit = xMax * rate;
            camXLimit = new Vector2(-xLimit, xLimit);
            camZLimit = new Vector2(zLimitMin, zLimitMax);
            CamSpeed = CamSpeedMax * rate;
        }

        pos.x = Mathf.Clamp(pos.x, camXLimit.x, camXLimit.y);
        pos.z = Mathf.Clamp(pos.z, camZLimit.x, camZLimit.y);
        transform.position = pos;
    }
}
