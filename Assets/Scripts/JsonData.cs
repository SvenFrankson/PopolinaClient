using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ChunckData 
{
	public int iPos;
	public int jPos;
	public int[] map;
    public BlockData[] blocks;
}

[Serializable]
public class BlockData
{
    public int iPos;
    public int jPos;
    public int kPos;
    public int dir;
    public string reference;
    public string texture;
}

[Serializable]
public class BrickData
{
    public string reference;
    public double[] vertices;
    public double[] uvs;
    public double[] triangles;
}