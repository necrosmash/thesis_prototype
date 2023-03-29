using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogViewer : MonoBehaviour
{
    [SerializeField]
    private OpenAiApi apiClient;

    private string mostRecentResponse;

    private TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        inputField = this.GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (apiClient.response == null || apiClient.response.logString.Equals(mostRecentResponse)) return;

        mostRecentResponse = apiClient.response.logString;
        inputField.text = mostRecentResponse + "\n\n" + inputField.text;
    }
}
