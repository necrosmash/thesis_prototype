using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class HealthDisplay : MonoBehaviour
{
    //private Image heart1, heart2, heart3;
    //private List<Image> hearts;

    //[SerializeField]
    //private Sprite heartFull, heartEmpty;

    //private int health;
    private PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetPlayerController();
        /*heart1 = this.transform.Find("Heart1").GetComponent<Image>();
        heart2 = this.transform.Find("Heart2").GetComponent<Image>();
        heart3 = this.transform.Find("Heart3").GetComponent<Image>();
        hearts = new List<Image>() {
            heart1, heart2 , heart3
        };*/

        //GameOverCalled = false;
        //health = 3;
    }
    /*
    public void TakeDamage(int qty = 10)
    {
        health = health - qty < 0 ? 0 : health - qty;
        renderHealth();
    }

    public void Heal(int qty = 10)
    {
        health = health + qty > 3 ? 3 : health + qty;
        renderHealth();
    }
    */
    private void Update()
    {
        if (pc is null) pc = GetPlayerController();
        if (pc is null) return;

        Debug.Log(pc.health);
        //DamageKeystroke(); // for testing

        //if (health < 1 && !GameOverCalled) GameOver();
    }

    /*private PlayerController GetPlayerController()
    {
        try
        {
            return GameObject.Find("Player(Clone)").GetComponent<PlayerController>();
        }
        catch (NullReferenceException e) {
            Debug.Log("Can't find player yet, " + e.Message);
            return null;
        }
    }*/

    private PlayerController GetPlayerController() => GameObject.Find("Player(Clone)")?.GetComponent<PlayerController>();

    /*private void GameOver()
    {
        GameOverCalled = true;
        SceneManager.LoadScene("Scenes/MenuScene", LoadSceneMode.Additive);
    }*/

    /*private void renderHealth()
    {
        // full hearts for the health we have
        // empty hearts for the health we don't have
        for (int i = hearts.Count - 1; i >= 0; i--)
        {
            if (health > i) hearts[i].sprite = heartFull;
            else hearts[i].sprite = heartEmpty;
        }
    }*/

}