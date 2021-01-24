using MonoBehaviourTools.UI;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class UICameraTest
    {
        private UICameraFunction cameraFunction;
        private readonly Vector2 mapSize = new Vector2(100, 120); 
        
        [SetUp]
        public void Setup()
        {
            cameraFunction = new UICameraFunction(mapSize,10f);
        }
        
        [Test]
        public void CheckCameraBorder()
        {
            var yMin = cameraFunction.GetYMin();
            
            Vector3 pos1 = new Vector3(float.PositiveInfinity, yMin, float.PositiveInfinity);
            Vector3 pos2 = new Vector3(float.NegativeInfinity, yMin, float.NegativeInfinity);
            
            Vector3 pos3  = cameraFunction.GetNewCameraPosition(pos1,yMin+5f);
            Vector3 pos4  = cameraFunction.GetNewCameraPosition(pos2,yMin+5f);
            
            Assert.AreEqual(new Vector2(pos4.x,pos3.x), new Vector2 (-mapSize.x * 5, mapSize.x * 5));
            Assert.AreEqual(new Vector2(pos4.z,pos3.z), new Vector2 (-mapSize.y * 5, mapSize.y * 5));
            
            Vector3 pos5  = cameraFunction.GetNewCameraPosition(new Vector3(mapSize.x * 5,yMin+4f,mapSize.y * 5),yMin);
            float camSpeed = cameraFunction.GETCameraSpeed();
            Assert.AreEqual(new Vector2(pos5.x,pos5.z), new Vector2 (mapSize.x * 5,mapSize.y * 5));
            
            Vector3 pos6  = cameraFunction.GetNewCameraPosition(new Vector3(mapSize.x * 5,yMin+5f,mapSize.y * 5),yMin);
            float camSpeed2 = cameraFunction.GETCameraSpeed();
            Assert.AreEqual(new Vector2(pos5.x,pos5.z), new Vector2 (mapSize.x * 5,mapSize.y * 5));
            Assert.Less(pos6.x,mapSize.x*5);
            Assert.Less(pos6.z,mapSize.y*5);
            Assert.Less(camSpeed2, camSpeed);
        }
    }
}