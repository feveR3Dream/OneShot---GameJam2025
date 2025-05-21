using UnityEngine;

public class KYSRN : MonoBehaviour
{
    ParticleSystem _particleSystem;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if(!_particleSystem.IsAlive())
        {
            Destroy(this.gameObject);
        }
    }
}