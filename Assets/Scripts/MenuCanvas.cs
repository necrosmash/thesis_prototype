using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCanvas : MonoBehaviour
{
    public static bool IsRendered { get; private set; }

    private void Start()
    {
        IsRendered = true;
    }

    private void OnDestroy()
    {
        IsRendered = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !HealthDisplay.GameOverCalled)
        {
            SceneManager.UnloadSceneAsync("Scenes/MenuScene");
        }
    }
}
