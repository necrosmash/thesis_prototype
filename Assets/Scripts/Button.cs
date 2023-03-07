using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void onRestart()
    {
        SceneManager.LoadScene("Scenes/SampleScene");
    }

    public void onExit()
    {
        Application.Quit();
    }
}
