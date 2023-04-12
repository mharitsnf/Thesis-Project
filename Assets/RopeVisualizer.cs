using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class RopeVisualizer : MonoBehaviour
{
    public GameObject ropeRendererPrefab;

    public void LoadData(string[] paths)
    {
        GameObject ropeParent = new GameObject();
        ropeParent.name = "Ropes";
        ropeParent.transform.parent = transform;
        
        for (int i = 0; i < paths.Length - 1; i++)
        {
            string path = paths[i];
        
            if (path.EndsWith(".meta")) continue;
        
            string filename = path.Split("\\")[2];
            
            string pattern = @"\d+(\.\d+)?(?=\.)";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(filename);
            if (match.Success)
            {
                GameObject ropeRenderer = Instantiate(ropeRendererPrefab, ropeParent.transform);
                ropeRenderer.name = $"Rope-{match.Value}";
                
                ropeRenderer.GetComponent<RopeDataProcessor>().LoadData(path);
            }
        }
    }
}
