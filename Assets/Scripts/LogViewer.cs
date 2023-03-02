using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogViewer : MonoBehaviour
{
    private bool openingSceneRendered;

    [SerializeField]
    private OpenAiApi apiClient;

    private TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        openingSceneRendered = false;
        inputField = this.GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!openingSceneRendered && apiClient.response.battleInfo.openingScene != null)
        {
            inputField.text = apiClient.response.battleInfo.openingScene;
            inputField.text = "this is some text added after, I guess we can add new text at the top?\n" + inputField.text;
            openingSceneRendered = true;
        }
    }
}
