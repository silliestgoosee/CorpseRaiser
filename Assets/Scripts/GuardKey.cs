using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GuardKey : MonoBehaviour
{
    [SerializeField] private bool hasKey = true;
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
        // nothing to interact with if the key's already been taken
        if (!hasKey)
            return;

        PlayerMovement controlledCharacter = controlManager.GetCurrentCharacter();
        
        if (controlledCharacter == null)
            return;
        
        float distance = Vector3.Distance(transform.position, controlledCharacter.transform.position);

        
        // 1. are we close enough, 2. we are not controlling the mancer & 3. E was pressed
        if (distance <= interactionRange && !controlManager.IsControllingNecromancer()
                                         && Keyboard.current.eKey.wasPressedThisFrame)
        {
            TakeKey();
        }
    }

    
    private void TakeKey()
    {
        hasKey = false;
        
        playerInventory.CollectCellKey();
        Debug.Log("Key taken from guard!");
    }
}