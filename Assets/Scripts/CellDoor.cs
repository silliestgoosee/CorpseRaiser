using UnityEngine;
using UnityEngine.InputSystem;

public class CellDoor : MonoBehaviour
{
    [SerializeField] private float interactionRange = 2f;
    
    private CharacterControlManager controlManager;
    private PlayerInventory playerInventory;


    void Start()
    {
        controlManager = FindFirstObjectByType<CharacterControlManager>();
        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }


    void Update()
    {
        PlayerMovement controlledCharacter =
            controlManager.GetCurrentCharacter();

        if (controlledCharacter == null)
            return;
        
        float distance = Vector3.Distance(transform.position, controlledCharacter.transform.position);
        
        // only the corpse can unlock the cell
        if (distance <= interactionRange &&
            !controlManager.IsControllingNecromancer() &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryUnlockDoor();
        }
    }


    private void TryUnlockDoor()
    {
        // check whether the key has been collected from the guard
        if (playerInventory.HasCellKey())
        {
            Debug.Log("Cell unlocked!");
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("The cell is locked. I need the key.");
        }
    }
}