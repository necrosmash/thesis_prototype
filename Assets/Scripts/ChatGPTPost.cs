using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class ChatGPTPost
{
    public string model;
    public Message[] messages;
}
