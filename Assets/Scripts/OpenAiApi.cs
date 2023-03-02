using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class OpenAiApi : MonoBehaviour
{
    APIResponse response;
    string prompt;

    public Model model;
    public string apiKey;

    private string daVinciUrl = "https://api.openai.com/v1/completions";
    private string chatGptUrl = "https://api.openai.com/v1/chat/completions";

    public enum Model
    {
        davinci,
        chatgpt
    }

    // Start is called before the first frame update
    void Start()
    {
        prompt = Regex.Replace("I want you to return me a JSON object. All of your output should be a part of the JSON object. Do not output any text except for the JSON object. Here is the example of the JSON object: " + JsonUtility.ToJson(new BattleInfo()) + " \"weapon\" must be one of the following values: \"sword\", \"hammer\", \"bow\". It cannot be anything else. \"size\" must be one of the following values: \"small\", \"medium\", \"large\". It cannot be anything else. Orc names must also include a descriptor starting with \"the\", such as \"Uzguk the Undefeated\". Be creative about these descriptors. Describe the opening scene in the \"openingScene\" string. The opening scene must be a story about an elf about to engage in a battle with a group of orcs. Be creative when you come up with descriptions of the orcs. Populate the \"orcs\" array based on the opening scene. ", "\"", "\\\"");
        Post(model);
    }

    private void Post(Model m)
    {
        string davinciPostData = "{\"model\": \"text-davinci-003\", \"prompt\": \"" + prompt + "\", \"temperature\": 0.7, \"max_tokens\": 3000}";
        string chatgptPostData = "{\"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"user\", \"content\": \"" + prompt + "\"}]}";

        switch (m)
        {
            case Model.chatgpt:
                StartCoroutine(Post(GenerateRequest(chatGptUrl, chatgptPostData)));
                break;
            default:
            case Model.davinci:
                StartCoroutine(Post(GenerateRequest(daVinciUrl, davinciPostData)));
                break;
        }
    }

    private UnityWebRequest GenerateRequest(string url, string postData)
    {
        UnityWebRequest request = UnityWebRequest.Post(url, postData, "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        return request;
    }

    private IEnumerator Post(UnityWebRequest request)
    {
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (model.Equals(Model.davinci))
                response = JsonUtility.FromJson<DaVinciResponse>(request.downloadHandler.text);
            else
                response = JsonUtility.FromJson<ChatGPTResponse>(request.downloadHandler.text);

            response.ParseBattleInfo();
            Debug.Log("API call complete");
            Debug.Log(response.battleInfo.openingScene);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
