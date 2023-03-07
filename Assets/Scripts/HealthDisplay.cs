using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthDisplay : MonoBehaviour
{
    private Image heart1, heart2, heart3;
    private List<Image> hearts;

    [SerializeField]
    private Sprite heartFull, heartEmpty;

    private int health;

    // can use this to stop processing Update in other GOs
    public static bool GameOverCalled { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        heart1 = this.transform.Find("Heart1").GetComponent<Image>();
        heart2 = this.transform.Find("Heart2").GetComponent<Image>();
        heart3 = this.transform.Find("Heart3").GetComponent<Image>();
        hearts = new List<Image>() {
            heart1, heart2 , heart3
        };

        GameOverCalled = false;
        health = 3;
    }

    public void TakeDamage(int qty = 1)
    {
        health = health - qty < 0 ? 0 : health - qty;
        renderHealth();
    }

    public void Heal(int qty = 1)
    {
        health = health + qty > 3 ? 3 : health + qty;
        renderHealth();
    }

    private void Update()
    {
        DamageKeystroke(); // for testing

        if (health < 1 && !GameOverCalled) GameOver();
    }

    private void GameOver()
    {
        GameOverCalled = true;
        SceneManager.LoadScene("Scenes/MenuScene", LoadSceneMode.Additive);
    }

    private void renderHealth()
    {
        // full hearts for the health we have
        // empty hearts for the health we don't have
        for (int i = hearts.Count - 1; i >= 0; i--)
        {
            if (health > i) hearts[i].sprite = heartFull;
            else hearts[i].sprite = heartEmpty;
        }
    }

    private void DamageKeystroke()
    {
        if (MenuCanvas.IsRendered) return;
        if (Input.GetKeyDown(KeyCode.H)) health -= 1;
    }

}