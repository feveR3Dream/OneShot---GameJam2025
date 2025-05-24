using System.Collections;
using System.Threading;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Values")]
    [SerializeField] private bool canFire = true;


    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<GamePaused>(PausedShoot);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Subscribe<GamePaused>(PausedShoot);
    }

    private void Update()
    {
        Fire();
    }

    void Fire()
    {
        if(Input.GetMouseButtonDown(0) && canFire)
        {
            canFire = false;
            SoundManager.PlaySound(SoundType.SHOOT_SOUND, 0.5f);
            Projectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            var temp = new ShootIndicator
            {
                color = Color.white,
                timer = 0.05f
            };
            EventDispatcher.Instance.SendEvent(temp);

            projectile.owner = this;
        }
    }

    public void SetFire(bool newValue)
    {
        canFire = newValue;
    }

    private void PausedShoot(GamePaused e)
    {
        canFire = !e.paused;
    }
}
