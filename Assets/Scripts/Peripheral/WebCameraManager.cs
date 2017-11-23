using UnityEngine;
using System.Collections;

public class WebCameraManager : MonoBehaviour
{

    public string DeviceName;
    public Vector2 CameraSize;
    public float CameraFPS;

    //接收返回的图片数据    
    WebCamTexture _webCamera;
    public GameObject Plane;//作为显示摄像头的面板  

    void Start()
    {
        StopCamera();
        StartCoroutine("InitCameraCor");
    }

    //void OnGUI()
    //{
        //if (GUI.Button(new Rect(100, 100, 100, 100), "Initialize Camera"))
        //{
        //    StartCoroutine("InitCameraCor");
        //}

        ////添加一个按钮来控制摄像机的开和关  
        //if (GUI.Button(new Rect(100, 250, 100, 100), "ON/OFF"))
        //{
        //    if (_webCamera != null && Plane != null)
        //    {

        //        if (_webCamera.isPlaying)
        //            StopCamera();
        //        else
        //            PlayCamera();
        //    }
        //}
        //if (GUI.Button(new Rect(100, 450, 100, 100), "Quit"))
        //{

        //    Application.Quit();
        //}
    //}

    public void PlayCamera()
    {
        Plane.GetComponent<MeshRenderer>().enabled = true;
        _webCamera.Play();
    }


    public void StopCamera()
    {
        if (_webCamera != null) { 
            Plane.GetComponent<MeshRenderer>().enabled = false;
            _webCamera.Stop();
        }
    }

    /// <summary>    
    /// 初始化摄像头  
    /// </summary>    
    public IEnumerator InitCameraCor()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            DeviceName = devices[0].name;

            Camera camera = transform.GetComponent<Camera>();
            camera.orthographicSize = (float)Screen.height / 20f;
            //设置背景Plane的大小
            #if UNITY_IPHONE
                Plane.transform.localScale = new Vector3(-(float)Screen.width/100f, -1f, (float)Screen.height/100f);
            #else
                Plane.transform.localScale = new Vector3(-(float)Screen.width/100f, 1f, (float)Screen.height/100f);
            #endif

            _webCamera = new WebCamTexture(DeviceName, (int)CameraSize.x, (int)CameraSize.y, (int)CameraFPS);

            
            Plane.GetComponent<Renderer>().material.mainTexture = _webCamera;

            _webCamera.Play();
            Camera.main.clearFlags = CameraClearFlags.Depth;
        }
    }

    void OnDestroy() { 
        StopCamera();
    }
}
