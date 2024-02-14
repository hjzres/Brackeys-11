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

[CustomEditor(typeof(RandomizeTerrainHeights))]
public class TerrainHeightEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RandomizeTerrainHeights generator = (RandomizeTerrainHeights)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Generate Heights"))
        {
            generator.GenerateHeights();
        }
    }
}
