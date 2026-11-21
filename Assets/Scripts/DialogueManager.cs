using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    
    private string[] currentLines;
    private int currentLine = 0;
    private Action onDialogueFinished;
    private bool dialogueActive = false;


    void Start()
    {
        dialoguePanel.SetActive(false);
    }


    void Update()
    {
        if (!dialogueActive)
            return;
        
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            NextLine();
        }
    }


    public void StartDialogue(string[] lines, Action finishedAction)
    {
        Debug.Log("Starting Dialogue");
        
        currentLines = lines;
        currentLine = 0;
        
        onDialogueFinished = finishedAction;

        dialogueActive = true;
        dialoguePanel.SetActive(true);
        
        dialogueText.text = currentLines[currentLine];
    }


    private void NextLine()
    {
        currentLine++;

        // check if there are any dialogue lines remaining
        if (currentLine < currentLines.Length)
        {
            dialogueText.text = currentLines[currentLine];
        }
        else
        {
            EndDialogue();
        }
    }


    private void EndDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);
        
        onDialogueFinished?.Invoke();
    }
}