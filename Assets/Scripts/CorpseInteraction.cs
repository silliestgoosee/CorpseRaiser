using UnityEngine;
using UnityEngine.InputSystem;

public class CorpseInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 2f;
    
    private CharacterControlManager controlManager;
    private GameObject player;
    private bool playerInRange = false;
    private ResurrectionRitual resurrectionRitual;
    

    void Start()
    {
        controlManager = FindFirstObjectByType<CharacterControlManager>();
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

        // if the NECROMANCER is in range and E is pressed, begin the interaction
        if (playerInRange &&  controlManager.IsControllingNecromancer() && Keyboard.current.eKey.wasPressedThisFrame)
        {
            resurrectionRitual.StartRitual();
        }
    }
}