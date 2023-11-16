using Unity.AI.Navigation;
using UnityEngine;

public class GridObstacles : MonoBehaviour, IDamagable, IEffectCreator
{
    NavMeshModifier navMeshModifier;
    private void Awake()
    {
        navMeshModifier = GetComponent<NavMeshModifier>();
        navMeshModifier.ignoreFromBuild = true;
    }
    public void TakeDamage(int Damage)
    {
        EventBus.PublishBrickExplosionAction(this, transform.position);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        NavMeshManager.BakeNavMesh();
    }

}
