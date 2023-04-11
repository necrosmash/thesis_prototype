using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogViewer : MonoBehaviour
{
    [SerializeField]
    private OpenAiApi apiClient;

    private string mostRecentResponse;

    [SerializeField]
    private ThesisChatController cc;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (apiClient.response == null || apiClient.response.logString.Equals(mostRecentResponse)) return;

        mostRecentResponse = apiClient.response.logString;
        cc.AddToChatOutput(mostRecentResponse);
    }
}
