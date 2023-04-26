using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ThesisChatController : MonoBehaviour {


    public TMP_InputField ChatInputField;

    public TMP_Text ChatDisplayOutput;

    public Scrollbar ChatScrollbar;

    private GameManager gm;

    [SerializeField]
    private float DELAY_PER_CHAR;
    private float charCountdown;

    private Queue<string> outputStrings = new Queue<string>();
    private string currentOutput;
    private int charIndex;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnEnable()
    {
        ChatInputField.onSubmit.AddListener(OnSubmit);
    }

    void OnDisable()
    {
        ChatInputField.onSubmit.RemoveListener(AddToChatOutput);
    }

    private void OnSubmit(string text)
    {
        AddToChatOutput(text);
        gm.ProcessEnemyKill(text);
    }

    public void AddToChatOutput(string newText)
    {
        // Clear Input Field
        ChatInputField.text = string.Empty;

        outputStrings.Enqueue(newText.Replace("(Clone)", ""));
    }

    private void Update()
    {
        ChatInputField.gameObject.SetActive(GameManager.isAwaitingKill);
        ChatInputField.readOnly = MenuCanvas.IsRendered || OpenAiApi.isPostInProgress;

        charCountdown -= Time.deltaTime;

        if (string.IsNullOrEmpty(currentOutput) && outputStrings.TryPeek(out _))
        {
            currentOutput = outputStrings.Dequeue();
            charIndex = 0;
        }

        if (currentOutput == null) return;

        if (charCountdown <= 0)
        {
            if (charIndex == currentOutput.Length)
            {
                currentOutput = null;
                return;
            }
            else if (charIndex == 0) PrependOutput();

            charCountdown = DELAY_PER_CHAR;
            ChatDisplayOutput.text += currentOutput[charIndex++];
            ChatScrollbar.value = 0;
        }

        // Keep Chat input field active
        //ChatInputField.ActivateInputField();
    }

    private void PrependOutput()
    {
        if (ChatDisplayOutput.text != string.Empty)
            ChatDisplayOutput.text += "\n";

        var timeNow = System.DateTime.Now;

        string formattedInput = "[<#FFFF80>" + timeNow.Hour.ToString("d2") + ":" + timeNow.Minute.ToString("d2") + ":" + timeNow.Second.ToString("d2") + "</color>] ";

        ChatDisplayOutput.text += formattedInput;
    }

}
