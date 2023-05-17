using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class OpenAiApi : MonoBehaviour
{
    public APIResponse response { get; private set; }

    private List<Message> previousMessages;

    public Model model;

    private string apiKey;

    private string daVinciUrl = "https://api.openai.com/v1/completions";
    private string chatGptUrl = "https://api.openai.com/v1/chat/completions";

    private bool isFirstPost;

    public static bool isPostInProgress { get; private set; }

    public enum Model
    {
        davinci,
        chatgpt,
        test
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            apiKey = new StreamReader(Application.dataPath + "/OpenAiApiKey.txt").ReadToEnd();
        }
        catch (Exception e)
        {
            Debug.LogError("ERROR: Could not find OpenAiAPI key. Make sure you have added your key to \"thesis_prototype1_Data\\OpenAiApiKey.txt\"  Defaulting to test model.");
        }
        finally
        {
            previousMessages = new List<Message>();
            string prompt = Regex.Replace("I want you to return me a JSON object. All of your output should be a part of the JSON object. Do not output any text except for the JSON object. Here is the example of the JSON object: " + JsonUtility.ToJson(new BattleInfo()) + " \"weapon\" must be one of the following values: \"sword\", \"hammer\", \"bow\". It cannot be anything else. \"size\" must be one of the following values: \"small\", \"medium\", \"large\". It cannot be anything else. Orc names must also include a descriptor starting with \"the\", such as \"Uzguk the Undefeated\". Be creative about these descriptors. Describe the opening scene in the \"openingScene\" string. The opening scene must be a story about an elf about to engage in a battle with a group of orcs. It is the beginning of the story only. Be creative when you come up with descriptions of the orcs. Populate the \"orcs\" array based on the opening scene. ", "\"", "\\\"");
            isFirstPost = true;
            isPostInProgress = true;
            Post(prompt);
        }
    }

    public void Post(string prompt)
    {
        if (apiKey == null) model = Model.test;

        switch (model)
        {
            case Model.chatgpt:
                ChatGPTPost chatGPTPost = new ChatGPTPost();
                chatGPTPost.model = "gpt-3.5-turbo";
                chatGPTPost.messages = new Message[previousMessages.Count + 1];
                
                for (int i = 0; i < previousMessages.Count; i++)
                {
                    chatGPTPost.messages[i] = new Message(previousMessages[i].role, previousMessages[i].content);
                }
                Message nextMessage = new Message("user", prompt);
                chatGPTPost.messages[chatGPTPost.messages.Length - 1] = nextMessage;
                previousMessages.Add(nextMessage);

                string newChatGPTPostData = JsonUtility.ToJson(chatGPTPost);
                StartCoroutine(Post(GenerateRequest(chatGptUrl, newChatGPTPostData)));
                
                break;
            case Model.davinci:
                string davinciPostData = "{\"model\": \"text-davinci-003\", \"prompt\": \"" + prompt + "\", \"temperature\": 0.7}";
                
                StartCoroutine(Post(GenerateRequest(daVinciUrl, davinciPostData)));

                break;
            default:
            case Model.test:
                response = DaVinciResponse.GenerateTestResponse();
                response.ParseBattleInfo();
                isPostInProgress = false;
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
        isPostInProgress = true;

        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (model.Equals(Model.davinci))
            {
                response = JsonUtility.FromJson<DaVinciResponse>(request.downloadHandler.text);
                /*foreach (DaVinciResponse.Choice choice in ((DaVinciResponse) response).choices)
                {
                    Debug.Log("dvr: " + choice.text);
                }*/ // for debugging
            }
            else
            {
                response = JsonUtility.FromJson<ChatGPTResponse>(request.downloadHandler.text);
                foreach (ChatGPTResponse.Choice choice in ((ChatGPTResponse) response).choices)
                {
                    /*Debug.Log("cgr role: " + choice.message.role);
                    Debug.Log("cgr content: " + choice.message.content);*/ // for debugging
                    previousMessages.Add(new Message(choice.message.role, choice.message.content));
                }
            }

            Debug.Log("{ prompt_tokens: " + response.usage.prompt_tokens + " }, { completion_tokens: " + response.usage.completion_tokens + " }, { total_tokens: " + response.usage.total_tokens + " }");

            if (isFirstPost)
            {
                response.ParseBattleInfo();
                isFirstPost = false;
            }
            else response.ParseLogString();
            
            Debug.Log("API call complete");
        }
        
        isPostInProgress = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
