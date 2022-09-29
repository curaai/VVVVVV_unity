using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using VVVVVV.UI;

namespace Tests
{
    public class TestPanel : InputTestFixture
    {
        private PanelController pc;
        private Keyboard keyboard;

        public override void Setup()
        {
            base.Setup();

            SceneManager.LoadScene("Scenes/VVVVVV");

            keyboard = InputSystem.AddDevice<Keyboard>();
        }

        [UnityTest]
        public IEnumerator TestPausePanelBasic()
        {
            // TODO: Not implemented 
            Assert.Null(null);
            yield return null;
        }
    }
}
