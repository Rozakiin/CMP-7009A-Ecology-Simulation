using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [SerializeField] SimulationManager simulationManager;

    private Vector2 canXLimit;
    private Vector2 canYLimit;
    private Vector2 canZLimit;
    private Vector3 pos;
    private Vector2 MapSize;

    private readonly float yMin = 30f;
    private readonly float CanSpeedMax = 2f;
    private readonly float scrollSpeed = 200f;

    private float maxMapSize;
    private float CanSpeed;
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

        canYLimit = new Vector2(yMin, yMax);

        //just set you can't move camera if user didn't zoom in
        canXLimit = new Vector2(0f, 0f);
        canZLimit = new Vector2(zInitial, zInitial);

    }
    void Update()
    {
        pos = transform.position;
        lastPosY = pos.y;

        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow)) 
        {
            pos.z += CanSpeed;
        }
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)) 
        {
            pos.z -= CanSpeed;
        }
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow)) 
        {
            pos.x += CanSpeed;
        }
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow)) 
        {
            pos.x -= CanSpeed;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed;
        pos.y = Mathf.Clamp(pos.y, canYLimit.x, canYLimit.y);

        //canspeed and X,Z limit will increase as camera zoom in
        if (pos.y != lastPosY)
        {
            float rate = (yMax - pos.y) / (yMax - yMin);
            float zLimitMax = (zMax - zInitial) * rate + zInitial;
            float zLimitMin = zInitial - (zMax + zInitial) * rate;
            float xLimit = xMax * rate;
            canXLimit = new Vector2(-xLimit, xLimit);
            canZLimit = new Vector2(zLimitMin, zLimitMax);
            CanSpeed = CanSpeedMax * rate;
        }

        pos.x = Mathf.Clamp(pos.x, canXLimit.x, canXLimit.y);
        pos.z = Mathf.Clamp(pos.z, canZLimit.x, canZLimit.y);
        transform.position = pos;
    }
}
