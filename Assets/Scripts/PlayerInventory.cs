using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private bool hasCellKey = false;
    
    
    public void CollectCellKey()
    {
        hasCellKey = true;
        Debug.Log("Cell key collected");
    }
    
    // allow other scripts to check whether player has the key
    public bool HasCellKey()
    {
        return hasCellKey;
    }
}