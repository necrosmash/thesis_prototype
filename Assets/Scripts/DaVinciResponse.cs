using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DaVinciResponse : APIResponse
{
    public Choice[] choices;

    public override void ParseBattleInfo()
    {
        // choices[0] contains "text", "index", etc.
        if (choices[0] == null)
        {
            throw new BattleInfoNotFoundException();
        }

        battleInfo = JsonUtility.FromJson<BattleInfo>(choices[0].text);
    }

    [System.Serializable]
    public class Choice
    {
        public string text;
        public int index;
        public string logprobs;
        public string finish_reason;
    }
}