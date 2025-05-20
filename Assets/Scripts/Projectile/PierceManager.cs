using UnityEngine;

public class PierceManager : MonoBehaviour
{ 
    public static PierceManager Instance { get; private set; }
    [SerializeField] private int PierceStack;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public int GetPierceStack()
    {
        return PierceStack;
    }

    public void SetPierceStack(int newValue)
    {
        PierceStack = newValue;
    }
}
