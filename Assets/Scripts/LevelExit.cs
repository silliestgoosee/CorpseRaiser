using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private GameObject winCanvas;
    
    private CharacterControlManager controlManager;
    private bool levelComplete = false;


    void Start()
    {
        controlManager = FindFirstObjectByType<CharacterControlManager>();
        
        winCanvas.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (levelComplete)
            return;
        
        PlayerMovement currentCharacter = controlManager.GetCurrentCharacter();

        // ignore anything other than the controlled character
        if (currentCharacter == null || other.gameObject != currentCharacter.gameObject)
        {
            return;
        }

        // only the necromancer escaping completes the level
        if (!controlManager.IsControllingNecromancer())
            return;

        CompleteLevel();
    }


    private void CompleteLevel()
    {
        levelComplete = true;

        // show temp win screen
        winCanvas.SetActive(true);
    }
}