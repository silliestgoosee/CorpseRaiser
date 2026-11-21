using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class CharacterControlManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement necromancerMovement;

    private PlayerMovement currentCharacter;
    
    void Start()
    {
        // begin the game controlling the Necromancer
        currentCharacter = necromancerMovement;
        necromancerMovement.enabled = true;
    }


    void Update()
    {
        // TEMPORARY:
        // tab returns control to the necromancer
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ReturnToNecromancer();
        }
    }

    
    public void TakeControl(PlayerMovement newCharacter)
    {
        // safety check in case there is no character around
        if (newCharacter == null)
            return;
                
        // stop the currently controlled character receiving input
        if (currentCharacter != null)
        {
            currentCharacter.enabled = false;
            
            // cancel any old movement inputs
            NavMeshAgent oldAgent = currentCharacter.GetComponent<NavMeshAgent>();

            if (oldAgent != null && oldAgent.isOnNavMesh) ;
            {
                oldAgent.ResetPath();
            }
        }

        // store and enable the new controlled character to receive input
        currentCharacter = newCharacter;
        currentCharacter.enabled = true;
        
        Debug.Log("Control switched to " + currentCharacter.gameObject.name);
    }
    
    // returns movement component of character currently controlled
    public PlayerMovement GetCurrentCharacter()
    {
        return currentCharacter;
    }
    
    public bool IsControllingNecromancer()
    {
        return currentCharacter == necromancerMovement;
    }

    
    public void ReturnToNecromancer()
    {
        // no need to switch if the necromancer is already being controlled
        if (currentCharacter == necromancerMovement)
            return;
        
        TakeControl(necromancerMovement);
    }
}