using BasicScript;
using UnityEngine;
using Game.Core;
using Game.Const;
using System.Collections.Generic;

/// <summary>
/// Author: NicolasTse
/// Email: xiehaojiejob@qq.com
/// </summary>
namespace Game.Manager {

    public class KeyInputManager : Singleton<KeyInputManager> {

        public KeyInputManager() {
        }

        public void Update() {
            // PC上需要的按键侦听
            if (Input.GetKeyDown(KeyCode.J)) {
                BtnADown();
            } else if (Input.GetKeyUp(KeyCode.J)) {
                BtnAUp();
            } else if (Input.GetKeyUp(KeyCode.K)) {
                BtnBDown();
            } else if (Input.GetKeyUp(KeyCode.L)) {
                BtnCDown();
            }
        }

        public void BtnADown() {
            Fire(new BaseEvent(EventName.BTN_A_DOWN));
        }

        public void BtnAUp() { 
            Fire(new BaseEvent(EventName.BTN_A_UP));
        }

        public void BtnBDown() { 
            Fire(new BaseEvent(EventName.BTN_B_DOWN));
        }

        public void BtnCDown() { 
            Fire(new BaseEvent(EventName.BTN_C_DOWN));
        }
    }
}