using System.Collections;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private LayerMask weakspot;
    [SerializeField] private LayerMask obstacle;
    [SerializeField] private LayerMask hittable;
    [SerializeField] private LayerMask boss;


    [Header("Values")]
    [SerializeField] private float slowPercent; // FOR TUNG
    [SerializeField] public float currentSpeed;

    bool canDamage = true;
    public GunController owner;
    private void Start()
    {
        Invoke("WhiffedShot", 2f);
    }

    private void FixedUpdate()
    {

        _rb.velocity = currentSpeed * transform.right;
        currentSpeed = _rb.velocity.magnitude;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((weakspot & (1 << collision.gameObject.layer)) != 0 && canDamage)
        {
            owner.SetFire(true);
            ParticleManager.instance.SpawnParticle(ParticleType.SPARK, (Vector2)transform.position, Quaternion.identity);
            SoundManager.PlaySound(SoundType.HIT_WEAKSPOT, 0.25f);
            EventDispatcher.Instance.SendEvent(new BossHurt {projectilePos = (Vector2)transform.position });
            PierceManager.Instance.SetPierceStack(PierceManager.Instance.GetPierceStack() + 1);
            EventDispatcher.Instance.SendEvent(new PierceModified());
            canDamage = false;
            Destroy(gameObject);
            return;
        }
        if((obstacle & (1 << collision.gameObject.layer)) != 0 && canDamage)
        {
            SoundManager.PlaySound(SoundType.RICOCHET, 0.25f);
            if (PierceManager.Instance.GetPierceStack() > 0)
            {
                ParticleManager.instance.SpawnParticle(ParticleType.EXPLOSIONOST, (Vector2)transform.position, Quaternion.identity);
                
                Destroy(collision.gameObject);
                PierceManager.Instance.SetPierceStack(PierceManager.Instance.GetPierceStack() - 1);
                EventDispatcher.Instance.SendEvent(new PierceModified());
            }
            else
            {
                ParticleManager.instance.SpawnParticle(ParticleType.WITHSTAND, (Vector2)transform.position, Quaternion.identity);
                EventDispatcher.Instance.SendEvent(new BossWhiffed());
                canDamage = false;
                Destroy(gameObject);
            }
        }
        if ((hittable & (1 << collision.gameObject.layer)) != 0 && canDamage)
        {
            SoundManager.PlaySound(SoundType.RICOCHET, 0.25f);
            ParticleManager.instance.SpawnParticle(ParticleType.EXPLOSIONHIT, (Vector2)transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            owner.SetFire(true);
            canDamage = false;
            Destroy(gameObject);
        }
        if ((boss & (1 << collision.gameObject.layer)) != 0 && canDamage)
        {
            SoundManager.PlaySound(SoundType.RICOCHET, 0.25f);
            ParticleManager.instance.SpawnParticle(ParticleType.HIT, (Vector2)transform.position, Quaternion.identity);
            EventDispatcher.Instance.SendEvent(new BossWhiffed());
            canDamage = false;
            Destroy(gameObject);
        }
    }

    private void WhiffedShot()
    {
        Destroy(gameObject);
        EventDispatcher.Instance.SendEvent(new BossWhiffed());
    }
}