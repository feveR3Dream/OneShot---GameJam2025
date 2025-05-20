using UnityEngine;

public class PierceManager : MonoBehaviour
{
<<<<<<< Updated upstream
}
=======
    public static PierceManager Instance { get; private set; }
    private int PierceStack;

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
>>>>>>> Stashed changes
