using UnityEngine;

namespace MonoBehaviourTools.UI
{
    public class UICamera : MonoBehaviour
    {
        [SerializeField] private SimulationManager simulationManager;
        private UICameraFunction cameraFunction;
    
        private void Start()
        {
            Vector2 mapSize = SimulationManager.MapSize();
            cameraFunction = new UICameraFunction(mapSize);
            transform.position = cameraFunction.GetInitialPosition();
        }
        private void Update()
        {
            Vector3 pos = transform.position;
            float lastPosY = pos.y;
            if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
            {
                pos.z += cameraFunction.GETCameraSpeed();
            }
            if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)) 
            {
                pos.z -= cameraFunction.GETCameraSpeed();
            }
            if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow)) 
            {
                pos.x += cameraFunction.GETCameraSpeed();
            }
            if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow)) 
            {
                pos.x -= cameraFunction.GETCameraSpeed();
            }
        
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y -= scroll * cameraFunction.GETScrollSpeed();
            transform.position = cameraFunction.CheckCameraBorder(pos,lastPosY);
        }


    }
}
