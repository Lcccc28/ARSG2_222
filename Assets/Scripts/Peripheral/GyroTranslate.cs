using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Manager;

public class GyroTranslate : MonoBehaviour
{
    public new GameObject camera;
    public Boolean Enabled {
        get { return gyro.enabled; }
        set { gyro.enabled = value; }
    }

    bool gyinfo;
    Gyroscope gyro;
    Quaternion quatMult;

    private Vector3 acceleration;
	private Vector3 velocity;
	private float cd; 
	private int sceneId; 

    void Start()
    {
        gyinfo = SystemInfo.supportsGyroscope;
        gyro = Input.gyro;
        gyro.enabled = true;

        sceneId = GameSceneManager.Instance.GetSceneId();
    }

    //private float time = 0;

    void Update()
    {
        // y 340 50 x 
        //time += Time.fixedDeltaTime;
        //if (true)
        if (gyinfo && gyro.enabled)
        {
            Vector3 a = gyro.attitude.eulerAngles;

            //if (-a.y > 40 && -a.y < 220) {
            //    transform.eulerAngles = new Vector3(-a.x, 40, a.z); //直接使用读取的欧拉角发现不对，于是自己调整一下符号
            //} else if (-a.y > 220 && -a.y < 340) {
            //    transform.eulerAngles = new Vector3(-a.x, 340, a.z); //直接使用读取的欧拉角发现不对，于是自己调整一下符号
            //} else { 
                transform.eulerAngles = new Vector3(-a.x, -a.y, a.z); //直接使用读取的欧拉角发现不对，于是自己调整一下符号    
            //}
            
            transform.Rotate(Vector3.right * 90, Space.World);
			transform.Rotate(Vector3.up * 90, Space.World);

            Vector3 lookTorward = transform.TransformPoint(Vector3.forward);
            if (sceneId == 1) {
                if (Vector3.Project(transform.forward, Vector3.forward).z < 0) { 
                    lookTorward = new Vector3(lookTorward.x, lookTorward.y, transform.position.z);
                }
            } else if (sceneId == 3) { 

            }

            Vector3 lookParallel = new Vector3(lookTorward.x, transform.position.y, lookTorward.z);
            //lookParallel = new Vector3(time, time, time);

            transform.LookAt(lookParallel);
            camera.transform.LookAt(lookTorward);

			if (sceneId == 3) {
				if (transform.eulerAngles.y > 60 && transform.eulerAngles.y < 220) { 
					transform.eulerAngles = new Vector3 (transform.eulerAngles.x, 60, transform.eulerAngles.z);
				} else if (transform.eulerAngles.y >= 220 && transform.eulerAngles.y < 320) { 
					transform.eulerAngles = new Vector3 (transform.eulerAngles.x, 320, transform.eulerAngles.z);
				}

				if (camera.transform.localEulerAngles.x > 20 && camera.transform.localEulerAngles.x < 220) { 
					camera.transform.localEulerAngles = new Vector3 (20, 0, 0);
				} else if (camera.transform.localEulerAngles.x >= 220 && camera.transform.localEulerAngles.x < 330) { 
					camera.transform.localEulerAngles = new Vector3 (330, 0, 0);
				}
			} else{
	            // 位移的
	            //acceleration = Vector3.zero;
	            //int i = 0;
	            //while (i < Input.accelerationEventCount)
	            //{
	            //    AccelerationEvent accEvent = Input.GetAccelerationEvent(i);
	            //    acceleration += accEvent.acceleration * accEvent.deltaTime;  
	            //    ++i;
	            //}
	            //X = acceleration * Time.deltaTime * Time.deltaTime / 2; // X = at^2/2

	            //this.transform.Translate(new Vector3(X.y, 0, -X.z) * speed, Space.Self);
                acceleration = Input.gyro.userAcceleration;
                Vector3 aX = Vector3.Project(transform.TransformPoint(new Vector3(-acceleration.x, -acceleration.y, -acceleration.z)) - transform.position, Vector3.right);
                if (acceleration.magnitude > 0.3f && cd <= 0)
                {
                    // velocity += aX * Time.deltaTime * speed;
                    // transform.position += velocity * Time.deltaTime;
                    if (aX.x > 0 && transform.position.x <= 12)
                    {
                        cd = 3f;
                        transform.DOMove(transform.position + new Vector3(5, 0, 0), 1f);
                    }
                    else if (aX.x < 0 && transform.position.x >= -12)
                    {
                        cd = 3f;
                        transform.DOMove(transform.position + new Vector3(-5, 0, 0), 1f);
                    }
                }
                cd -= Time.deltaTime;
            }
        }
    }

    
//    void OnGUI()
//    {
//        GUIStyle s = new GUIStyle() { fontSize = 30 };
//
//		GUI.Label(new Rect(100, 200, 200, 100), "加速x：" + acceleration.x.ToString("F2"), s);
//		GUI.Label(new Rect(100, 230, 200, 100), "加速y：" + acceleration.y.ToString("F2"), s);
//		GUI.Label(new Rect(100, 260, 200, 100), "加速z：" + acceleration.z.ToString("F2"), s);
//
//		GUI.Label(new Rect(100, 400, 200, 100), "重力x：" + Input.gyro.gravity.x.ToString("F2"), s);
//		GUI.Label(new Rect(100, 430, 200, 100), "重力y：" + Input.gyro.gravity.y.ToString("F2"), s);
//		GUI.Label(new Rect(100, 460, 200, 100), "重力z：" + Input.gyro.gravity.z.ToString("F2"), s);
//
//		GUI.Label(new Rect(100, 600, 200, 100), "x：" + transform.position.x, s);
//		GUI.Label(new Rect(100, 630, 200, 100), "y：" + transform.position.y, s);
//		GUI.Label(new Rect(100, 660, 200, 100), "z：" + transform.position.z, s);
//    }
     
}

