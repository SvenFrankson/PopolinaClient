using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ChunckManager : MonoBehaviour
{
    static private ChunckManager instance;
    static private ChunckManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ChunckManager>();
            }
            return instance;
        }
    }
    static private List<Chunck> chuncks = new List<Chunck>();
    static private List<int[]> queriedChuncks = new List<int[]>();
    public Chunck template;
    static private Chunck Template
    {
        get
        {
            return Instance.template;
        }
    }

    static public void Query(int i, int j, int update = 0)
    {
        queriedChuncks.Add(new int[] { i, j, update });
    }

    static public void Add(ChunckData newChunckData)
    {
        if (chuncks.Find(c => (c.iPos == newChunckData.iPos) && (c.jPos == newChunckData.jPos)) != null)
        {
            return;
        }
        int i = newChunckData.iPos;
        int j = newChunckData.jPos;
        Chunck newChunck = Instantiate(Template);
        newChunck.SetData(newChunckData);
        chuncks.Add(newChunck);
        TrySetMesh(newChunck);
        newChunck.SetBlock();
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Chunck side = chuncks.Find(c => (c.iPos == i - 1 + x) && (c.jPos == j - 1 + y));
                TrySetMesh(side);
            }
        }
    }

    static public void UpdateChunckData(ChunckData chunckData)
    {
        Chunck chunck = chuncks.Find(c => (c.iPos == chunckData.iPos) && (c.jPos == chunckData.jPos));
        if (chunck == null)
        {
            return;
        }
        chunck.SetData(chunckData);
    }

    static public void UpdateChunckMesh(Chunck chunck)
    {
        Debug.Log("Mesh Update Request 3");
        TrySetMesh(chunck);
    }

    static public void UpdateChunckBlocks(Chunck chunck)
    {
        chunck.SetBlock();
    }

    static public void TrySetMesh(Chunck c)
    {
        if (c == null)
        {
            return;
        }

        int i = c.iPos;
        int j = c.jPos;
        Chunck[][] sideChuncks = new Chunck[3][];
        for (int x = 0; x < 3; x++)
        {
            sideChuncks[x] = new Chunck[3];
            for (int y = 0; y < 3; y++)
            {
                sideChuncks[x][y] = chuncks.Find(o => (o.iPos == i - 1 + x) && (o.jPos == j - 1 + y));
                if (sideChuncks[x][y] == null)
                {
                    return;
                }
            }
        }
        c.SetMesh(sideChuncks);
    }

    public void Start()
    {
        StartCoroutine(RequestLoop());
    }

    IEnumerator RequestLoop()
    {
        while (true)
        {
            while (queriedChuncks.Count > 0)
            {
                int i = queriedChuncks[0][0];
                int j = queriedChuncks[0][1];
                int update = queriedChuncks[0][2];
                queriedChuncks.RemoveAt(0);
                if (update == 0) // Request new chunck
                {
                    if (chuncks.Find(o => (o.iPos == i) && (o.jPos == j)) == null)
                    {
                        WWW request = new WWW("http://localhost:8080/maps/" + i + "/" + j);
                        yield return request;
                        ChunckData c = JsonUtility.FromJson<ChunckData>(request.text);
                        Add(c);
                    }
                }
                if (update == 1) // Request chunckdata update
                {
                    Debug.Log("Mesh ChunckData Request");
                    if (chuncks.Find(o => (o.iPos == i) && (o.jPos == j)) != null)
                    {
                        Debug.Log("Mesh ChunckData Request 2");
                        WWW request = new WWW("http://localhost:8080/maps/" + i + "/" + j);
                        yield return request;
                        ChunckData c = JsonUtility.FromJson<ChunckData>(request.text);
                        UpdateChunckData(c);
                    }
                }
                if (update == 2) // Request mesh update
                {
                    Debug.Log("Mesh Update Request");
                    Chunck chunck = chuncks.Find(o => (o.iPos == i) && (o.jPos == j));
                    if (chunck != null)
                    {
                        Debug.Log("Mesh Update Request 2");
                        UpdateChunckMesh(chunck);
                    }
                }
                if (update == 3) // Request blocks update
                {
                    Chunck chunck = chuncks.Find(o => (o.iPos == i) && (o.jPos == j));
                    if (chunck != null)
                    {
                        UpdateChunckBlocks(chunck);
                    }
                }
            }
            yield return null;
        }
    }
}