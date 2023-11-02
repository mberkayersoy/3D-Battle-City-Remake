using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshManager : MonoBehaviour
{
    private static NavMeshSurface Surface;

    //private static NavMeshManager _Instance;
    //public static NavMeshManager Instance
    //{
    //    get
    //    {
    //        return _Instance;
    //    }

    //    private set
    //    {
    //        _Instance = value;
    //    }
    //}

    private void Awake()
    {
        //if (Instance != null)
        //{
        //    Debug.LogError($"Multiple NavMeshManagers in the scene! Destroying {name}!");
        //    Destroy(gameObject);
        //    return;
        //}

        Surface = GetComponent<NavMeshSurface>();
        Surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
        //Instance = this;

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
