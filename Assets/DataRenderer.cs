using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class DataRenderer : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private MaterialPropertyBlock _propertyBlock;
    private TextAsset data;
    
    // Start is called before the first frame update
    void Start()
    {
        _propertyBlock = new MaterialPropertyBlock();

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[] {new GradientColorKey(Color.green, 0f), new GradientColorKey(Color.red, 1f)},
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 0f) }
            );
        
        lineRenderer.colorGradient = gradient;
    }

    private void OnEnable()
    {
        DataVisualizer.OnPercentageChange += OnPercentageChange;
    }

    private void OnDisable()
    {
        DataVisualizer.OnPercentageChange -= OnPercentageChange;
    }

    private void OnPercentageChange(int lowerLimit, int upperLimit)
    {
        if (!data) return;
        lineRenderer.positionCount = 0;

        string[] rows = data.text.Split('\n');
        List<string> newRows = rows.ToList();
        newRows.RemoveAt(0); // remove header
        newRows.RemoveAt(newRows.Count - 1); // remove empty row
        int rowsCount = rows.Length;

        int startRow = (int) Mathf.Floor(rowsCount * (lowerLimit / 100f));
        int endRow = (int) Mathf.Floor(rowsCount * (upperLimit / 100f));
        lineRenderer.positionCount = endRow - startRow;

        int index = 0;
        for (int i = startRow; i < endRow; i++)
        {
            try
            {
                var content = newRows[i].Split(";");
                float x = float.Parse(content[2].Replace(".", ","));
                float y = float.Parse(content[3].Replace(".", ","));
                float z = float.Parse(content[4].Replace(".", ","));
                Vector3 pointPosition = new Vector3(x, y, z);

                lineRenderer.SetPosition(index, pointPosition);
                index++;
            }
            catch (Exception e)
            {
                print($"index: {i} {index} {newRows[i]}");
            }
        }
    }

    public void LoadData(TextAsset newData)
    {
        data = newData;
    }
}
