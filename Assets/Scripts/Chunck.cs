using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunck : MonoBehaviour 
{
    public static float TILESIZE = 0.5f;
    public static float TILEHEIGHT = 0.2f;
    public static int CHUNCKSIZE = 32;
    public bool set = false;
    public int iPos { get; set; }
    public int jPos { get; set; }
    public int[] map { get; set; }
    public Block[] blocks;

    public Chunck()
    {

    }

    public void SetData(ChunckData data)
    {
        iPos = data.iPos;
        jPos = data.jPos;
        map = new int[CHUNCKSIZE * CHUNCKSIZE];
        data.map.CopyTo(map, 0);
        blocks = new Block[data.blocks.Length];
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i] = new Block();
            blocks[i].SetData(data.blocks[i]);
        }
    }

    public void SetMesh(Chunck[][] sideChuncks)
    {
        this.GetComponent<MeshFilter>().mesh = this.BuildMesh(sideChuncks);
        this.GetComponent<MeshCollider>().sharedMesh = this.GetComponent<MeshFilter>().mesh;
        StartCoroutine(BrickManager.Instance.RequestBrickOn(this));
        this.set = true;
    }

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
                trianglesList.Add(i + j * (CHUNCKSIZE + 1));
                trianglesList.Add(i + 1 + (j + 1) * (CHUNCKSIZE + 1));
                trianglesList.Add(i + 1 + j * (CHUNCKSIZE + 1));

                trianglesList.Add(i + j * (CHUNCKSIZE + 1));
                trianglesList.Add(i + (j + 1) * (CHUNCKSIZE + 1));
                trianglesList.Add(i + 1 + (j + 1) * (CHUNCKSIZE + 1));
            }
        }

        m.vertices = verticesList.ToArray();
        m.triangles = trianglesList.ToArray();
        m.RecalculateNormals();

        return m;
    }

    public void BuildBlocks()
    {
        foreach (Block b in blocks)
        {
            GameObject newBlock = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            newBlock.transform.parent = this.transform;
            newBlock.transform.localPosition = new Vector3(TILESIZE * b.iPos, TILEHEIGHT * b.kPos, TILESIZE * b.jPos);
            newBlock.GetComponent<MeshFilter>().mesh = BrickManager.Instance.bricks[b.reference].mesh;
        }
    }
}
