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

    public bool isDialogueActive = false;


    void Start()
    {
        dialoguePanel.SetActive(false);
    }


    void Update()
    {
        // only listen for dialogue input while dialogue is active
        if (!isDialogueActive)
            return;
        
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            NextLine();
        }
    }


    public void StartDialogue(string[] lines, Action finishedAction)
    {
        currentLines = lines;
        currentLine = 0;
        
        onDialogueFinished = finishedAction;

        isDialogueActive = true;
        
        dialoguePanel.SetActive(true);
        dialogueText.text = currentLines[currentLine];
    }


    private void NextLine()
    {
        // go to the next line
        currentLine++;

        // check if there are any dialogue lines remaining
        if (currentLine < currentLines.Length)
        {
            dialogueText.text = currentLines[currentLine];
        }
        else
        {
            // no lines remaining so finish the dialogue
            EndDialogue();
        }
    }


    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        
        onDialogueFinished?.Invoke();
    }
}