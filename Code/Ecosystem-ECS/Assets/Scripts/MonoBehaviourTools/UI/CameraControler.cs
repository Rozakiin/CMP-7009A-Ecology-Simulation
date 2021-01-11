using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public float CanSpeed = 20f;
    public float CanBorderThicknerss = 20f;
    public Vector2 canLimit;

    public float scrollSpeed = 20f;
    public float minY = -100f;
    public float maxY = 300;


    void Update()
    {
        Vector3 pos = transform.position;
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - CanBorderThicknerss)
        {
            pos.z += CanSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= CanBorderThicknerss)
        {
            pos.z -= CanSpeed * Time.deltaTime;
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - CanBorderThicknerss)
        {
            pos.x += CanSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= CanBorderThicknerss)
        {
            pos.x -= CanSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y += scroll * scrollSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -canLimit.x, canLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, -canLimit.y, canLimit.y);
        transform.position = pos;
    }
}
