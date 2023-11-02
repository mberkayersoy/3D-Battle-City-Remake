using Unity.AI.Navigation;
using UnityEngine;

public class GridObstacles : MonoBehaviour, IDamagable
{
    NavMeshModifier navMeshModifier;

    [SerializeField] private ParticleSystem destroyVFX;

    private void Awake()
    {
        navMeshModifier = GetComponent<NavMeshModifier>();
        navMeshModifier.ignoreFromBuild = true;

    }
    public void TakeDamage(int Damage)
    {
        if (destroyVFX != null)
        {
            destroyVFX.gameObject.SetActive(true);
            destroyVFX.transform.SetParent(null, true);
            destroyVFX.Play();
        }
        gameObject.SetActive(false);
        NavMeshManager.BakeNavMesh();
        Destroy(gameObject);
    }

}
