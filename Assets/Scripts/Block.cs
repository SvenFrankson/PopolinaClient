using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Block
{
    public int iPos;
    public int jPos;
    public int kPos;
    public int dir;
    public string reference;

    public Block()
    {

    }

    public void SetData(BlockData data)
    {
        iPos = data.iPos;
        jPos = data.jPos;
        kPos = data.kPos;
        dir = data.dir;
        reference = data.reference;
    }
}
