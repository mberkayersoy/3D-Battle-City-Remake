using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshManager : MonoBehaviour
{
    public static NavMeshSurface Surface;

    private void Awake()
    {
        Surface = GetComponent<NavMeshSurface>();
        Surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
        Surface.collectObjects = CollectObjects.All;
    }
    private void Start()
    {
        BakeNavMesh();
    }

    public static void BakeNavMesh()
    {
        Surface.BuildNavMesh();
    }
}
