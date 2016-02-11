using UnityEngine;
using UnityEditor;

public class MeshSerializer : EditorWindow
{
    public string reference;
    public Mesh mesh;
    public string output;

    [MenuItem("Popolina/MeshSerializer")]
    static void Init()
    {
        MeshSerializer window = EditorWindow.GetWindow(typeof(MeshSerializer)) as MeshSerializer;
        window.Show();
    }

    void OnGUI()
    {
        this.reference = EditorGUILayout.TextField("Reference", reference);
        this.mesh = EditorGUILayout.ObjectField("Mesh", mesh, typeof(Mesh)) as Mesh;
        if (GUILayout.Button("Get JSON Format"))
        {
            output = CreateJSON(this.mesh);
        }
        EditorGUILayout.TextArea(output);
    }

    public string CreateJSON(Mesh mesh)
    {
        string jsonString = "{ \"reference\" : \"" + reference + "\", ";
        string trianglesString = "\"triangles\" : [";
        for (int i = 0; i < mesh.triangles.Length; i++)
        {
            trianglesString += mesh.triangles[i] + ", ";
        }
        trianglesString.Remove(trianglesString.Length - 2, 2);
        trianglesString += "], ";
        string verticesString = "\"vertices\" : [";
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            verticesString += mesh.vertices[i].x.ToString("0.00") + ", " + mesh.vertices[i].y.ToString("0.00") + ", " + mesh.vertices[i].z.ToString("0.00") + ", ";
        }
        verticesString.Remove(verticesString.Length - 2, 2);
        verticesString += "]";
        jsonString += trianglesString;
        jsonString += verticesString;
        jsonString += "}";

            return jsonString;
    }
}