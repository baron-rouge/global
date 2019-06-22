using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PartLoader : MonoBehaviour
{
    GameObject cylinder;
    private void Awake()
    {
        var AB = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "AssetBundles/test"));
        if (AB == null)
        {
            Debug.Log("Failed to load asset bundle");
            return;
        }
        cylinder = AB.LoadAsset<GameObject>("Cylinder");
    }

    private void Start()
    {
        if (cylinder == null)
            return;
        Instantiate(cylinder);
    }
}
