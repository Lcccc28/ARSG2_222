using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {

    public new GameObject camera;

    // 方向灵敏度
    private float sensitivityX = 1f;
    private float sensitivityY = 2f;

    // 左右的视角范围
    private float minimumX = 60;
    private float maximumX = 300;

    //// 上下最大视角
    private float minimumY = -60f;
    private float maximumY = 30f;

    float rotationX = 0;
    float rotationY = 0f;

    void Update() {
        if (Time.timeScale == 0) return;

        // 根据鼠标移动的快慢(增量), 获得相机左右旋转的角度(处理X) 
        rotationX  = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

        //if (rotationX > minimumX && rotationX < maximumX) {
        //    if (rotationX - 180 > 0) {
        //        rotationX = maximumX;
        //    }  else {
        //        rotationX = minimumX;
        //    }
        //}

        // 根据鼠标移动的快慢(增量), 获得相机上下旋转的角度(处理Y) 
        rotationY -= Input.GetAxis("Mouse Y") * sensitivityY;
        //rotationY = Mathf.Clamp (rotationY, minimumY, maximumY); 

        // 总体设置一下相机角度  
        transform.localEulerAngles = new Vector3(0, rotationX, 0);
        camera.transform.localEulerAngles = new Vector3(rotationY, 0, 0);

        //if (transform.eulerAngles.y > 60 && transform.eulerAngles.y < 220) { 
        //    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 60, transform.eulerAngles.z);
        //} else if (transform.eulerAngles.y >= 220 && transform.eulerAngles.y < 320) { 
        //    transform.eulerAngles = new Vector3(transform.eulerAngles.x, 320, transform.eulerAngles.z);
        //}

        //if (camera.transform.localEulerAngles.x > 20 && camera.transform.localEulerAngles.x < 220) { 
        //    camera.transform.localEulerAngles = new Vector3(20, 0
        //                , 0);
        //} else if (camera.transform.localEulerAngles.x >= 220 && camera.transform.localEulerAngles.x < 330) { 
        //    camera.transform.localEulerAngles = new Vector3(330, 0
        //                , 0);
        //}
    }
}
