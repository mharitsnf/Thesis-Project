using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataVisualizer : MonoBehaviour
{
    public delegate void PercentageChange(int lowerLimit, int upperLimit);
    public static event PercentageChange OnPercentageChange;
    
    public List<TextAsset> playerFiles;
    public GameObject rendererPrefab;

    public int lowerLimit;
    public int upperLimit;

    // Start is called before the first frame update
    void Start()
    {
        int index = 0;
        foreach (var data in playerFiles)
        {
            GameObject dataRendererObject = Instantiate(rendererPrefab, transform);
            dataRendererObject.name = index.ToString();

            DataRenderer dataRenderer = dataRendererObject.GetComponent<DataRenderer>();
            
            dataRenderer.LoadData(data);
            
            index++;
        }
        
        OnPercentageChange?.Invoke(lowerLimit, upperLimit);
    }

    IEnumerator AnimateUpperLimit()
    {
        while (upperLimit < 100)
        {
            upperLimit++;
            yield return new WaitForSeconds(.5f);
        }
    }

    private void OnValidate()
    {
        if (lowerLimit >= upperLimit) upperLimit = lowerLimit + 1;
        OnPercentageChange?.Invoke(lowerLimit, upperLimit);
    }
}
