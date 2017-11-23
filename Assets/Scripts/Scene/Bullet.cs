using UnityEngine;
using System.Collections;
using Game.Manager;
using Game.Const;

public class Bullet : MonoBehaviour {

    public Vector3 velocity;
	public float lifeTime; // 子弹生存时间
	public BaseObject owner; // 子弹的发射者
	public BaseObject target; // 子弹的目标
    public BulletData data;
    public int soundId;

    void Start() { 
        velocity = transform.forward * data.speed;
		lifeTime = data.lifeTime;
    }

	void FixedUpdate () {
        transform.position += velocity * Time.fixedDeltaTime;
        lifeTime -= Time.fixedDeltaTime;
        if (lifeTime < 0) { 
            Destroy();
        }
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerConst.WALL) { 
            ShowHitEffect(GamePoolManager.Instance.Spawn("Hit").gameObject);
            Destroy();
            return;
        }

		if (owner != null)  // 可能已经挂掉了
        	if (other.gameObject == owner.gameObject) return;

        if (owner.GetType() == ObjectType.PLAYER) {
            if (other.gameObject.layer == LayerConst.ENEMY) { 
                Enemy enemy = other.transform.GetComponent<Enemy>();
                if (enemy != null) { 
                    enemy.BeHit(data.atk);
                    if (data.hitModelName != "")
                    {
                        ShowHitEffect(GamePoolManager.Instance.Spawn(data.hitModelName).gameObject);
                    }
                    Destroy();
                }
            } else if (other.gameObject.layer == LayerConst.FRUIT) { 
                Fruit fruit = other.transform.GetComponent<Fruit>();
                if (fruit != null) { 
                    fruit.BeHit(data.atk);
                    Destroy();
                }
            }
        } else if (other.gameObject.layer == LayerConst.PLAYER && owner.GetType() == ObjectType.MONS) { 
            Player player = other.transform.GetComponent<Player>();
            if (player != null) { 
                player.BeHit(data.atk);
                if (data.hitModelName != "")
                {
                    ShowHitEffect(GamePoolManager.Instance.Spawn(data.hitModelName).gameObject);
                }
                Destroy();
            }
        }
    }

    private void ShowHitEffect(GameObject hit) { 
        hit.SetActive(true);
        hit.transform.position = this.transform.position + new Vector3(Random.Range(0,0.5f), Random.Range(0, 0.5f));
		GamePoolManager.Instance.Despawn(hit.transform, data.hitDestroyTime);
    }


	protected void Destroy() { 
        AudioManager.Instance.StopSound(soundId);
        GamePoolManager.Instance.Despawn(this.transform);
    }
}

