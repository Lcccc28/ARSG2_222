using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Game.Manager;

public class HPBar : MonoBehaviour {

    public Transform hpbar;
    public Enemy enemy;
    private Transform enemyHpBar;
    private Camera mainCamera;
    private Camera uiCamera;
    private RectTransform rectTrans;
    private Image slider;
    private Vector3 pos;
    private Vector3 followPosition;

    void Start() { 
        hpbar = GamePoolManager.Instance.Spawn("HPBar");
        hpbar.SetParent(GameObject.FindGameObjectWithTag("SceneUI").transform);
        hpbar.localScale = Vector3.one;  
        hpbar.gameObject.SetActive(true);

        enemy = transform.GetComponent<Enemy>();
        enemyHpBar = transform.FindChild("HpBar");
        slider = hpbar.FindChild("Slider").GetComponent<Image>();

        rectTrans = hpbar.GetComponent<RectTransform>();
        mainCamera = Camera.main;
        uiCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
    }

    public bool IsInCamera(Vector3 worldPos) {
        Transform camTransform = mainCamera.transform;
        Vector2 viewPos = mainCamera.WorldToViewportPoint(worldPos);
        Vector3 dir = (worldPos - camTransform.position).normalized;
        float dot = Vector3.Dot (camTransform.forward, dir);//判断物体是否在相机前面
  
        if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
            return true;         
        else             
            return false;
    }

    void Update() {
        if (IsInCamera(transform.position)) { 
            hpbar.gameObject.SetActive(true);
        } else { 
            hpbar.gameObject.SetActive(false);
            return;
        }

        pos = mainCamera.WorldToScreenPoint(enemyHpBar.position);
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTrans
            , pos, uiCamera, out followPosition)) {
            hpbar.position = followPosition;
        }

        if (enemy != null && enemy.data != null) { 
            slider.fillAmount = enemy.data.curHP / (float)enemy.data.maxHP;
        }
    }

    void OnDisable() { 
        if (!GamePoolManager.Instance.HadDespawn(hpbar))
            GamePoolManager.Instance.Despawn(hpbar);
    }
}
