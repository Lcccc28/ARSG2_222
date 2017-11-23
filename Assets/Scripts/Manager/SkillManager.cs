using System;
using Game.Core;
using Game.Manager;
using UnityEngine;
using System.Collections;

namespace Game.Manager {

    public class SkillManager : Singleton<SkillManager> {

		public void Fire(BaseObject owner, BaseObject target, Vector3 origin, Vector3 forward, BulletData bulletData) {
            if (bulletData.num > 1) { 
				SceneB.Instance.StartCoroutine(FireMultiBullet(owner, target, origin, forward, bulletData));
            } else { 
				FireBullet(owner, target, origin, forward, bulletData);
            }
        }

		private IEnumerator FireMultiBullet(BaseObject owner, BaseObject target, Vector3 origin, Vector3 forward, BulletData bulletData) {
            for (int i = 0; i < bulletData.num; ++i) { 
				FireBullet(owner, target, origin, forward, bulletData);
                yield return new WaitForSeconds(bulletData.interval);
            }
        }

		private void FireBullet(BaseObject owner, BaseObject target, Vector3 origin, Vector3 forward, BulletData bulletData) { 
            GameObject bullet = GamePoolManager.Instance.Spawn(bulletData.modelName).gameObject;
			Bullet script = bullet.GetComponent<Bullet>();
            
            if (script != null) {
                GameObject.DestroyImmediate(script);
            }

			switch (bulletData.script) {
			case "BulletParapola":
				script = bullet.AddComponent<BulletParapola> ();
				break;
			default:
				script = bullet.AddComponent<Bullet> ();
				break;
			}
				
            script.owner = owner;
			script.target = target;
            script.data = bulletData;
            bullet.transform.forward = forward;
            bullet.transform.position = origin;

            if (bulletData.isRandomRotation) { 
                bullet.transform.eulerAngles = bullet.transform.rotation.eulerAngles + new Vector3(0, 0, UnityEngine.Random.Range(0, 360));
            }
            bullet.SetActive(true);
            script.soundId = AudioManager.Instance.PlaySound(bulletData.sound);
        }
    }
}
