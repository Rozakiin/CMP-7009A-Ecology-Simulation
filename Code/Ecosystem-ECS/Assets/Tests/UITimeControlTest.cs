using System.Collections;
using MonoBehaviourTools.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class UITimeControlTest
    {
        private UITimeControl _uITimeControl;
        private GameObject _gameObject;
        
        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _uITimeControl = _gameObject.AddComponent<UITimeControl>();
        }
        
        [UnityTest]
        public IEnumerator CheckBoolPause()
        {
            yield return null;
            
            _uITimeControl.Pause();
            Assert.True(_uITimeControl.GetPause());
            
            _uITimeControl.Play();
            Assert.False(_uITimeControl.GetPause());
        }
        
        
        [Test]
        public void IncreaseSpeedTest()
        {
            _uITimeControl.FastForwardSpeed = 1f;
            _uITimeControl.IncreaseSpeed();
            Assert.AreEqual(2f,_uITimeControl.FastForwardSpeed);
            
            _uITimeControl.FastForwardSpeed = 0.2f;
            _uITimeControl.IncreaseSpeed();
            Assert.AreEqual(0.4f,_uITimeControl.FastForwardSpeed);
        }
        
        [Test]
        public void DecreaseSpeedTest()
        {
            _uITimeControl.FastForwardSpeed = 1f;
            _uITimeControl.DecreaseSpeed();
            Assert.AreEqual(0.8f,_uITimeControl.FastForwardSpeed);
            
            _uITimeControl.FastForwardSpeed = 0.2f;
            _uITimeControl.DecreaseSpeed();
            Assert.AreEqual(0f,_uITimeControl.FastForwardSpeed);
            
            _uITimeControl.FastForwardSpeed = 0.4f;
            _uITimeControl.DecreaseSpeed();
            Assert.AreEqual(0.2f,_uITimeControl.FastForwardSpeed);
        }
    }
}
