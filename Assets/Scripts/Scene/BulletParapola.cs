using UnityEngine;
using System.Collections;
using Game.Util;
using DG.Tweening;

public class BulletParapola : Bullet {
	public float randomRange = 2f;
	void Start() { 
		lifeTime = Vector3.Distance(target.transform.position, transform.position) / data.speed;

		Bezier bezier = new Bezier(100, transform.position, (target.transform.position + transform.position) * 0.5f + new Vector3(Random.Range(-randomRange, randomRange), 2f, Random.Range(-randomRange, randomRange)), target.transform.position + new Vector3(0, -0.5f));
		transform.DOPath(bezier.vertexs.ToArray(), lifeTime, PathType.Linear, PathMode.Full3D, 10, Color.red).SetLookAt(0.01f);
	}

	void FixedUpdate () {
		lifeTime -= Time.fixedDeltaTime;
		if (lifeTime < 0) { 
			Destroy();
		}
	}

}
