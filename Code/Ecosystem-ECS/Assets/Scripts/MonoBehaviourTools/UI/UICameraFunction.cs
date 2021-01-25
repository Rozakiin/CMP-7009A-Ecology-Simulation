using System;
using UnityEngine;

namespace MonoBehaviourTools.UI
{
    public class UICameraFunction
    {
        private Vector2 camXLimit;
        private readonly Vector2 camYLimit;
        private Vector2 camZLimit;
        private Vector3 pos;
        private Vector2 mapSize;

        private float yMin = 30f;
        private float camSpeedInitial = 1f;
        private float camSpeedMultiplier = 1f;
        private float scrollSpeed = 200f;
        private float cameraYMultiplier = 8.5f;
        private float cameraZMultiplier = -3f;

        private float camSpeed;
        private float lastPosY;
        private readonly float xMax;
        private readonly float yMax;
        private readonly float zMax;
        private readonly float zInitial;
        

        public UICameraFunction(Vector2 mapSize,float tileSize)
        {
            var maxMapSize = Mathf.Max(mapSize.x, mapSize.y);
            xMax = mapSize.x * tileSize / 2;
            zMax = mapSize.y * tileSize / 2;
            
            yMax = cameraYMultiplier * maxMapSize;
            zInitial = cameraZMultiplier * maxMapSize;

            camYLimit = new Vector2(yMin, yMax);

            //just set you can't move camera if user didn't zoom in
            camXLimit = new Vector2(0f, 0f);
            camZLimit = new Vector2(zInitial, zInitial);
        }
        public Vector3 GetNewCameraPosition(Vector3 cameraPosition, float cameraLastPositionY)
        {
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, camYLimit.x, camYLimit.y);

            //camSpeed and the limitation of Camera.x, and Camera.z (Camera boundary) will increase as camera zoom in
            // only update camera border when the movement of Camera.Y more than 5f or camera zoom in more than 5f
            if (Math.Abs(cameraPosition.y - cameraLastPositionY) >= 5f)
            {
                float rate = (yMax - cameraPosition.y) / (yMax - yMin);
                float zLimitMax = (zMax - zInitial) * rate + zInitial;
                float zLimitMin = zInitial - (zMax + zInitial) * rate;
                float xLimit = xMax * rate;
                camXLimit = new Vector2(-xLimit, xLimit);
                camZLimit = new Vector2(zLimitMin, zLimitMax);
                camSpeed = camSpeedInitial + (camSpeedMultiplier * rate);
            }

            cameraPosition.x = Mathf.Clamp(cameraPosition.x, camXLimit.x, camXLimit.y);
            cameraPosition.z = Mathf.Clamp(cameraPosition.z, camZLimit.x, camZLimit.y);
            return cameraPosition;
        }

        public Vector3 GetInitialPosition()
        {
            return new Vector3(0f, yMax, zInitial);
        }

        public float GETCameraSpeed()
        {
            return camSpeed;
        }

        public float GETScrollSpeed()
        {
            return scrollSpeed;
        }

        public float GetYMin()
        {
            return yMin;
        }
    }
}