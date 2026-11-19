using UnityEngine;
using UnityEngine.InputSystem;

public class ResurrectionRitual : MonoBehaviour
{
    private bool ritualActive = false;
    private bool waitingForInput = false;

    // parent object containing all QTE points for this sigil
    [SerializeField] private Transform ritualPoints;

    // stores how many QTE points the sigil has
    private int totalStages;
    
    [SerializeField] private GameObject ritualCanvas;
    [SerializeField] private RectTransform ritualMarker;
    [SerializeField] private float markerSpeed = 300f;
    
    // the point the marker is currently moving towards
    private int targetPointIndex = 1;

    void Update()
    {
        // don't check for ritual input unless a ritual is active
        if (!ritualActive)
            return;
        
        // If we have reached a point, wait for the player's QTE input.
        if (waitingForInput)
        {
            // TEMP: Space is being used as our QTE button.
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                CompletePoint();
            }
            return;
        }
        
        // get the point that the marker should currently travel towards
        RectTransform targetPoint = ritualPoints.GetChild(targetPointIndex).GetComponent<RectTransform>();
        
        // move the marker towards the target point
        ritualMarker.position = Vector3.MoveTowards(ritualMarker.position, targetPoint.position, markerSpeed * Time.deltaTime
        );
        
        // has the marker has arrived at the point
        if (Vector3.Distance(ritualMarker.position, targetPoint.position) < 1f)
        {
            // snap onto the point
            ritualMarker.position = targetPoint.position;

            // stop movement until the player completes the QTE
            waitingForInput = true;

            Debug.Log(
                "Reached ritual point " +
                (targetPointIndex + 1) +
                ". Press SPACE!"
            );
        }
    }
    
    public void StartRitual()
    {
        // prevents the same ritual from starting multiple times
        if (ritualActive)
            return;
        
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
        
        Debug.Log("Resurrection ritual started!");
    }
    
    private void CompletePoint()
    {
        Debug.Log(
            "Ritual point " +
            (targetPointIndex + 1) +
            " completed!"
        );

        waitingForInput = false;
        // move to the next ritual point.
        targetPointIndex++;
        
        // if there are no more points, the ritual is complete.
        if (targetPointIndex >= totalStages)
        {
            CompleteRitual();
        }
    }

    
    private void CompleteRitual()
    {
        ritualActive = false;
        waitingForInput = false;
        
        ritualCanvas.SetActive(false);
        Debug.Log("RESURRECTION SUCCESSFUL!");
    }
}