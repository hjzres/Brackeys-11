using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [Min(0)] public int width, height;
    public Vector3 startPosition;
    public Material meshMaterial;
    public bool showGizmos;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    public void GenerateMesh()
    {
        DestroyImmediate(GameObject.Find("Mesh Object"));
        mesh = new Mesh();
        mesh.Clear();

        GameObject meshObject = new GameObject("Mesh Object", typeof(MeshFilter), typeof(MeshRenderer));
        meshObject.GetComponent<MeshFilter>().mesh = mesh;
        meshObject.GetComponent<MeshRenderer>().material = meshMaterial;

        vertices = new Vector3[(width + 1) * (height + 1)];
        triangles = new int[width * height * 6];

        int k = 0;
        for (int x = (int)startPosition.x; x <= (int)startPosition.x + width; x++)
        {
            for (int y = (int)startPosition.z; y <= (int)startPosition.z + height; y++)
            {
                vertices[k] = new Vector3(x, 0, y);
                k++;                
            }
        }

        int tris = 0;
        int vert = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + width + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + width + 1;
                triangles[tris + 5] = vert + width + 2;
                vert++;
                tris += 6;
            }

            vert++;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
