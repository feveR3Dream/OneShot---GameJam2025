using System.Collections;
using UnityEngine;

public class BossBeam : MonoBehaviour
{
    [SerializeField] GameObject beamPrefab;
    [SerializeField] Transform target;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float fireTime;
    bool isFiring;

    private void OnEnable()
    {
        EventDispatcher.Instance.Subscribe<BossWhiffed>(FireAction);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.Unsubscribe<BossWhiffed>(FireAction);
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (isFiring)
        {
            isFiring = false;
            StartCoroutine(FireBeam());
        }
    }

    private void FireAction(BossWhiffed context)
    {
        isFiring = true;
    }

    IEnumerator FireBeam()
    {
        Instantiate(beamPrefab, spawnPoint.position, spawnPoint.rotation);
        SoundManager.PlaySound(SoundType.LASER, 0.25f);
        yield return new WaitForSeconds(fireTime);
    }
}