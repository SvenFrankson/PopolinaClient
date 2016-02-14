using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunck : MonoBehaviour 
{
    public static float TILESIZE = 0.5f;
    public static float TILEHEIGHT = 0.2f;
    public static int CHUNCKSIZE = 32;
    public bool meshIsSet = false;
    public int iPos { get; set; }
    public int jPos { get; set; }
    public int[] map { get; set; }
    public Dictionary<string, List<Block>> texturedBlocks;

    public Chunck()
    {

    }

    public void SetData(ChunckData data)
    {
        iPos = data.iPos;
        jPos = data.jPos;
        map = new int[CHUNCKSIZE * CHUNCKSIZE];
        data.map.CopyTo(map, 0);
        texturedBlocks = new Dictionary<string, List<Block>>();
        for (int i = 0; i < data.blocks.Length; i++)
        {
            var b = new Block();
            b.SetData(data.blocks[i]);
            if (!texturedBlocks.ContainsKey(b.texture))
            {
                texturedBlocks.Add(b.texture, new List<Block>());
            }
            texturedBlocks[b.texture].Add(b);
        }
        this.transform.position = new Vector3(Chunck.TILESIZE * iPos * Chunck.CHUNCKSIZE, 0f, Chunck.TILESIZE * jPos * Chunck.CHUNCKSIZE);
    }

    public void SetMesh(/*Chunck[][] sideChuncks*/)
    {
        this.GetComponent<MeshFilter>().mesh = this.BuildMesh();
        this.GetComponent<MeshCollider>().sharedMesh = this.GetComponent<MeshFilter>().mesh;
        this.meshIsSet = true;
    }

    public void SetBlock()
    {
        StartCoroutine(BrickManager.Instance.RequestBrickOn(this));
    }

    public Mesh BuildMesh()
    {
        Mesh m = new Mesh();
        List<Vector3> verticesList = new List<Vector3>();
        List<int> trianglesList = new List<int>();

        for (int j = 0; j < CHUNCKSIZE; j++)
        {
            for (int i = 0; i < CHUNCKSIZE; i++)
            {
                Vector3 v1 = new Vector3(i * TILESIZE - TILESIZE / 2, map[i + CHUNCKSIZE * j] * TILEHEIGHT, j * TILESIZE - TILESIZE / 2);
                Vector3 v2 = new Vector3(i * TILESIZE - TILESIZE / 2, map[i + CHUNCKSIZE * j] * TILEHEIGHT, j * TILESIZE + TILESIZE / 2);
                Vector3 v3 = new Vector3(i * TILESIZE + TILESIZE / 2, map[i + CHUNCKSIZE * j] * TILEHEIGHT, j * TILESIZE + TILESIZE / 2);
                Vector3 v4 = new Vector3(i * TILESIZE + TILESIZE / 2, map[i + CHUNCKSIZE * j] * TILEHEIGHT, j * TILESIZE - TILESIZE / 2);
                Vector3 v5 = new Vector3(i * TILESIZE - TILESIZE / 2, -1 * TILEHEIGHT, j * TILESIZE - TILESIZE / 2);
                Vector3 v6 = new Vector3(i * TILESIZE - TILESIZE / 2, -1 * TILEHEIGHT, j * TILESIZE + TILESIZE / 2);
                Vector3 v7 = new Vector3(i * TILESIZE + TILESIZE / 2, -1 * TILEHEIGHT, j * TILESIZE + TILESIZE / 2);
                Vector3 v8 = new Vector3(i * TILESIZE + TILESIZE / 2, -1 * TILEHEIGHT, j * TILESIZE - TILESIZE / 2);

                verticesList.Add(v1);
                verticesList.Add(v2);
                verticesList.Add(v3);
                verticesList.Add(v4);
                trianglesList.Add(verticesList.Count - 4);
                trianglesList.Add(verticesList.Count - 3);
                trianglesList.Add(verticesList.Count - 2);
                trianglesList.Add(verticesList.Count - 4);
                trianglesList.Add(verticesList.Count - 2);
                trianglesList.Add(verticesList.Count - 1);

                verticesList.Add(v4);
                verticesList.Add(v3);
                verticesList.Add(v7);
                verticesList.Add(v8);
                trianglesList.Add(verticesList.Count - 4);
                trianglesList.Add(verticesList.Count - 3);
                trianglesList.Add(verticesList.Count - 2);
                trianglesList.Add(verticesList.Count - 4);
                trianglesList.Add(verticesList.Count - 2);
                trianglesList.Add(verticesList.Count - 1);

                verticesList.Add(v3);
                verticesList.Add(v2);
                verticesList.Add(v6);
                verticesList.Add(v7);
                trianglesList.Add(verticesList.Count - 4);
                trianglesList.Add(verticesList.Count - 3);
                trianglesList.Add(verticesList.Count - 2);
                trianglesList.Add(verticesList.Count - 4);
                trianglesList.Add(verticesList.Count - 2);
                trianglesList.Add(verticesList.Count - 1);

                verticesList.Add(v2);
                verticesList.Add(v1);
                verticesList.Add(v5);
                verticesList.Add(v6);
                trianglesList.Add(verticesList.Count - 4);
                trianglesList.Add(verticesList.Count - 3);
                trianglesList.Add(verticesList.Count - 2);
                trianglesList.Add(verticesList.Count - 4);
                trianglesList.Add(verticesList.Count - 2);
                trianglesList.Add(verticesList.Count - 1);

                verticesList.Add(v1);
                verticesList.Add(v4);
                verticesList.Add(v8);
                verticesList.Add(v5);
                trianglesList.Add(verticesList.Count - 4);
                trianglesList.Add(verticesList.Count - 3);
                trianglesList.Add(verticesList.Count - 2);
                trianglesList.Add(verticesList.Count - 4);
                trianglesList.Add(verticesList.Count - 2);
                trianglesList.Add(verticesList.Count - 1);
            }
        }

        m.vertices = verticesList.ToArray();
        m.triangles = trianglesList.ToArray();
        m.RecalculateNormals();

        return m;
    }

    /*
    public Mesh BuildMesh(Chunck[][] sideChuncks)
    {
        int[][] extendedMap = new int[CHUNCKSIZE + 3][];
        for (int i = 0; i < CHUNCKSIZE + 3; i++)
        {
            extendedMap[i] = new int[CHUNCKSIZE + 3];
        }

        // Center
        for (int j = 0; j < CHUNCKSIZE; j++)
        {
            for (int i = 0; i < CHUNCKSIZE; i++)
            {
                extendedMap[i + 1][j + 1] = this.map[i + j * CHUNCKSIZE];
            }
        }
        // Top Left
        extendedMap[0][0] = sideChuncks[0][0].map[CHUNCKSIZE * CHUNCKSIZE - 1];
        // Top
        for (int j = 0; j < CHUNCKSIZE; j++)
        {
            extendedMap[0][j + 1] = sideChuncks[0][1].map[(CHUNCKSIZE - 1) + j * CHUNCKSIZE];
        }
        // Top Right
        extendedMap[0][CHUNCKSIZE + 1] = sideChuncks[0][2].map[(CHUNCKSIZE - 1)];
        extendedMap[0][CHUNCKSIZE + 2] = sideChuncks[0][2].map[(CHUNCKSIZE - 1) + CHUNCKSIZE];
        // Right
        for (int i = 0; i < CHUNCKSIZE; i++)
        {
            extendedMap[i + 1][CHUNCKSIZE + 1] = sideChuncks[1][2].map[i];
            extendedMap[i + 1][CHUNCKSIZE + 2] = sideChuncks[1][2].map[i + CHUNCKSIZE];
        }
        // Bottom Right
        extendedMap[CHUNCKSIZE + 1][CHUNCKSIZE + 1] = sideChuncks[2][2].map[0];
        extendedMap[CHUNCKSIZE + 2][CHUNCKSIZE + 1] = sideChuncks[2][2].map[1];
        extendedMap[CHUNCKSIZE + 1][CHUNCKSIZE + 2] = sideChuncks[2][2].map[CHUNCKSIZE];
        extendedMap[CHUNCKSIZE + 2][CHUNCKSIZE + 2] = sideChuncks[2][2].map[1 + CHUNCKSIZE];
        // Bottom
        for (int j = 0; j < CHUNCKSIZE; j++)
        {
            extendedMap[CHUNCKSIZE + 1][j + 1] = sideChuncks[2][1].map[j * CHUNCKSIZE];
            extendedMap[CHUNCKSIZE + 2][j + 1] = sideChuncks[2][1].map[1 + j * CHUNCKSIZE];
        }
        // Bottom Left
        extendedMap[CHUNCKSIZE + 1][0] = sideChuncks[2][0].map[CHUNCKSIZE];
        extendedMap[CHUNCKSIZE + 2][0] = sideChuncks[2][0].map[1 + CHUNCKSIZE];
        // Left
        for (int i = 0; i < CHUNCKSIZE; i++)
        {
            extendedMap[i + 1][0] = sideChuncks[1][0].map[i + (CHUNCKSIZE - 1) * CHUNCKSIZE];
        }

        Mesh m = new Mesh();
        List<Vector3> verticesList = new List<Vector3>();
        List<int> trianglesList = new List<int>();
        List<Vector3> normalsList = new List<Vector3>();

        for (int j = 0; j < CHUNCKSIZE + 1; j++)
        {
            for (int i = 0; i < CHUNCKSIZE + 1; i++)
            {
                verticesList.Add(new Vector3(TILESIZE * i, TILEHEIGHT * extendedMap[i + 1][j + 1], TILESIZE * j));
            }
        }

        for (int i = 0; i < CHUNCKSIZE; i++)
        {
            for (int j = 0; j < CHUNCKSIZE; j++)
            {
                float sqrDiag1 = (verticesList[i + j * (CHUNCKSIZE + 1)] + verticesList[i + 1 + (j + 1) * (CHUNCKSIZE + 1)]).sqrMagnitude;
                float sqrDiag2 = (verticesList[i + 1 + j * (CHUNCKSIZE + 1)] + verticesList[i + (j + 1) * (CHUNCKSIZE + 1)]).sqrMagnitude;

                if (sqrDiag1 <= sqrDiag2)
                {
                    trianglesList.Add(i + j * (CHUNCKSIZE + 1));
                    trianglesList.Add(i + 1 + (j + 1) * (CHUNCKSIZE + 1));
                    trianglesList.Add(i + 1 + j * (CHUNCKSIZE + 1));

                    trianglesList.Add(i + j * (CHUNCKSIZE + 1));
                    trianglesList.Add(i + (j + 1) * (CHUNCKSIZE + 1));
                    trianglesList.Add(i + 1 + (j + 1) * (CHUNCKSIZE + 1));
                }
                else
                {
                    trianglesList.Add(i + j * (CHUNCKSIZE + 1));
                    trianglesList.Add(i + (j + 1) * (CHUNCKSIZE + 1));
                    trianglesList.Add(i + 1 + j * (CHUNCKSIZE + 1));

                    trianglesList.Add(i + 1 + (j + 1) * (CHUNCKSIZE + 1));
                    trianglesList.Add(i + 1 + j * (CHUNCKSIZE + 1));
                    trianglesList.Add(i + (j + 1) * (CHUNCKSIZE + 1));
                }
            }
        }

        for (int j = 0; j < CHUNCKSIZE + 1; j++)
        {
            for (int i = 0; i < CHUNCKSIZE + 1; i++)
            {
                Vector3 v1 = new Vector3(1, extendedMap[i + 2][j + 1] - extendedMap[i + 1][j + 1], 0);
                Vector3 v2 = new Vector3(0, extendedMap[i + 1][j + 2] - extendedMap[i + 1][j + 1], 1);
                Vector3 v3 = new Vector3(-1, extendedMap[i][j + 1] - extendedMap[i + 1][j + 1], 0);
                Vector3 v4 = new Vector3(0, extendedMap[i + 1][j] - extendedMap[i + 1][j + 1], -1);

                Vector3 n1 = Vector3.Cross(v2, v1).normalized;
                Vector3 n2 = Vector3.Cross(v4, v3).normalized;

                normalsList.Add((n1 + n2).normalized);
            }
        }

        m.vertices = verticesList.ToArray();
        m.triangles = trianglesList.ToArray();
        m.normals = normalsList.ToArray();

        return m;
    }
    */

    public void BuildBlocks()
    {
        foreach (KeyValuePair<string, List<Block>> bs in texturedBlocks) {
            string textureName = bs.Key;
            List<Block> blocks = bs.Value;

            CombineInstance[] blockParts = new CombineInstance[blocks.Count];

            for (int i = 0; i < blocks.Count; i++)
            {
                Matrix4x4 matrix = new Matrix4x4();
                Vector3 position = new Vector3(TILESIZE * blocks[i].iPos, TILEHEIGHT * blocks[i].kPos, TILESIZE * blocks[i].jPos);
                Debug.Log(blocks[i].dir);
                Quaternion rotation = Quaternion.AngleAxis(blocks[i].dir * 90f, Vector3.up);
                Vector3 scale = Vector3.one;
                matrix.SetTRS(position, rotation, scale);
                blockParts[i].transform = matrix;
                blockParts[i].mesh = BrickManager.Instance.bricks[blocks[i].reference].mesh;
            }

            GameObject newBlock = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newBlock.transform.parent = this.transform;
            newBlock.transform.localPosition = Vector3.zero;
            newBlock.transform.localRotation = Quaternion.identity;
            newBlock.GetComponent<MeshFilter>().mesh.CombineMeshes(blockParts, true, true);
            newBlock.GetComponent<Renderer>().material.mainTexture = BrickManager.Instance.textures[textureName];
        }
    }
}
