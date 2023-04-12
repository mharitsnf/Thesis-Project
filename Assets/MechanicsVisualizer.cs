using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicsVisualizer : MonoBehaviour
{
    // public MechanicRendererPrefab mechanicRendererPrefab;
    
    public GameObject mechanicsAPrefab;
    public GameObject mechanicsBPrefab;
    public GameObject mechanicsCPrefab;
    public GameObject mechanicsDPrefab;
    public GameObject mechanicsEPrefab;
    public GameObject mechanicsFPrefab;

    public void LoadData(string[] paths)
    {
        GameObject mechanicParent = new GameObject();
        mechanicParent.name = "Mechanics";
        mechanicParent.transform.parent = transform;
        mechanicParent.SetActive(false);
        
        foreach (var path in paths)
        {
            if (path.EndsWith(".meta")) continue;
            
            string mechanicsName = path.Split("\\")[2].Split(".")[0];

            GameObject mechanicsGameObject;
            switch (mechanicsName)
            {
                case "A":
                    mechanicsGameObject = Instantiate(mechanicsAPrefab, mechanicParent.transform);
                    mechanicsGameObject.name = "A";
                    mechanicsGameObject.GetComponent<MechanicsDataProcessor>().LoadData(path);
                    break;
                case "B":
                    mechanicsGameObject = Instantiate(mechanicsBPrefab, mechanicParent.transform);
                    mechanicsGameObject.name = "B";
                    mechanicsGameObject.GetComponent<MechanicsDataProcessor>().LoadData(path);
                    break;
                case "C":
                    mechanicsGameObject = Instantiate(mechanicsCPrefab, mechanicParent.transform);
                    mechanicsGameObject.name = "C";
                    mechanicsGameObject.GetComponent<MechanicsDataProcessor>().LoadData(path);
                    break;
                case "D":
                    mechanicsGameObject = Instantiate(mechanicsDPrefab, mechanicParent.transform);
                    mechanicsGameObject.name = "D";
                    mechanicsGameObject.GetComponent<MechanicsDataProcessor>().LoadData(path);
                    break;
                case "E":
                    mechanicsGameObject = Instantiate(mechanicsEPrefab, mechanicParent.transform);
                    mechanicsGameObject.name = "E";
                    mechanicsGameObject.GetComponent<MechanicsDataProcessor>().LoadData(path);
                    break;
                case "F":
                    mechanicsGameObject = Instantiate(mechanicsFPrefab, mechanicParent.transform);
                    mechanicsGameObject.name = "F";
                    mechanicsGameObject.GetComponent<MechanicsDataProcessor>().LoadData(path);
                    break;
            }
        }
    }
    
    
}
