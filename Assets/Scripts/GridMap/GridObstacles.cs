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
    }

    private void OnDisable()
    {
        if (NavMeshManager.Surface != null)
        {
            NavMeshManager.BakeNavMesh();
        }
        
    }
    //private void OnDestroy()
    //{
    //    NavMeshManager.BakeNavMesh();
    //}

}
