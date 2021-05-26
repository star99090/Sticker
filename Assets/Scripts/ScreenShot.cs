using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    [SerializeField] GameObject blink;

    public void Screenshot()
    {
        StartCoroutine(Capture());
    }

    IEnumerator Capture()
    {
        string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        string fileName = "Screenshot" + timeStamp + ".png";
        string pathToSave = fileName;
        ScreenCapture.CaptureScreenshot(pathToSave);
        yield return new WaitForEndOfFrame();
        Instantiate(blink, Vector2.zero, Quaternion.identity);
    }
}