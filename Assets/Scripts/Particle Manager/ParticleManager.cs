using System;
using UnityEngine;

public enum ParticleType
{
    EXPLOSIONHIT,
    SPARK,
    HIT,
    EXPLOSIONOST,
    WITHSTAND,
    SHOCKWAVE,
    // Add more particles here
}

[ExecuteInEditMode]
public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleList[] particleList;
    public static ParticleManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnParticle(ParticleType type, Vector2 position, Quaternion rotation)
    {
        ParticleSystem[] particles = instance.particleList[(int)type].Particles;

        if (particles == null || particles.Length == 0)
        {
            Debug.LogWarning($"No particle assigned for: {type}");
            return;
        }

        ParticleSystem chosen = particles[UnityEngine.Random.Range(0, particles.Length)];
        ParticleSystem spawned = Instantiate(chosen, position, rotation);
        spawned.Play();
        Destroy(spawned.gameObject, spawned.main.duration + 1f); // Cleanup after play
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(ParticleType));

        Array.Resize<ParticleList>(ref particleList, names.Length); // match enum length

        for (int i = 0; i < particleList.Length; i++)
        {
            particleList[i].name = names[i];
        }
    }
#endif
}

[Serializable]
public struct ParticleList
{
    public ParticleSystem[] Particles { get => particles; }
    [HideInInspector] public string name;
    [SerializeField] private ParticleSystem[] particles;
}
