using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class ObjectMechanicsController : MonoBehaviour
{
    private GameObject _parent;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    
    private Rigidbody _rb;
    private MeshRenderer _meshRenderer;
    private ParticleSystem _particleSystem;
    
    public Material[] materials;
    private static readonly int TimeScale = Shader.PropertyToID("_Time_Scale");

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
        _particleSystem = transform.GetChild(0).GetComponent<ParticleSystem>();

        SetMaterial(0);
    }

    private void Start()
    {
        _initialPosition = _parent.transform.position;
        _initialRotation = _parent.transform.rotation;
    }

    private void OnEnable()
    {
        InteractionController.OnToggleAiming.AddListener(ToggleAiming);
    }

    private void OnDisable()
    {
        InteractionController.OnToggleAiming.AddListener(ToggleAiming);
    }

    private void ToggleAiming(bool isAiming)
    {
        if (isAiming)
        {
            SetMaterial(Vector3.Distance(PlayerData.Instance.realCamera.position, transform.position) > PlayerData.Instance.ropeRayCastDistance ? 4 : 1);
        }
        else
        {
            SetMaterial(_rb.freezeRotation ? 3 : 0);
        }
    }

    public void SetSelected()
    {
        SetMaterial(2);
    }

    public void PlayParticle()
    {
        _particleSystem.Play();
    }

    public void SetMaterial(int index)
    {
        _meshRenderer.material = materials[index];
        if (index == 2)
        {
            _meshRenderer.material.SetFloat(TimeScale, 1/Time.timeScale);
        }
    }
    
    private void ResetPosition()
    {
        if (!(transform.position.y < -50f)) return;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _parent.transform.position = _initialPosition;
        _parent.transform.rotation = _initialRotation;
    }

    private void FixedUpdate()
    {
        ResetPosition();
    }

    private void ResetMaterial()
    {
        if (PlayerData.Instance.isAiming) return;
        if (!_rb.freezeRotation || _parent.GetComponents<SpringJoint>().Length != 0) return;
        
        _rb.freezeRotation = false;
        SetMaterial(0);
    }

    private void UpdateMaterial()
    {
        if (!PlayerData.Instance.isAiming) return;
        if (!PlayerData.Instance.selectedGameObject.Equals(default(RaycastHit)) && PlayerData.Instance.selectedGameObject.collider.gameObject.Equals(_parent)) return;
        
        SetMaterial(Vector3.Distance(PlayerData.Instance.realCamera.position, transform.position) > PlayerData.Instance.ropeRayCastDistance ? 4 : 1);
    }

    private void LateUpdate()
    {
        UpdateMaterial();
        ResetMaterial();
    }
}
