using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class OpenAiApi : MonoBehaviour
{
    DaVinciResponse dvr;
    string prompt;

    // Start is called before the first frame update
    void Start()
    {
        prompt = Regex.Replace("I want you to return me a JSON object. All of your output should be a part of the JSON object. Do not output any text except for the JSON object. Here is the example of the JSON object: " + JsonUtility.ToJson(new BattleInfo()) + " \"weapon\" must be one of the following values: \"sword\", \"hammer\", \"bow\". It cannot be anything else. \"size\" must be one of the following values: \"small\", \"medium\", \"large\". It cannot be anything else. Orc names must also include a descriptor starting with \"the\", such as \"Uzguk the Undefeated\". Be creative about these descriptors. Describe the opening scene in the \"openingScene\" string. The opening scene must be a story about an elf about to engage in a battle with a group of orcs. Be creative when you come up with descriptions of the orcs. Populate the \"orcs\" array based on the opening scene. ", "\"", "\\\"");
        StartCoroutine(Post());
    }

    IEnumerator Post()
    {
        // hardcode values, should be configurable?
        // could just be through the inspector.
        string postData = "{\"model\": \"text-davinci-003\", \"prompt\": \"" + prompt + "\", \"temperature\": 0.7, \"max_tokens\": 3000}";
        UnityWebRequest request = UnityWebRequest.Post("https://api.openai.com/v1/completions", postData, "application/json");
        request.SetRequestHeader("Authorization", "Bearer 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");

        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            dvr = JsonUtility.FromJson<DaVinciResponse>(request.downloadHandler.text);
            dvr.ParseBattleInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
