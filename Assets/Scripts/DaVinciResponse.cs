using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DaVinciResponse
{
    public string id;
    public string objectProperty; // doesn't work right now, but might not be needed
    public string created;
    public string model;
    public Choice[] choices;
    public Usage usage;
    public BattleInfo battleInfo;

    public void ParseBattleInfo()
    {
        // choices[0] is "text"
        if (choices[0] == null)
        {
            throw new BattleInfoNotFoundException();
        }

        battleInfo = JsonUtility.FromJson<BattleInfo>(choices[0].text);
    }
}

[System.Serializable]
public class Choice
{
    public string text;
    public int index;
    public string logprobs;
    public string finish_reason;
}

[System.Serializable]
public class Usage
{
    public int prompt_tokens;
    public int completion_tokens;
    public int total_tokens;
}

[System.Serializable]
public class BattleInfoNotFoundException : System.Exception { }