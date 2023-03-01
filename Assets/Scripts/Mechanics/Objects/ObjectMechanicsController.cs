using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class ObjectMechanicsController : MonoBehaviour
{
    private GameObject _parent;
    
    private Rigidbody _rb;
    private MeshRenderer _meshRenderer;
    public Material[] materials;
    
    private void Awake()
    {
        try
        {
            _parent = transform.parent.gameObject;
        }
        catch (Exception)
        {
            Debug.Break();
            return;
        }
        
        gameObject.tag = "Object";
        _parent.tag = "Object";
        
        _rb = _parent.GetComponent<Rigidbody>();
        _meshRenderer = _parent.GetComponent<MeshRenderer>();
        
        SetMaterial(0);
    }

    private void SetMaterial(int index)
    {
        _meshRenderer.material = materials[index];
    }
    
    private void LateUpdate()
    {
        if (_parent.GetComponents<SpringJoint>().Length == 0) _rb.freezeRotation = false;
    }
}
