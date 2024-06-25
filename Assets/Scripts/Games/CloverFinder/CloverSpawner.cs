using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Unity.Collections;
using System.Collections.Generic;

public class CloverSpawner : MonoBehaviour
{
    public GameObject threeLeafCloverPrefab;
    public GameObject fourLeafCloverPrefab;
    public int numberOfClovers = 50;
    public Material grassMaterial;

    private List<ARPlane> planes = new List<ARPlane>();
    private ARPlaneManager _planeManager;

    void Start()
    {
        _planeManager = GetComponent<ARPlaneManager>();
        _planeManager.planesChanged += OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (ARPlane plane in args.added)
        {
            if (!planes.Contains(plane) && IsPlaneValid(plane))
            {
                planes.Add(plane);
                SpawnClovers(plane);
            }
        }
    }

    private bool IsPlaneValid(ARPlane plane)
    {
        // Check if the plane is below the camera
        if (plane.transform.position.y >= Camera.main.transform.position.y)
        {
            return false;
        }

        // Check if the plane is level with the ground (normal vector pointing up)
        if (Vector3.Dot(plane.transform.up, Vector3.up) < 0.9f)
        {
            return false;
        }

        return true;
    }

    private void SpawnClovers(ARPlane plane)
    {
        NativeArray<Vector2> boundaryPoints = plane.boundary;
        Vector3[] boundaryPoints3D = new Vector3[boundaryPoints.Length];

        for (int i = 0; i < boundaryPoints.Length; i++)
        {
            boundaryPoints3D[i] = plane.transform.TransformPoint(new Vector3(boundaryPoints[i].x, 0, boundaryPoints[i].y));
        }

        Vector3 center = plane.transform.position;
        Quaternion rotation = plane.transform.rotation;

        // Create a grass plane
        GameObject grassPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        grassPlane.transform.position = center;
        grassPlane.transform.rotation = rotation;
        grassPlane.transform.localScale = new Vector3(plane.size.x, 1, plane.size.y) * 0.1f;
        grassPlane.GetComponent<Renderer>().material = grassMaterial;

        // Spawn clovers
        for (int i = 0; i < numberOfClovers; i++)
        {
            Vector3 randomPosition = GetRandomPointInPlane(boundaryPoints3D) + center;
            GameObject clover = Instantiate(threeLeafCloverPrefab, randomPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
            clover.transform.localScale *= Random.Range(0.9f, 1.1f);
        }

        // Spawn one 4-leaf clover
        Vector3 specialPosition = GetRandomPointInPlane(boundaryPoints3D) + center;
        GameObject fourLeafClover = Instantiate(fourLeafCloverPrefab, specialPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
        fourLeafClover.transform.localScale *= Random.Range(0.9f, 1.1f);
    }

    private Vector3 GetRandomPointInPlane(Vector3[] boundary)
    {
        // Generate a random point inside the plane boundary using the centroid method
        Vector3 randomPoint = Vector3.zero;
        float area = 0;

        for (int i = 1; i < boundary.Length - 1; i++)
        {
            Vector3 v0 = boundary[0];
            Vector3 v1 = boundary[i];
            Vector3 v2 = boundary[i + 1];

            float triangleArea = Vector3.Cross(v1 - v0, v2 - v0).magnitude * 0.5f;
            area += triangleArea;

            randomPoint += triangleArea * (v0 + v1 + v2) / 3;
        }

        randomPoint /= area;
        return randomPoint;
    }
}
