using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    private float initialZPosition;
    private int iterationCount = 0;
    int z = 0;
    void Start()
    {
        mainCamera = Camera.main;
        initialZPosition = transform.position.z;

        
        StartCoroutine(RepeatCapture());
        

        Debug.Log("bitti");
    }

    IEnumerator RepeatCapture()
    {
        while (iterationCount < 1)
        {
            
            yield return MoveCameraForward(20f);
            yield return new WaitForSeconds(1f);
            yield return MoveCameraForward(35f);
            yield return new WaitForSeconds(1f);
            yield return MoveCameraForward(30f);
            yield return new WaitForSeconds(1f);
            yield return MoveCameraForward(30f);
            yield return new WaitForSeconds(1f);

            iterationCount++;
        }

        CallPythonScript();
        BaskaSahneyeGec();


    }

    IEnumerator MoveCameraForward(float distance)
    {
        Vector3 newPosition = transform.position + Vector3.forward * distance;
        transform.position = newPosition;
        yield return new WaitForSeconds(0.2f);
        RotateCamera(90f);
        CaptureScreenshot();
        yield return new WaitForSeconds(0.2f);
        RotateCameraX(330f);
        CaptureScreenshot();
        yield return new WaitForSeconds(0.2f);
        RotateCameraX(30f);
        yield return new WaitForSeconds(0.2f);
        RotateCamera(180f);
        CaptureScreenshot();
        yield return new WaitForSeconds(0.2f);
        RotateCameraX(330f);
        CaptureScreenshot();
        yield return new WaitForSeconds(0.2f);
        RotateCameraX(30f);
        yield return new WaitForSeconds(0.2f);
        RotateCamera(90f);
        yield return new WaitForSeconds(0.2f);
    }
    IEnumerator MoveCameraForward1(float distance)
    {
        Vector3 newPosition = transform.position + Vector3.forward * distance;
        transform.position = newPosition;
        yield return new WaitForSeconds(0.2f);
    }

    void RotateCameraX(float angle)
    {
        transform.Rotate(Vector3.right, angle);
    }

    void RotateCamera(float angle)
    {
        transform.Rotate(Vector3.up, angle);
    }

    void CaptureScreenshot()
    {
        string screenshotPath = "Assets/Scripts/Screenshots/";

        if (!System.IO.Directory.Exists(screenshotPath))
        {
            System.IO.Directory.CreateDirectory(screenshotPath);
        }
        string screenshotName = $"{screenshotPath}{z}.png";

        ScreenCapture.CaptureScreenshot(screenshotName);

        Debug.Log(z);
        z++;
    }
    void CallPythonScript()
    {
        string pythonScriptPath = "\"D:\\DosyalarToplu\\UnityProjects\\BitirmeSehirOlusturmaYapayZeka\\Assets\\Scripts\\inference.py\"";
        string pythonImagePath = "\"D:\\DosyalarToplu\\UnityProjects\\BitirmeSehirOlusturmaYapayZeka\\Assets\\Scripts\\Screenshots\"";
        string pythonOutputPath = "\"D:\\DosyalarToplu\\UnityProjects\\BitirmeSehirOlusturmaYapayZeka\\Assets\\Scripts\\Results\"";
        // img_path parametresi
        //string imgPath = "D:\\DosyalarToplu\\UnityProjects\\BitirmeSehirOlusturmaYapayZeka\\Assets\\Scripts\\image1.png";

        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("python", $"{pythonScriptPath} {pythonImagePath} {pythonOutputPath}");
        startInfo.RedirectStandardError = true;
        startInfo.UseShellExecute = false;

        using (System.Diagnostics.Process process = new System.Diagnostics.Process() { StartInfo = startInfo })
        {
            try
            {
                process.Start();
                string errorOutput = process.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(errorOutput))
                {
                    Debug.LogError("Python Error: " + errorOutput);
                }
                process.WaitForExit();
            }

            catch (InvalidOperationException ex)
            {
                Debug.LogError($"InvalidOperationException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error: {ex.Message}");
            }
        }
    }
    public void BaskaSahneyeGec()
    {
        SceneManager.LoadScene("binaDik");
    }
}
