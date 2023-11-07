using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamagable, IShooting
{
    public EnemyController (EnemyType enemyType)
    {
        this.enemyType = enemyType;
    }

    [Header("Characteristics")]
    [SerializeField] private float enemyDetectionRange;
    [SerializeField] private float checkDistance = 1f;
    [SerializeField] private int armorCapacity;
    [SerializeField] private float shotTimeOut;
    [SerializeField] private LayerMask destructibleLayers;
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private int givingScore;

    [Header("Transforms")]
    [SerializeField] private Transform currentTarget;
    [SerializeField] private Transform mainTarget;
    [SerializeField] private Transform firePointTransform;

    [Header("Effects")]
    [SerializeField] private ParticleSystem muzzleFlashVFX;
    [SerializeField] private ParticleSystem explosionVFX;

    [Header("GameObjects")]
    [SerializeField] private GameObject shild;
    [SerializeField] private GameObject popUpCanvas;
    [SerializeField] private GameObject projectile;

    [Header("Renderers")]
    [SerializeField] private Material[] materialList;
    [SerializeField] private MeshRenderer[] modelParts;

    private NavMeshAgent Agent;
    private float remainingShotTime;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
    }
    private void Start()
    {
        ActivateShild();
        mainTarget = GameManager.Instance.CurrentLevelManager.EnemyMainTarget;
        EventBus.OnLevelEndAction += EventBus_OnLevelEndAction;
    }

    public void SetEnemyCharacteristics(EnemyType enemyType)
    {
        switch (enemyType)
        {
            default:
            case EnemyType.Gray:
                enemyDetectionRange = 3f;
                shotTimeOut = 1f;
                givingScore = 250;
                break;
            case EnemyType.Green:
                enemyDetectionRange = 3.5f;
                shotTimeOut = 1f;
                givingScore = 500;
                break;
            case EnemyType.Blue:
                enemyDetectionRange = 3.5f;
                shotTimeOut = 0.9f;
                givingScore = 750;
                break;
            case EnemyType.Red:
                enemyDetectionRange = 4f;
                shotTimeOut = 0.8f;
                givingScore = 1000;
                break;
        }
        ApplyProperMaterial(enemyType);
        armorCapacity = (int)enemyType;
    }

    private void ApplyProperMaterial(EnemyType enemyType)
    {
        foreach (MeshRenderer item in modelParts)
        {
            item.material = materialList[(int)enemyType];
        }
    }

    private void EventBus_OnLevelEndAction(object sender, EventBus.OnLevelEndEventArgs e)
    {
        Agent.enabled = false;
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
        SearchEnemy();
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

    private void SearchEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyDetectionRange);

        foreach (Collider target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                currentTarget = target.transform;
                CheckEnemy();
                return;
            }
            else
            {
                currentTarget = mainTarget;
            }
        }
    }

    private void CheckEnemy()
    {
        if (Physics.Raycast(firePointTransform.position, transform.forward * enemyDetectionRange, out RaycastHit hit))
        {
            if (hit.transform.TryGetComponent(out ITarget target))
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

    public void Shot()
    {
        muzzleFlashVFX.Play();
        EventBus.PublishShotAction(this, transform.position);
        Instantiate(projectile, firePointTransform.position, transform.rotation);
    }

    public void TakeDamage(int damage)
    {
        if (shild.activeSelf) return;

        armorCapacity--;
        if (armorCapacity < 0)
        {
            explosionVFX.Play();
            PointPopUp popup = Instantiate(popUpCanvas, transform.position + Vector3.up, Quaternion.Euler(90, 0, 0), null).GetComponent<PointPopUp>();
            popup.SetContent(givingScore);
            EventBus.PublishUpdateScore(this, givingScore);
            EventBus.PublishExplosionAction(this, transform.position);
            EventBus.PublishEnemyDeath(this);
            explosionVFX.transform.SetParent(null, true);
            Destroy(gameObject);
        }
        else
        {
            // To do: hit effect and sound
        }

    }

    private void OnDestroy()
    {
        EventBus.OnLevelEndAction -= EventBus_OnLevelEndAction;
    }
}

public enum EnemyType
{
    Gray = 0,
    Green = 1,
    Blue = 2,
    Red = 3,
}
