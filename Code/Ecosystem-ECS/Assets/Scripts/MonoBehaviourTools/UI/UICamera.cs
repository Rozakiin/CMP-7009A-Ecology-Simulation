using UnityEngine;

namespace MonoBehaviourTools.UI
{
    public class UICamera : MonoBehaviour
    {
        private UICameraFunction _cameraFunction;
        private float _scrollSpeed;

        private void Start()
        {
            Vector2 mapSize = SimulationManager.MapSize();
            float tileSize = SimulationManager.TileSize;
            _cameraFunction = new UICameraFunction(mapSize, tileSize);
            transform.position = _cameraFunction.GetInitialPosition();
            _scrollSpeed = _cameraFunction.GetScrollSpeed();
        }

        private void Update()
        {
            Vector3 pos = transform.position;
            float lastPosY = pos.y;
            if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
            {
                pos.z += _cameraFunction.GetCameraSpeed();
            }
            if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))
            {
                pos.z -= _cameraFunction.GetCameraSpeed();
            }
            if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
            {
                pos.x += _cameraFunction.GetCameraSpeed();
            }
            if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
            {
                pos.x -= _cameraFunction.GetCameraSpeed();
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y -= scroll * _scrollSpeed;
            transform.position = _cameraFunction.GetNewCameraPosition(pos, lastPosY);
        }
    }
}
