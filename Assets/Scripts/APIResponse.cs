using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class APIResponse
{
    public string id;
    public string objectProperty; // doesn't work right now, but might not be needed
    public string created;
    public string model;
    public Usage usage;
    public BattleInfo battleInfo;

    public abstract void ParseBattleInfo();
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