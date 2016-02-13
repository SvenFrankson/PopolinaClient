using UnityEngine;

public class Brick
{
    public string reference;
    public Mesh mesh;

    public Brick()
    {

    }

    public void SetData(BrickData data)
    {
        reference = data.reference;
        mesh = new Mesh();
        Vector3[] vertices = new Vector3[data.vertices.Length / 3];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3((float)data.vertices[3 * i], (float)data.vertices[3 * i + 1], (float)data.vertices[3 * i + 2]);
        }

        int[] triangles = new int[data.triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            triangles[i] = Mathf.RoundToInt((float)data.triangles[i]);
        }

        Vector2[] uvs = new Vector2[data.uvs.Length / 2];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2((float)data.uvs[2 * i], (float)data.uvs[2 * i + 1]);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }
}
