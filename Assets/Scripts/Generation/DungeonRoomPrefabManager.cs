using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DungeonRoomPrefabManager : MonoBehaviour
{
    [SerializeField] public GameObject WallPrefab;
    public Bounds WallBounds => WallPrefab.GetComponent<MeshFilter>().sharedMesh.bounds;
    [SerializeField] public GameObject FloorPrefab;
    public Bounds FloorBounds => FloorPrefab.GetComponent<MeshFilter>().sharedMesh.bounds;
    [SerializeField] public GameObject CeilingPrefab;
    public Bounds CeilingBounds => CeilingPrefab.GetComponent<MeshFilter>().sharedMesh.bounds;
    [SerializeField] public GameObject WallDoorPrefab;
    public Bounds WallDoorBounds => WallDoorPrefab.GetComponent<MeshFilter>().sharedMesh.bounds;
    [SerializeField] public GameObject WallCornerPrefab;
    public Bounds WallCornerBounds => WallCornerPrefab.GetComponent<MeshFilter>().sharedMesh.bounds;

    [SerializeField] public GameObject TorchPrefab;
    public Bounds TorchBounds => TorchPrefab.GetComponent<MeshFilter>().sharedMesh.bounds;

    [SerializeField] public GameObject EnemyPrefab;

    [SerializeField] public GameObject hallwayPrefab;

    public static readonly float Height = 4.0f;


    public static DungeonRoomPrefabManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }
}