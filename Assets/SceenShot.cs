using UnityEngine;

public class SceenShot : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string fileName = "screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
            // Use superSize parameter (e.g., 2 or 4) for higher resolution
            ScreenCapture.CaptureScreenshot(fileName, 2);
             Debug.Log("Screenshot saved: " + fileName);
        }
    }
}
