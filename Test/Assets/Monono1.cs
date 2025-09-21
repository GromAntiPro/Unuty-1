using UnityEngine;
using System.Collections.Generic;


public class Monono : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int sumCubes = 8;
    [SerializeField] private float radius = 5f; 
    [SerializeField] private Vector3 center = Vector3.zero;
    [SerializeField] private float selfSpeed = 90f;    
    [SerializeField] private float rotationSpeed = 30f;    
    [SerializeField] private Transform centerObject;      
    [SerializeField] private Direction orbitDirection = Direction.Left;
    [SerializeField] private PlacementMode placementMode = PlacementMode.Evenly;
    [SerializeField] private float spacingBetweenPlaces = 0.9f;      
    
    public GameObject Prefab
    {
        get => prefab;
        set => prefab = value;

    }

    public int CubSum
    {
        get => sumCubes;
        set => sumCubes = value;

    }

    public float Rad
    {
        get => radius;
        set => radius = value;          
    }    

    public Vector3 Center
    {
        get => center;
        set => center = value;
    }
    
    public float SelfSpeed
    {
        get => selfSpeed;
        set => selfSpeed = value;
    }
    
    public float RotationSpeed
    {
        get => rotationSpeed;
        set => rotationSpeed = value;
    }
    

    public Transform CenterObject
    {
        get => centerObject;
        set => centerObject = value;
    }
    
    private enum Direction
    {
        Left = 1,
        Right = -1
    }
        
    private enum PlacementMode
    {
        Evenly,   
        Sequential 
    }
    
    public float SpacingBetweenPlaces
    {
        get => spacingBetweenPlaces;
        set => spacingBetweenPlaces = value;
    }    
        
    private GameObject[] _objects;
    private float[] _angles;
    private int _lastCount;
    private PlacementMode _lastPlacementMode;
    private float _lastSpacing;

    private void Awake()
    {
        centerObject ??= this.transform;

        InitializeObjects();
        _lastPlacementMode = placementMode;
        _lastSpacing = spacingBetweenPlaces;
    }

    private void Update()
    {
        if (sumCubes != _lastCount ||           
            placementMode != _lastPlacementMode ||
            Mathf.Abs(spacingBetweenPlaces - _lastSpacing) > 0.001f)
        {
            InitializeObjects();       
        }

        var dir = (int)orbitDirection;
        
        for (var i = 0; i < CubSum; i++)
        {
            _angles[i] += RotationSpeed * Mathf.Deg2Rad * dir * Time.deltaTime;

            Vector3 pos = new(
                centerObject.position.x + Mathf.Cos(_angles[i]) * Rad,
                centerObject.position.y,
                centerObject.position.z + Mathf.Sin(_angles[i]) * Rad
            );

            _objects[i].transform.position = pos;
            _objects[i].transform.Rotate(Vector3.up, selfSpeed * Time.deltaTime, Space.Self);
        }
    }
    
    private void InitializeObjects()
    {
        if (_objects != null)
            foreach (var obj in _objects)
                if (obj)
                    Destroy(obj);

        _objects = new GameObject[CubSum];
        _angles = new float[CubSum];
        _lastCount = CubSum;

        var step = 2f * Mathf.PI / CubSum;
        
        for (var i = 0; i < CubSum; i++)
        {
            var angle = placementMode switch
            {
                PlacementMode.Evenly => i * step,
                PlacementMode.Sequential => i * SpacingBetweenPlaces,
                _ => i * step
            };
            Vector3 pos = new(
                centerObject.position.x + Mathf.Cos(angle) * Rad,
                centerObject.position.y,
                centerObject.position.z + Mathf.Sin(angle) * Rad
            );

            _objects[i] = Instantiate(Prefab, pos, Quaternion.LookRotation(centerObject.position - pos), transform);
            _angles[i] = angle;
        }
    }
}