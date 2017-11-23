using UnityEngine;
using System.Collections;
using Game.Manager;
using Game.Core;
using Game.Const;
using BehaviorDesigner.Runtime;

public class Fruit : Enemy  {

    private Rigidbody rigidbody;
    private float speed = 0;
    private float speedX = 0;
    private float initialSpeed = 8;
    private float acceleration = 6;
    private float fallTime = 0;

    void Awake() {
        data = new EnemyData();
        transform.localEulerAngles = new Vector3(Random.Range(-90, 90),Random.Range(-90, 90),Random.Range(-90, 90));
        speedX = Random.Range(0.1f, 0.18f);
    }

    void FixedUpdate() {
        fallTime += Time.fixedDeltaTime;
        speed = initialSpeed - fallTime * acceleration;
        if (speed < -10) 
            speed = -10;

        transform.position += new Vector3(speedX, speed * Time.fixedDeltaTime, 0);

        if (fallTime > 4) { 
            Death(true);
        }
    }

    public override void BeHit(int val) {
        if (data.type == ObjectType.MONS) {
            CreateHalf(true);
            CreateHalf(false);
            Death(false);
        } else { 
            Death(false);
        }
    }

    public override void Death(bool isSuicide)
    {
        GlobalEventSystem.Fire(new BaseEvent(EventName.MONS_DEATH, new object[]{ this, isSuicide }));
        GameObject.Destroy(gameObject);
    }

    private void CreateHalf(bool isLeft) { 
        float v = Random.Range(2f, 4f);
        Transform child = transform.GetChild(isLeft ? 0 : 1);
        GameObject gameObj = GameObject.Instantiate(child.gameObject);
        gameObj.transform.position = child.position;
        gameObj.transform.localScale = child.localScale;
        gameObj.transform.rotation = child.rotation;

        Rigidbody rigidbody = gameObj.gameObject.AddComponent<Rigidbody>();
        Vector3 localForward = transform.TransformPoint(transform.forward * 5);
        Vector3 vecSpeed = localForward - transform.position;
        
        //localForward = gameObj.transform.TransformPoint(gameObj.transform.forward * 5);
        //vecSpeed = localForward - gameObj.transform.position;
        
        if (isLeft) {
            if (transform.forward.y > 0) {
                rigidbody.velocity = -new Vector3(vecSpeed.x, vecSpeed.y, vecSpeed.z);
            } else { 
                rigidbody.velocity = new Vector3(vecSpeed.x, vecSpeed.y, vecSpeed.z);
            }
        } else { 
            if (transform.forward.y > 0) {
                rigidbody.velocity = new Vector3(vecSpeed.x, vecSpeed.y, vecSpeed.z);
            } else { 
                rigidbody.velocity = -new Vector3(vecSpeed.x, vecSpeed.y, vecSpeed.z);
            }
        }
        GameObject.Destroy(gameObj, 5f);
    }
}
