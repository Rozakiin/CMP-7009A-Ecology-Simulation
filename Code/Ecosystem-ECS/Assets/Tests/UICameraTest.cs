using MonoBehaviourTools.UI;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class UICameraTest
    {
        private UICameraFunction _cameraFunction;
        private readonly Vector2 _mapSize = new Vector2(100, 120); 
        
        [SetUp]
        public void Setup()
        {
            _cameraFunction = new UICameraFunction(_mapSize,10f);
        }
        
        [Test]
        public void CheckCameraBorder()
        {
            var yMin = _cameraFunction.GetYMin();
            
            Vector3 pos1 = new Vector3(float.PositiveInfinity, yMin, float.PositiveInfinity);
            Vector3 pos2 = new Vector3(float.NegativeInfinity, yMin, float.NegativeInfinity);
            
            Vector3 pos3  = _cameraFunction.GetNewCameraPosition(pos1,yMin+5f);
            Vector3 pos4  = _cameraFunction.GetNewCameraPosition(pos2,yMin+5f);
            
            Assert.AreEqual(new Vector2(pos4.x,pos3.x), new Vector2 (-_mapSize.x * 5, _mapSize.x * 5));
            Assert.AreEqual(new Vector2(pos4.z,pos3.z), new Vector2 (-_mapSize.y * 5, _mapSize.y * 5));
            
            Vector3 pos5  = _cameraFunction.GetNewCameraPosition(new Vector3(_mapSize.x * 5,yMin+4f,_mapSize.y * 5),yMin);
            float camSpeed = _cameraFunction.GetCameraSpeed();
            Assert.AreEqual(new Vector2(pos5.x,pos5.z), new Vector2 (_mapSize.x * 5,_mapSize.y * 5));
            
            Vector3 pos6  = _cameraFunction.GetNewCameraPosition(new Vector3(_mapSize.x * 5,yMin+5f,_mapSize.y * 5),yMin);
            float camSpeed2 = _cameraFunction.GetCameraSpeed();
            Assert.AreEqual(new Vector2(pos5.x,pos5.z), new Vector2 (_mapSize.x * 5,_mapSize.y * 5));
            Assert.Less(pos6.x,_mapSize.x*5);
            Assert.Less(pos6.z,_mapSize.y*5);
            Assert.Less(camSpeed2, camSpeed);
        }
    }
}