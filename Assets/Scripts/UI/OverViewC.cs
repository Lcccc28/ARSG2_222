﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Game.Manager;

public class OverViewC : OverView {

    public override void OnBtnRestartClick() {
        GameSceneManager.Instance.LoadSceneAsync("ModelC");
    }
}