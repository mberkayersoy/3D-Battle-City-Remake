using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamagable
{
    private NavMeshAgent Agent;
    [SerializeField] private Transform currentTarget;
    [SerializeField] private Transform mainTarget;

    [SerializeField] private Transform firePointTransform;
    [SerializeField] private GameObject projectile;

    [SerializeField] private float enemyDetectionRange;
    [SerializeField] private float checkDistance = 1f;
    [SerializeField] private LayerMask destructibleLayers;

    [SerializeField] private float shotTimeOut;
    [SerializeField] private ParticleSystem muzzleFlashVFX;
    [SerializeField] private ParticleSystem explosionVFX;
    private float remainingShotTime;

    [SerializeField] private GameObject shild;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
    }
    private void Start()
    {
        ActivateShild();
    }
    private void ActivateShild()
    {
        shild.SetActive(true);
        StartCoroutine(DeactivateShild());
    }
    private IEnumerator DeactivateShild()
    {
        yield return new WaitForSeconds(2f);
        shild.SetActive(false);
    }

    private void FixedUpdate()
    {
        CheckEnemy();
    }
    private void Update()
    {
        if (remainingShotTime > 0)
        {
            remainingShotTime -= Time.deltaTime;
        }

        if (currentTarget != null && Agent.enabled)
        {
            Agent.SetDestination(currentTarget.position);

        }

        CheckForDestructibleObjects();
    }

    private void CheckForDestructibleObjects()
    {
        if (Physics.Raycast(firePointTransform.position, transform.forward, checkDistance, destructibleLayers))
        {
            Agent.enabled = false;
            if (remainingShotTime <= 0)
            {
                Shot();
                remainingShotTime = shotTimeOut;
            }
        }
        else 
        {
            Agent.enabled = true;
        }
    }

    private void CheckEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyDetectionRange);

        foreach (Collider target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                currentTarget = target.transform;
                AimEnemy();
                return;
            }
            else
            {
                currentTarget = mainTarget;
            }
        }
    }

    private void AimEnemy()
    {
        if (Physics.Raycast(firePointTransform.position, transform.forward * enemyDetectionRange, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Player"))
            {
                if (remainingShotTime <= 0)
                {
                    Shot();
                    remainingShotTime = shotTimeOut;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyDetectionRange);
    }

    private void Shot()
    {
        muzzleFlashVFX.Play();
        Instantiate(projectile, firePointTransform.position, transform.rotation);
    }

    public void TakeDamage(int damage)
    {
        if (shild.activeSelf) return;
        explosionVFX.Play();
        EventBus.PublishEnemyDeath(this);
        explosionVFX.transform.SetParent(null, true);
        Destroy(gameObject);
    }
}
