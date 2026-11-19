using UnityEngine;
using UnityEngine.InputSystem;

public class CorpseInteraction : MonoBehaviour
{
    // how close the Necromancer needs to be to interact
    [SerializeField] private float interactionRange = 2f;
    
    private GameObject player;
    private bool playerInRange = false;
    private ResurrectionRitual resurrectionRitual;
    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        // gets the ResurrectionRitual attached to corpse
        resurrectionRitual = GetComponent<ResurrectionRitual>();
    }

    void Update()
    {
        if (player == null)
            return;

        // measures the distance between the corpse and necromancer
        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );
        
        playerInRange = distance <= interactionRange;

        // if in range close and E is pressed, begin the interaction
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            resurrectionRitual.StartRitual();
        }
    }
}