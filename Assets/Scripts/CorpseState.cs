using UnityEngine;

public class CorpseState : MonoBehaviour
{
    private bool isResurrected = false;
    private PlayerMovement playerMovement;
    private CharacterControlManager controlManager;

    
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.enabled = false;

        controlManager = FindFirstObjectByType<CharacterControlManager>();
    }


    public void Resurrect()
    {
        // prevent corpse from being resurrected twice
        if (isResurrected)
            return;

        isResurrected = true;
        Debug.Log(gameObject.name + " has been resurrected!");

        // transfer player control to resurrected corpse
        controlManager.TakeControl(playerMovement);
        Debug.Log("Control transferred to " + gameObject.name);
    }

    public bool IsResurrected()
    {
        return isResurrected;
    }

    // gives control manager access to this corpse's movement component
    public PlayerMovement GetMovement()
    {
        return playerMovement;
    }
}