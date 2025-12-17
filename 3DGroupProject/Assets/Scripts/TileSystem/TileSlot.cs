using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.AI.Navigation;

public class TileSlot : MonoBehaviour
{
    private MeshRenderer meshRenderer => GetComponent<MeshRenderer>();
    private MeshFilter meshFilter => GetComponent<MeshFilter>();

    private Collider myCollider => GetComponent<Collider>();
    private NavMeshSurface myNavMesh => GetComponentInParent<NavMeshSurface>();

    public void SwitchTile(GameObject referenceTile)
    {
        gameObject.name = referenceTile.name;

        TileSlot newTile = referenceTile.GetComponent<TileSlot>();

        // Assign info that we get from the reference tile
        meshFilter.mesh = newTile.GetMesh();
        meshRenderer.material = newTile.GetMaterial();

        // Update collider based on reference tile
        UpdateCollider(newTile.GetCollider());
        UpdateChildren(newTile);

        // Update layer and navmesh after changes
        UpdateLayer(referenceTile);
        UpdateNavMesh();
    }


    // Get info from a tile
    public Material GetMaterial() => meshRenderer.sharedMaterial;
    public Mesh GetMesh() => meshFilter.sharedMesh;

    // Get info for collider
    public Collider GetCollider() => myCollider;

    public List<GameObject> GetAllChildren()
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }
        return children;
    }

    private void UpdateNavMesh()
    {
        myNavMesh.BuildNavMesh();
    }

    // Modify collider based on reference tile
    public void UpdateCollider(Collider newCollider)
    {
        DestroyImmediate(myCollider);
        if (newCollider is BoxCollider)
        {
            BoxCollider original = newCollider.GetComponent<BoxCollider>();
            BoxCollider myNewCollider = transform.AddComponent<BoxCollider>();

            myNewCollider.center = original.center;
            myNewCollider.size = original.size;
        }

        if (newCollider is MeshCollider)
        {
            MeshCollider original = newCollider.GetComponent<MeshCollider>();
            MeshCollider myNewCollider = transform.AddComponent<MeshCollider>();

            myNewCollider.sharedMesh = original.sharedMesh;
            myNewCollider.convex = original.convex;
        }
    }

    private void UpdateChildren(TileSlot newTile)
    {
        foreach (GameObject obj in GetAllChildren())
        {
            DestroyImmediate(obj);
        }

        foreach (GameObject obj in newTile.GetAllChildren())
        {
            Instantiate(obj, transform);
        }
    }

    public void UpdateLayer(GameObject referenceObj) => gameObject.layer = referenceObj.layer;

    public void RotateTile(int dir)
    {
        transform.Rotate(0, 90 * dir, 0);
        UpdateNavMesh();
    }
    public void AdjustY(int verticalDir)
    {
        transform.position += new Vector3(0, 0.1f * verticalDir, 0);
        UpdateNavMesh();
    }
}
