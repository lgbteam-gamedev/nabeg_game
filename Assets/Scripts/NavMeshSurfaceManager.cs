using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshSurfaceManager : MonoBehaviour
{
    public static NavMeshSurfaceManager Instance { get; private set; }

    public NavMeshSurface surface;

    void Awake()
    {
        Instance = this;
    }

    public void RebuildNavMesh()
    {
        surface.BuildNavMesh();
    }
}