using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Hourai {

    [RequireComponent(typeof (Text))]
    public sealed class FPSCounter : MonoBehaviour {

        private Text Counter;
        private float deltaTime;
        private float fps;
        private float msec;
        private string outputText;

        private void Awake() {
            Counter = GetComponent<Text>();
            StartCoroutine(UpdateDisplay());
        }

        private void Update() {
            deltaTime += (Time.deltaTime - deltaTime)*0.1f;
            msec = deltaTime*1000.0f;
            fps = 1.0f/deltaTime;
        }

        private IEnumerator UpdateDisplay() {
            while (true) {
                yield return new WaitForSeconds(0.5f);
                Counter.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            }
        }

    }

}