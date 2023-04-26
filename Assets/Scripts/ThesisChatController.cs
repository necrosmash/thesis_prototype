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

    private Queue<OutputString> outputStrings = new Queue<OutputString>();
    private OutputString currentOutput;
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
        ChatInputField.onSubmit.RemoveListener(OnSubmit);
    }

    private void OnSubmit(string text)
    {
        AddToChatOutput(text);
        gm.ProcessEnemyKill(text);
    }

    public void AddToChatOutput(string newText, bool isItalic = false)
    {
        // Clear Input Field
        ChatInputField.text = string.Empty;

        outputStrings.Enqueue(new OutputString(newText.Replace("(Clone)", ""), isItalic));
    }

    private void Update()
    {
        ChatInputField.gameObject.SetActive(GameManager.isAwaitingKill);
        ChatInputField.readOnly = MenuCanvas.IsRendered || OpenAiApi.isPostInProgress;

        charCountdown -= Time.deltaTime;

        if (string.IsNullOrEmpty(currentOutput?.ToString()) && outputStrings.TryPeek(out _))
        {
            currentOutput = outputStrings.Dequeue();
            charIndex = 0;
        }

        if (currentOutput == null) return;

        if (charCountdown <= 0)
        {
            if (charIndex == currentOutput.ToString().Length)
            {
                if (currentOutput.isItalic) ChatDisplayOutput.text += "</i>";
                currentOutput = null;
                return;
            }
            else if (charIndex == 0) PrependOutput(currentOutput.isItalic);

            charCountdown = DELAY_PER_CHAR;
            ChatDisplayOutput.text += currentOutput.ToString()[charIndex++];
            ChatScrollbar.value = 0;
        }

        // Keep Chat input field active
        //ChatInputField.ActivateInputField();
    }

    private void PrependOutput(bool isItalic)
    {
        if (ChatDisplayOutput.text != string.Empty)
            ChatDisplayOutput.text += "\n";

        var timeNow = System.DateTime.Now;

        string formattedInput = "[<#FFFF80>" + timeNow.Hour.ToString("d2") + ":" + timeNow.Minute.ToString("d2") + ":" + timeNow.Second.ToString("d2") + "</color>] " + (isItalic ? "<i>" : "");

        ChatDisplayOutput.text += formattedInput;
    }

    private class OutputString
    {
        private string output;
        public bool isItalic { get; private set; }

        public OutputString(string output, bool isItalic = false)
        {
            this.output = output;
            this.isItalic = isItalic;
        }

        public override string ToString()
        {
            return output;
        }
    }

}
