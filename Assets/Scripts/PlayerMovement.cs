using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Camera mainCamera;
    private DialogueManager dialogueManager;
    
    [SerializeField] private GameObject moveMarker;
    [SerializeField] private LayerMask movementBlockingLayers;
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
        moveMarker.SetActive(false);
        dialogueManager = FindFirstObjectByType<DialogueManager>();
    }

    void Update()
    {
        // lock movement while dialogue is active
        if(dialogueManager != null && dialogueManager.isDialogueActive)
        {
            return;
        }
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // creates a ray from the camera through the mouse position
            Ray ray = mainCamera.ScreenPointToRay( Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // checks whether the object clicked on is the corpse layer
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Corpse"))
                {
                    Debug.Log("Corpse clicked!");
                }
                else
                {
                    // if something else was clicked, move towards that position
                    agent.SetDestination(hit.point);
                    
                    // move the destination marker to where the mouse clicked
                    moveMarker.SetActive(true);
                    moveMarker.transform.position = hit.point;
                }
            }
        }
        
        // has the char finished calculating the path
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                // has the character stopped moving
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    // hide the marker after movement stops
                    moveMarker.SetActive(false);
                }
            }
        }
    }
}