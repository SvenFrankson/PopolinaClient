using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TemplateCreator : EditorWindow
{
    public string reference;
    public GameObject rootTarget;
    public string output;

    [MenuItem("Popolina/TemplateCreator")]
    static void Init()
    {
        TemplateCreator window = EditorWindow.GetWindow(typeof(TemplateCreator)) as TemplateCreator;
        window.Show();
    }

    void OnGUI()
    {
        this.reference = EditorGUILayout.TextField("Reference", reference);
        this.rootTarget = EditorGUILayout.ObjectField("Root Target", rootTarget, typeof(GameObject), true) as GameObject;
        if (GUILayout.Button("Get JSON Format"))
        {
            output = CreateJSON(this.rootTarget);
        }
        EditorGUILayout.TextArea(output);
    }

    public string CreateJSON(GameObject target)
    {
        Transform[] blocks = target.GetComponentsInChildren<Transform>();
        Debug.Log(blocks.Length);
        List<BlockData> datas = new List<BlockData>();

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].transform.parent == target.transform)
            {
                BlockData data = new BlockData();

                data.iPos = Mathf.RoundToInt(blocks[i].transform.localPosition.x / Chunck.TILESIZE);
                data.jPos = Mathf.RoundToInt(blocks[i].transform.localPosition.z / Chunck.TILESIZE);
                data.kPos = Mathf.RoundToInt(blocks[i].transform.localPosition.y / Chunck.TILEHEIGHT);
                data.dir = Mathf.RoundToInt(blocks[i].transform.localRotation.eulerAngles.y / 90f);
                data.reference = blocks[i].GetComponent<MeshFilter>().sharedMesh.name.Split(' ')[0];
                data.texture = blocks[i].GetComponent<MeshRenderer>().sharedMaterial.name.Split(' ')[0];

                datas.Add(data);
            }
        }

        string jsonString = "{\"reference\" : \"" + reference + "\", \"blocks\" : [";
        foreach (BlockData data in datas)
        {
            jsonString += JsonUtility.ToJson(data);
            jsonString += ", ";
        }
        jsonString = jsonString.Remove(jsonString.Length - 2, 2);
        jsonString += "]}";

        return jsonString;
    }
}