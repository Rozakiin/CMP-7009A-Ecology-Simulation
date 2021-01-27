using System;
using UnityEngine;

namespace MonoBehaviourTools.UI
{
    public class UICameraFunction
    {
        private Vector2 _camXLimit;
        private readonly Vector2 _camYLimit;
        private Vector2 _camZLimit;

        private float _yMin = 30f;
        private float _camSpeedInitial = 1f;
        private float _camSpeedMultiplier = 1f;
        private float _scrollSpeed = 200f;
        private float _cameraYMultiplier = 8.5f;
        private float _cameraZMultiplier = -3f;

        private float _camSpeed;
        private readonly float _xMax;
        private readonly float _yMax;
        private readonly float _zMax;
        private readonly float _zInitial;


        public UICameraFunction(Vector2 mapSize, float tileSize)
        {
            var maxMapSize = Mathf.Max(mapSize.x, mapSize.y);
            _xMax = mapSize.x * tileSize / 2;
            _zMax = mapSize.y * tileSize / 2;

            _yMax = _cameraYMultiplier * maxMapSize;
            _zInitial = _cameraZMultiplier * maxMapSize;

            _camYLimit = new Vector2(_yMin, _yMax);

            //just set you can't move camera if user didn't zoom in
            _camXLimit = new Vector2(0f, 0f);
            _camZLimit = new Vector2(_zInitial, _zInitial);
        }
        public Vector3 GetNewCameraPosition(Vector3 cameraPosition, float cameraLastPositionY)
        {
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, _camYLimit.x, _camYLimit.y);

            //camSpeed and the limitation of Camera.x, and Camera.z (Camera boundary) will increase as camera zoom in
            // only update camera border when the movement of Camera.Y more than 5f or camera zoom in more than 5f
            if (Math.Abs(cameraPosition.y - cameraLastPositionY) >= 5f)
            {
                float rate = (_yMax - cameraPosition.y) / (_yMax - _yMin);
                float zLimitMax = (_zMax - _zInitial) * rate + _zInitial;
                float zLimitMin = _zInitial - (_zMax + _zInitial) * rate;
                float xLimit = _xMax * rate;
                _camXLimit = new Vector2(-xLimit, xLimit);
                _camZLimit = new Vector2(zLimitMin, zLimitMax);
                _camSpeed = _camSpeedInitial + (_camSpeedMultiplier * rate);
            }

            cameraPosition.x = Mathf.Clamp(cameraPosition.x, _camXLimit.x, _camXLimit.y);
            cameraPosition.z = Mathf.Clamp(cameraPosition.z, _camZLimit.x, _camZLimit.y);
            return cameraPosition;
        }

        public Vector3 GetInitialPosition()
        {
            return new Vector3(0f, _yMax, _zInitial);
        }

        public float GetCameraSpeed()
        {
            return _camSpeed;
        }

        public float GetScrollSpeed()
        {
            return _scrollSpeed;
        }

        public float GetYMin()
        {
            return _yMin;
        }
    }
}