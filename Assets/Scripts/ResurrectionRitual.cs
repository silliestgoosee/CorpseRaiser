using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ResurrectionRitual : MonoBehaviour
{
    private DialogueManager dialogueManager;
    
    private bool ritualActive = false;
    private bool waitingForInput = false;
    private Key currentQTEKey;
    private CorpseState corpseState;

    // parent object containing all QTE points for this sigil
    [SerializeField] private Transform ritualPoints;
    [SerializeField] private TMP_Text QTEPrompt;

    // stores how many QTE points the sigil has
    private int totalStages;
    
    [SerializeField] private GameObject ritualCanvas;
    [SerializeField] private RectTransform ritualMarker;
    [SerializeField] private float markerSpeed = 300f;
    
    // the point the marker is currently moving towards
    private int targetPointIndex = 1;


    void Start()
    {
        corpseState = GetComponent<CorpseState>();
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }

    void Update()
    {
        // don't check for ritual input unless a ritual is active
        if (!ritualActive)
            return;
        
        // if we have reached a point, wait for the player's QTE input
        if (waitingForInput)
        {
            // checks whether the randomly chosen QTE button was pressed
            if (Keyboard.current[currentQTEKey].wasPressedThisFrame)
            {
                Debug.Log("Correct input!");
                CompletePoint();
            }
            return;
        }
        
        // get the point that the marker should currently travel towards
        RectTransform targetPoint = ritualPoints.GetChild(targetPointIndex).GetComponent<RectTransform>();
        
        // move the marker towards the target point
        ritualMarker.position = Vector3.MoveTowards(ritualMarker.position, targetPoint.position, markerSpeed * Time.deltaTime
        );
        
        if (Vector3.Distance(ritualMarker.position, targetPoint.position) < 1f)
        {
            // snap the marker exactly onto the QTE point.
            ritualMarker.position = targetPoint.position;

            // stop the marker until the correct input is pressed
            waitingForInput = true;
        }
    }
    
    public void StartRitual()
    {
        // prevents the same ritual from starting multiple times
        if (ritualActive)
            return;
        
        // hide the button prompt while the marker is moving
        QTEPrompt.gameObject.SetActive(false);
        
        // counts the QTE points inside this sigil
        totalStages = ritualPoints.childCount;
        
        // need at least 2 points for the marker to travel between
        if (totalStages < 2)
        {
            Debug.LogWarning("ritual needs at least two ritual points");
            return;
        }
        
        ritualActive = true;
        waitingForInput = false;
        ritualCanvas.SetActive(true);
        
        // start marker on point 1
        RectTransform firstPoint =
            ritualPoints.GetChild(0).GetComponent<RectTransform>();

        ritualMarker.position = firstPoint.position;
        
        // Point2 is the first destination
        targetPointIndex = 1;
        
        GenerateQTE();
        
        // Debug.Log("Resurrection ritual started!");
    }
    
    
    private void GenerateQTE()
    {
        //  possible buttons that can appear during the ritual
        Key[] possibleKeys =
        {
            Key.W,
            Key.A,
            Key.S,
            Key.D
        };

        // chooses a button randomly from the list
        currentQTEKey = possibleKeys[Random.Range(0, possibleKeys.Length)];

        // Debug.Log("QTE: Press " + currentQTEKey);
        // Display the right button on screen
        QTEPrompt.text = currentQTEKey.ToString();
        QTEPrompt.gameObject.SetActive(true);
    }
    
    
    private void CompletePoint()
    {
        waitingForInput = false;
        
        Debug.Log(
            "Ritual point " +
            (targetPointIndex + 1) +
            " completed!"
        );

        // if the marker has returned to Point1, the full sigil is complete
        if (targetPointIndex == 0)
        {
            CompleteRitual();
            return;
        }

        // move to the next ritual point
        targetPointIndex++;

        // after reaching the final point, loop back to Point1
        if (targetPointIndex >= totalStages)
        {
            targetPointIndex = 0;
        }
        
        GenerateQTE();
    }

    
    private void CompleteRitual()
    {
        ritualActive = false;
        waitingForInput = false;
        
        ritualCanvas.SetActive(false);
        
        // temp
        string[] dialogue =
        {
            "Necromancer: Rise...",
            "Undead: What have you done to me?",
            "Necromancer: I need your help getting out of here."
        };
        
        // CorpseState.Resurrect will only run once the conversation ends.
        dialogueManager.StartDialogue(dialogue, corpseState.Resurrect);
    }
}