using System.Collections;
using MonoBehaviourTools.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class UITimeControlTest
    {
        private UITimeControl uITimeControl;
        private GameObject gameObject;
        
        [SetUp]
        public void SetUp()
        {
            gameObject = new GameObject();
            uITimeControl = gameObject.AddComponent<UITimeControl>();
        }
        
        [UnityTest]
        public IEnumerator CheckBoolPause()
        {
            yield return null;
            
            uITimeControl.Pause();
            Assert.True(uITimeControl.GetPause());
            
            uITimeControl.Play();
            Assert.False(uITimeControl.GetPause());
        }
        
        
        [Test]
        public void IncreaseSpeedTest()
        {
            uITimeControl.fastForwardSpeed = 1f;
            uITimeControl.IncreaseSpeed();
            Assert.AreEqual(2f,uITimeControl.fastForwardSpeed);
            
            uITimeControl.fastForwardSpeed = 0.2f;
            uITimeControl.IncreaseSpeed();
            Assert.AreEqual(0.4f,uITimeControl.fastForwardSpeed);
        }
        
        [Test]
        public void DecreaseSpeedTest()
        {
            uITimeControl.fastForwardSpeed = 1f;
            uITimeControl.DecreaseSpeed();
            Assert.AreEqual(0.8f,uITimeControl.fastForwardSpeed);
            
            uITimeControl.fastForwardSpeed = 0.2f;
            uITimeControl.DecreaseSpeed();
            Assert.AreEqual(0f,uITimeControl.fastForwardSpeed);
            
            uITimeControl.fastForwardSpeed = 0.4f;
            uITimeControl.DecreaseSpeed();
            Assert.AreEqual(0.2f,uITimeControl.fastForwardSpeed);
        }
    }
}
