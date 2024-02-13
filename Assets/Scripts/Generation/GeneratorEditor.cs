using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshGenerator))]
public class GeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MeshGenerator generator = (MeshGenerator)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Generate Mesh"))
        {
            generator.GenerateMesh();
        }
    }
}
