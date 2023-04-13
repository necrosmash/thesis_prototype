using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class HealthDisplay : MonoBehaviour
{
    private PlayerController pc;

    private Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetPlayerController();
        healthBar = this.transform.Find("HealthBarInner").GetComponent<Image>();
    }

    private void Update()
    {
        if (pc is null) pc = GetPlayerController();
        if (pc is null) return;

        healthBar.fillAmount = Mathf.Clamp((float) pc.health / (float) pc.maxHealth, 0, 1);
    }

    private PlayerController GetPlayerController() => GameObject.Find("Player(Clone)")?.GetComponent<PlayerController>();

}