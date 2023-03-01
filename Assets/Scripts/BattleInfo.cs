using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleInfo
{
    public Orc[] orcs;
    public string openingScene;

    [System.Serializable]
    public class Orc
    {
        public string name;
        public string description;
        public string weapon;
        public string size;

        // needed for blank prompt info
        public Orc()
        {
            name = string.Empty;
            description = string.Empty;
            weapon = string.Empty;
            size = string.Empty;
        }
    }

    // needed for blank prompt info
    public BattleInfo()
    {
        orcs = new Orc[1];
    }
}