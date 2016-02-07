using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ChunckManager : MonoBehaviour
{
    static private List<Chunck> chuncks = new List<Chunck>();
    static private List<int[]> queriedChuncks = new List<int[]>();
    public Chunck template;

    static public void Query(int i, int j)
    {
        queriedChuncks.Add(new int[] { i, j });
    }

    static public void Add(Chunck newChunck)
    {
        int i = newChunck.iPos;
        int j = newChunck.jPos;
        chuncks.Add(newChunck);
        TrySetMesh(newChunck);
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Chunck side = chuncks.Find(c => (c.iPos == i - 1 + x) && (c.jPos == j - 1 + y));
                TrySetMesh(side);
            }
        }
    }

    static public void TrySetMesh(Chunck c)
    {
        if ((c == null) || (c.set == true))
        {
            return;
        }

        int i = c.iPos;
        int j = c.jPos;
        if ((i > 0) && (j > 0))
        {
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
                queriedChuncks.RemoveAt(0);
                if (chuncks.Find(o => (o.iPos == i) && (o.jPos == j)) == null)
                {
                    WWW request = new WWW("http://localhost:8080/maps/" + i + "/" + j);
                    yield return request;
                    ChunckData c = JsonUtility.FromJson<ChunckData>(request.text);
                    BuildChunck(c);
                }
            }
            yield return null;
        }
    }

    public void BuildChunck(ChunckData c)
    {
        Chunck chunck = Instantiate(this.template);
        chunck.transform.position = new Vector3(Chunck.TILESIZE * c.iPos * Chunck.CHUNCKSIZE, 0f, Chunck.TILESIZE * c.jPos * Chunck.CHUNCKSIZE);
        chunck.SetData(c);
        Add(chunck);
    }
}