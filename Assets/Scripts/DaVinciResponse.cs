using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        logString = battleInfo.openingScene;
    }

    public override void ParseLogString()
    {
        logString = choices[0].text;
    }

    [System.Serializable]
    public class Choice
    {
        public string text;
        public int index;
        public string logprobs;
        public string finish_reason;
    }

    public static DaVinciResponse GenerateTestResponse()
    {
        DaVinciResponse response = new DaVinciResponse();

        response.battleInfo = new BattleInfo();
        response.battleInfo.openingScene = "The elf stands alone in the clearing, his heart pounding as he faces the three orcs that have cornered him. In the lead is a giant, hulking orc, Uzguk the Undefeated, who wields a huge two-handed sword. Behind him stands Grimgor the Ruthless, a much smaller orc who carries a heavy hammer. Last but not least is Krok the Swift, a lithe and agile orc who carries a bow and a quiver of arrows.";

        response.battleInfo.orcs = new BattleInfo.Orc[3];
        response.battleInfo.orcs[0] = new BattleInfo.Orc();
        response.battleInfo.orcs[0].description = "A giant, hulking orc, Uzguk stands at least eight feet tall and appears to be made of pure muscle. He wears heavy armor, and wields a huge two-handed sword.";
        response.battleInfo.orcs[0].name = "Uzguk the Undefeated";
        response.battleInfo.orcs[0].size = "large";
        response.battleInfo.orcs[0].weapon = "sword";
        response.battleInfo.orcs[0].sizeEnum = BattleInfo.Orc.Size.Large;
        response.battleInfo.orcs[0].weaponEnum = BattleInfo.Orc.Weapon.Sword;

        response.battleInfo.orcs[1] = new BattleInfo.Orc();
        response.battleInfo.orcs[1].description = "Grimgor is a much smaller orc, but is no less menacing. He has a wild look in his eyes and carries a heavy hammer.";
        response.battleInfo.orcs[1].name = "Grimgor the Ruthless";
        response.battleInfo.orcs[1].size = "medium";
        response.battleInfo.orcs[1].weapon = "hammer";
        response.battleInfo.orcs[1].sizeEnum = BattleInfo.Orc.Size.Medium;
        response.battleInfo.orcs[1].weaponEnum = BattleInfo.Orc.Weapon.Hammer;


        response.battleInfo.orcs[2] = new BattleInfo.Orc();
        response.battleInfo.orcs[2].description = "Krok is a lithe and agile orc who carries a bow and a quiver of arrows.";
        response.battleInfo.orcs[2].name = "Krok the Swift";
        response.battleInfo.orcs[2].size = "small";
        response.battleInfo.orcs[2].weapon = "bow";
        response.battleInfo.orcs[2].sizeEnum = BattleInfo.Orc.Size.Small;
        response.battleInfo.orcs[2].weaponEnum = BattleInfo.Orc.Weapon.Bow;

        response.created = "1677767171";
        response.id = "cmpl-6peAlHEEi7BavkFMc6phwg6VMIefN";
        response.model = "text-davinci-003";

        response.usage = new Usage();
        response.usage.completion_tokens = 334;
        response.usage.prompt_tokens = 204;
        response.usage.total_tokens = 538;

        response.choices = new Choice[1];
        response.choices[0] = new Choice();
        response.choices[0].finish_reason = "stop";
        response.choices[0].index = 0;
        response.choices[0].logprobs = "";
        response.choices[0].text = "\n\n{\n    \"orcs\": [\n        {\n            \"name\": \"Uzguk the Undefeated\",\n            \"description\": \"A giant, hulking orc, Uzguk stands at least eight feet tall and appears to be made of pure muscle. He wears heavy armor, and wields a huge two-handed sword.\",\n            \"weapon\": \"sword\",\n            \"size\": \"large\"\n        },\n        {\n            \"name\": \"Grimgor the Ruthless\",\n            \"description\": \"Grimgor is a much smaller orc, but is no less menacing. He has a wild look in his eyes and carries a heavy hammer.\",\n            \"weapon\": \"hammer\",\n            \"size\": \"medium\"\n        },\n        {\n            \"name\": \"Krok the Swift\",\n            \"description\": \"Krok is a lithe and agile orc who carries a bow and a quiver of arrows.\",\n            \"weapon\": \"bow\",\n            \"size\": \"small\"\n        }\n    ],\n    \"openingScene\": \"The elf stands alone in the clearing, his heart pounding as he faces the three orcs that have cornered him. In the lead is a giant, hulking orc, Uzguk the Undefeated, who wields a huge two-handed sword. Behind him stands Grimgor the Ruthless, a much smaller orc who carries a heavy hammer. Last but not least is Krok the Swift, a lithe and agile orc who carries a bow and a quiver of arrows.\"\n}";

        return response;
    }
}