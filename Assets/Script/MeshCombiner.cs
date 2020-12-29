using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class MeshCombiner : MonoBehaviour
{

    private void Start()
    {
        CombineMeshes();
    }

    public void CombineMeshes ()
    {
        

        Vector3 position = transform.position;
        transform.position = Vector3.zero;

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;

        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }

        transform.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
        transform.GetComponent<MeshFilter>().sharedMesh.Optimize();
        transform.GetComponent<MeshCollider>().sharedMesh = transform.GetComponent<MeshFilter>().sharedMesh;
        transform.gameObject.SetActive(true);

        transform.position = position;
    }
}
