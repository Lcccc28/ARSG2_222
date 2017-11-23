using UnityEngine;
using System.Collections;


public class GyroRotate : MonoBehaviour
{

    bool draw = false;
    bool gyinfo;
    Gyroscope go;

    void Start()
    {
        gyinfo = SystemInfo.supportsGyroscope;
        go = Input.gyro;
        go.enabled = true;
    }

    void Update()
    {
        if (gyinfo)
        {
            Vector3 a = go.attitude.eulerAngles;

            a = new Vector3(-a.x, -a.y, a.z); //直接使用读取的欧拉角发现不对，于是自己调整一下符号  
            this.transform.eulerAngles = a;
            this.transform.Rotate(Vector3.right * 90, Space.World);

            //Quaternion temp = Input.gyro.attitude;
            //temp.x *= -1;
            //temp.y *= -1;
            //transform.localRotation = temp;


            // this.transform.parent.localRotation = new Quaternion(0, 1, 0, temp.w);


            draw = false;
        }
        else
        {
            draw = true;
        }
    }

    //void OnGUI()
    //{
    //    //if (draw)
    //    //{
    //    //    GUI.Label(new Rect(100, 100, 100, 30), "启动失败");
    //    //}
    //}
}
