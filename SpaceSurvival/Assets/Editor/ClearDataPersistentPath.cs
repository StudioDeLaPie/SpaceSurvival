using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ClearDataPersistentPath : Editor
{
    [MenuItem("■ Tool/Clear Data Persistent Path %SPACE")]
    static public void test()
    {
        Debug.Log("Clear Persistent Path");
        DirectoryInfo dataDir = new DirectoryInfo(Application.persistentDataPath);
        dataDir.Delete(true);
    }
}
