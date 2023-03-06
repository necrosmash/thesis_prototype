using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private Image heart1, heart2, heart3;
    private List<Image> hearts;

    [SerializeField]
    private Sprite heartFull, heartEmpty;

    private int health;

    // Start is called before the first frame update
    void Start()
    {
        heart1 = this.transform.Find("Heart1").GetComponent<Image>();
        heart2 = this.transform.Find("Heart2").GetComponent<Image>();
        heart3 = this.transform.Find("Heart3").GetComponent<Image>();
        hearts = new List<Image>() {
            heart1, heart2 , heart3
        };

        health = 3;
    }

    public void TakeDamage(int qty = 1)
    {
        health = health - qty < 0 ? 0 : health - qty;
        renderHealth();

        if (health < 1) GameOver();
    }

    public void Heal(int qty = 1)
    {
        health = health + qty > 3 ? 3 : health + qty;
        renderHealth();
    }

    // not used now, but can be uncommented to demonstrate the effect.
    // just change `health` in `Start()` to whatever value you want to display.
    /*void Update()
    {
        renderHealth();
    }*/

    private void GameOver()
    {
        Debug.Log("GAME OVER!! OH SHIT!!!!");
    }

    private void renderHealth()
    {
        // full hearts for the health we have
        // empty hearts for the health we don't have
        for(int i = hearts.Count - 1; i >= 0; i--)
        {
            if (health > i) hearts[i].sprite = heartFull;
            else hearts[i].sprite = heartEmpty;
        }
    }
}