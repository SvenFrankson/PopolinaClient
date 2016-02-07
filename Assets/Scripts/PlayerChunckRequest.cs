using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PlayerChunckRequest : MonoBehaviour
{
    public int size;
    private int iPos = -1;
    private int jPos = -1;

    public void Start()
    {
        int startIPos = Mathf.FloorToInt(this.transform.position.x / (Chunck.CHUNCKSIZE * Chunck.TILESIZE));
        int startJPos = Mathf.FloorToInt(this.transform.position.z / (Chunck.CHUNCKSIZE * Chunck.TILESIZE));
        for (int i = startIPos - 1; i <= startIPos + 1; i++)
        {
            for (int j = startJPos - 1; j <= startJPos + 1; j++)
            {
                ChunckManager.Query(i, j);
            }
        }
    }

    public void Update()
    {
        int newIPos = Mathf.FloorToInt(this.transform.position.x / (Chunck.CHUNCKSIZE * Chunck.TILESIZE));
        int newJPos = Mathf.FloorToInt(this.transform.position.z / (Chunck.CHUNCKSIZE * Chunck.TILESIZE));

        if ((newIPos != iPos) || (newJPos != jPos))
        {
            iPos = newIPos;
            jPos = newJPos;
            for (int i = iPos - size; i <= iPos + size; i++)
            {
                for (int j = jPos - size; j <= jPos + size; j++)
                {
                    ChunckManager.Query(i, j);
                }
            }
        }
    }
}
