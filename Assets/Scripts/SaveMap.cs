
using UnityEngine;
using System.Collections;
using UnityEditor;

public class SaveMap : MonoBehaviour
{
    //Start is called before the first frame update
    int counter=0;
    private void Start()
    {
      
    }
    public void SaveLevelMap()
    {
        string name = "Map_" + counter;
        var scene = GameObject.Find("Grid");
        if (scene)
        {
            var save = "Assets/Prefabs/" + name + ".prefab";
            if (PrefabUtility.SaveAsPrefabAsset(scene, save)) 
               

            {
                EditorUtility.DisplayDialog("Tilemap saved", "Tilemap saved to " + save, "continue");
            }
            else
            {
                EditorUtility.DisplayDialog("Tilemap not saved", "There was an error saving the tilemap " + save, "continue");
            }
        }


    }

   // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveLevelMap();
            counter++;
        }
    }

}
