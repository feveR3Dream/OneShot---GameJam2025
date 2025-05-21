using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Values")]
    [SerializeField] private bool canFire = true;
    private void Update()
    {
        Fire();
    }

    void Fire()
    {
        if(Input.GetMouseButtonDown(0) && canFire)
        {
            canFire = false;
            SoundManager.PlaySound(SoundType.SHOOT_SOUND, 1f);
            Projectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.owner = this;
        }
    }

    public void SetFire(bool newValue)
    {
        canFire = newValue;
    }
}
/*
{
    [Header("References")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform boss;
    [SerializeField] private LayerMask bossLayer;
    [SerializeField] private LayerMask projectileLayer;
    [SerializeField] private Transform firePoint;

    [Header("Projectiles")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float ammo = 1;
    [SerializeField] private float retrieveSpeed;

    [Header("Radius")]
    [SerializeField] private float searchAmmoRad;

    private bool canShoot = true;
    private bool canPickUp = false;

    private int _pierceStack = 1;

    public float Ammo => ammo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<BulletSpawn>(RespawnAmmo);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<BulletSpawn>(RespawnAmmo);
    }


    // Update is called once per frame
    void Update()
    {
        PickUpAmmo();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0) && CanShoot())
        {
            // Get direction from firePoint to mouse
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (mousePos - (Vector2)firePoint.position).normalized;

            // Instantiate bullet
            GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            // Rotate bullet
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            bullet.GetComponent<Projectile>().Initiate(_pierceStack);

            bulletRb.AddForce(dir * projectileSpeed, ForceMode2D.Impulse);
            ammo--;

        }
    }

    private bool CanShoot()
    {
        return ammo >= 1;
    }

    private void RespawnAmmo(BulletSpawn e)
    {
        StartCoroutine(_respawnTime(e.timer));
    }

    private IEnumerator _respawnTime(float timer)
    {
        yield return new WaitForSeconds(timer);

        canPickUp = true;

        Vector2 bossPos = boss.transform.position;
        float minRadius = 6f;
        float maxRadius = 8f;

        // Get random direction
        Vector2 direction = Random.insideUnitCircle.normalized;

        // Get random distance between min and max radius
        float distance = Random.Range(minRadius, maxRadius);

        Vector2 spawnPos = bossPos + direction * distance;

        GameObject ammoPickup = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
    }

    private void PickUpAmmo()
    {
        if (!canPickUp) return;

        Collider2D ammoCollider = Physics2D.OverlapCircle(this.gameObject.transform.position, searchAmmoRad, projectileLayer);

        if (ammoCollider != null)
        {
            Vector2 temp = Vector2.Lerp(ammoCollider.transform.position, (Vector2) this.transform.position, retrieveSpeed * Time.deltaTime);

            ammoCollider.transform.position = temp;

            if (Vector2.Distance(ammoCollider.transform.position, (Vector2) transform.position) <= 0.5f)
            {
                ammo++;
                canPickUp = false;
                Destroy(ammoCollider.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.gameObject.transform.position, searchAmmoRad);
    }
}
*/
