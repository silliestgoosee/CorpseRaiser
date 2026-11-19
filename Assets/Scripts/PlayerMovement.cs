using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // controls the character's navigation and pathfinding
    private NavMeshAgent agent;
    
    private Camera mainCamera;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // creates a ray from the camera through the mouse position
            Ray ray = mainCamera.ScreenPointToRay(
                Mouse.current.position.ReadValue()
            );

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
                }
            }
        }
    }
}