using UnityEngine;
using UnityEditor;

public class ChangeStickMesh : MonoBehaviour
{
    [MenuItem("Tools/Change Stick Mesh")]
    public static void ChangeMesh()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "(Carriable) Stick")
            {
                MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    // Assuming the mesh is in the Resources folder
                    Mesh imtesoMesh = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Resources/imteso.mesh");

                    if (imtesoMesh != null)
                    {
                        meshFilter.mesh = imtesoMesh;
                        Debug.Log("Mesh changed for: " + obj.name);
                    }
                    else
                    {
                        Debug.LogError("Mesh 'imteso' not found in Resources folder.");
                    }
                }
                else
                {
                    Debug.LogWarning("No MeshFilter found on: " + obj.name);
                }
            }
        }
    }
}