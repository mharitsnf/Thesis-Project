using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataProcessor : MonoBehaviour
{
    public TextAsset data;
    public LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        string[] rows = data.text.Split('\n');
        lineRenderer.positionCount = rows.Length - 1;
        print(rows.Length - 1);

        for (int i = 1; i < rows.Length; i++)
        {
            var content = rows[i].Split(";");
            lineRenderer.SetPosition(i-1, new Vector3(float.Parse(content[2]), float.Parse(content[3]), float.Parse(content[4])));
        }
    }
}
