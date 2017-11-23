using BasicScript;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: NicolasTse
/// Email: xiehaojiejob@qq.com
/// </summary>
namespace BasicScript.Component {

    public class SplashScreen : MonoBehaviour {
    
        public Image splashImg;
        public Text splashText;
        private float alpha = 0;
        private int stage = 0;

        void Start() {
        }

        void FixedUpdate() {
            if (stage == 0) {
                alpha += 0.03f;
                if (alpha > 4.5f) {
                    stage = 1;
                    alpha = 1;
                }
                splashImg.color = new Color(splashImg.color.r, splashImg.color.g, splashImg.color.b, alpha);
                splashText.color = new Color(splashText.color.r, splashText.color.g, splashText.color.b, alpha);
            } else {
                alpha -= 0.015f;
                if (alpha < 0) {
                    stage = 2;
                    alpha = 0;
                    SceneManager.LoadScene("Main");    
                }
                splashImg.color = new Color(splashImg.color.r, splashImg.color.g, splashImg.color.b, alpha);
                splashText.color = new Color(splashText.color.r, splashText.color.g, splashText.color.b, alpha);
            }
        }
    }
}
