using UnityEngine;
using System.Collections;
using Game.Const;

public class BaseObject : MonoBehaviour {

    public virtual void BeHit(int val) {}

    public virtual bool IsDeath() { return true; }

    public virtual void Death(bool isSuicide) {
    }

    public virtual int GetType() { 
        return -1;
    }
}
