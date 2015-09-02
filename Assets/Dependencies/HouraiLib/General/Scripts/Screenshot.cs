using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Takes screenshots upon pressing F12
/// </summary>
/// Author: James Liu
/// Authored on: 07/01/2015
public class Screenshot : MonoBehaviour {

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F12)) {
            string filename = "screenshot-" + DateTime.UtcNow.ToString("MM-dd-yyyy-HHmmss") + ".png";
            string path = Path.Combine(Application.persistentDataPath, filename);

            if (File.Exists(path))
                File.Delete(path);

            if (Application.platform == RuntimePlatform.IPhonePlayer)
                Application.CaptureScreenshot(filename);
            else
                Application.CaptureScreenshot(path);
        }
    }

}