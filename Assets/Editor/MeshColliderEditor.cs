using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Const;

public class MeshColliderEditor
{
    [MenuItem("ARSGTools/AddMeshCollider")]
    static void AddMeshCollider() {
        Object[] selection = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets);
        foreach (var item in selection) {
            GameObject sceneRoot = (GameObject)item;
            MeshRenderer[] allMesh = sceneRoot.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < allMesh.Length; ++i) { 
                MeshCollider coll = allMesh[i].GetComponent<MeshCollider>();
                if (coll == null) { 
                    coll = allMesh[i].gameObject.AddComponent<MeshCollider>();
                    coll.convex = true;
                }
                coll.gameObject.layer = LayerConst.WALL;
            }
        }
    }

	[MenuItem("ARSGTools/DelCollider")]
	static void DelCollider() {
		Object[] selection = Selection.GetFiltered(typeof(GameObject), SelectionMode.Deep);
		foreach (var item in selection) {
			GameObject sceneRoot = (GameObject)item;
			GameObject.DestroyImmediate(sceneRoot.GetComponent<Collider>());
		}
	}

	[MenuItem("ARSGTools/SetColliderWall")]
	static void SetColliderWall() {
		Object[] selection = Selection.GetFiltered(typeof(GameObject), SelectionMode.Deep);
		foreach (var item in selection) {
			GameObject sceneRoot = (GameObject)item;
			if(sceneRoot.GetComponent<Collider>() != null){
				sceneRoot.layer = LayerConst.WALL;
			}
		}
	}
}
