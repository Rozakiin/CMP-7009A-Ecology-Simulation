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

        private readonly float yMin = 30f;
        private readonly float camSpeedMax = 2f;
        private readonly float scrollSpeed = 200f;

        private float camSpeed;
        private readonly float xMax;
        private readonly float yMax;
        private readonly float zMax;
        private readonly float zInitial;
        private float lastPosY;

        public UICameraFunction(Vector2 mapSize)
        {
            var maxMapSize = Mathf.Max(mapSize.x, mapSize.y);
            xMax = mapSize.x * 5;
            zMax = mapSize.y * 5;
            yMax = 8.5f * maxMapSize;
            zInitial = -3 * maxMapSize;

            camYLimit = new Vector2(yMin, yMax);

            //just set you can't move camera if user didn't zoom in
            camXLimit = new Vector2(0f, 0f);
            camZLimit = new Vector2(zInitial, zInitial);
        }
        public Vector3 CheckCameraBorder(Vector3 cameraPosition, float cameraLastPositionY)
        {
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, camYLimit.x, camYLimit.y);

            //camSpeed and X,Z limit will increase as camera zoom in
            if (Math.Abs(cameraPosition.y - cameraLastPositionY) >= 5f)
            {
                float rate = (yMax - cameraPosition.y) / (yMax - yMin);
                float zLimitMax = (zMax - zInitial) * rate + zInitial;
                float zLimitMin = zInitial - (zMax + zInitial) * rate;
                float xLimit = xMax * rate;
                camXLimit = new Vector2(-xLimit, xLimit);
                camZLimit = new Vector2(zLimitMin, zLimitMax);
                camSpeed = camSpeedMax * rate;
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