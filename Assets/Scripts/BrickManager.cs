using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class BrickManager : MonoBehaviour
{
    private static BrickManager instance;
    public static BrickManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BrickManager>();
            }
            return instance;
        }
    }

    public Dictionary<string, Brick> bricks = new Dictionary<string, Brick>();

    public void Add(Brick brick)
    {
        bricks.Add(brick.reference, brick);
    }

    public IEnumerator RequestBrickOn(Chunck chunck)
    {
        List<string> bricksToLoad = new List<string>();
        foreach (Block b in chunck.blocks)
        {
            if (!bricks.ContainsKey(b.reference))
            {
                if (!bricksToLoad.Contains(b.reference))
                {
                    bricksToLoad.Add(b.reference);
                }
            }
        }
        foreach (string reference in bricksToLoad)
        {
            WWW request = new WWW("http://localhost:8080/bricks/" + reference);
            yield return request;
            if (!bricks.ContainsKey(reference))
            {
                BrickData bData = JsonUtility.FromJson<BrickData>(request.text);
                Brick b = new Brick();
                b.SetData(bData);
                Add(b);
            }
        }
        chunck.BuildBlocks();
    }
}