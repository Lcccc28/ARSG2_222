using System.Collections.Generic;
using Game.Core;
using PathologicalGames;
using UnityEngine;

/// <summary>
/// Author: NicolasTse
/// Email: xiehaojiejob@qq.com
/// </summary>
namespace Game.Manager {

    public class GamePoolManager : Singleton<GamePoolManager> {

        public Transform pool;
        private SpawnPool prefabPool;

        public GamePoolManager() {
        }

        public void Init() {
            pool = GameObject.Find("Pool").transform;

            pool.gameObject.SetActive(false);

            prefabPool = pool.gameObject.AddComponent<SpawnPool>();
            prefabPool.poolName = "PrefabPool";

            pool.gameObject.SetActive(true);

            int childCount = pool.childCount;
            for (int i = 0; i < childCount; ++i) {
                Transform trans = pool.GetChild(i);
                InitPrefabPool(trans.name, trans);
            }
        }

        public void InitPrefabPool(string path, Transform prefab, int preloadAmount = 1, int limitAmount = 50, int cullAbove = 5, int cullMaxPerPass = 5) {
            if (!Contains(path)) {
                PrefabPool pool = new PrefabPool(prefab);
                pool.name = path;
                pool.preloadAmount = preloadAmount; // 预加载个数
                pool.limitInstances = true; // 是否限制
                pool.limitFIFO = true; // 关闭无限取prefab
                pool.limitAmount = limitAmount; // 限制池子里最大的Prefab数量
                pool.cullDespawned = true; // 自动清除缓冲池
                pool.cullAbove = cullAbove; // 最终保留
                pool.cullDelay = 10; // 多久清理一次
                pool.cullMaxPerPass = cullMaxPerPass; // 每次清理几个
                prefabPool.CreatePrefabPool(pool);
            }
        }

        public void RemovePrefabPool(string name) {
            prefabPool.RemovePrefabPool(name);
        }

        public bool Contains(string path) {
            return prefabPool.prefabs.ContainsKey(path);
        }

        public bool HadDespawn(Transform obj) {
            if (prefabPool._spawned.Contains(obj)) {
                return false;
            }
            return true;
        }

        public Transform Spawn(string path) {
            return prefabPool.Spawn(path);
        }

        public void Despawn(Transform obj) {
            if (obj)
                prefabPool.Despawn(obj, pool);
            else 
                Trace.LogWarning("特效丢失");
        }

        public void Despawn(Transform obj, float time) {
            if (obj)
                prefabPool.Despawn(obj, time, pool);
            else 
                Trace.LogWarning("特效丢失");
        }

        public void PutBackPool(Transform obj) {
            obj.SetParent(pool, true);
        }

        public void ClearPerfabPool() {
            prefabPool.Clear();
        }

        public void DespawnAllToPool() {
            var spawned = new List<Transform>(prefabPool._spawned);
            for (int i = 0; i < spawned.Count; ++i) {
                if (spawned[i]) {
                    Despawn(spawned[i]);
                }
            }
        }

        public Transform GetPrefabFromPool(string path) {
            if (prefabPool.prefabs.ContainsKey(path)) {
                return prefabPool.prefabs[path];
            }
            return null;
        }
    }
}
