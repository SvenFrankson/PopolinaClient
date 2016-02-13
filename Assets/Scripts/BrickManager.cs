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
    public Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

    public void Add(Brick brick)
    {
        bricks.Add(brick.reference, brick);
    }

    public void Add(string textureName, Texture2D texture)
    {
        textures.Add(textureName, texture);
    }

    // Todo : Running this coroutine several times quickly will cause client to request several time the same mesh or texture. Solve it.
    public IEnumerator RequestBrickOn(Chunck chunck)
    {
        List<string> bricksToLoad = new List<string>();
        List<string> texturesToLoad = new List<string>();
        foreach (string textureName in chunck.texturedBlocks.Keys)
        {
            if (!textures.ContainsKey(textureName))
            {
                if (!texturesToLoad.Contains(textureName))
                {
                    texturesToLoad.Add(textureName);
                }
            }
            foreach (Block b in chunck.texturedBlocks[textureName])
            {
                if (!bricks.ContainsKey(b.reference))
                {
                    if (!bricksToLoad.Contains(b.reference))
                    {
                        bricksToLoad.Add(b.reference);
                    }
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
        foreach (string texture in texturesToLoad)
        {
            Texture2D newTexture = new Texture2D(16, 16);
            WWW request = new WWW("http://localhost:8080/textures/" + texture + ".png");
            yield return request;
            request.LoadImageIntoTexture(newTexture);
            if (!textures.ContainsKey(texture))
            {
                Add(texture, newTexture);
            }
        }
        chunck.BuildBlocks();
    }
}