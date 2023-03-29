using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChatGPTResponse : APIResponse
{
    public Choice[] choices;

    public override void ParseBattleInfo()
    {
        // choices[0] contains "message"
        if (choices[0] == null)
        {
            throw new BattleInfoNotFoundException();
        }

        battleInfo = JsonUtility.FromJson<BattleInfo>(choices[0].message.content);
        logString = battleInfo.openingScene;
    }

    public override void ParseLogString()
    {
        logString = choices[0].message.content;
    }

    [System.Serializable]
    public class Choice
    {
        public Message message;
        public int index;
        public string finish_reason;
    }
}