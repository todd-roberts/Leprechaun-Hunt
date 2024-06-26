using UnityEngine;

public class CloverSpawner : MonoBehaviour
{
    public GameObject threeLeafCloverPrefab;
    public GameObject fourLeafCloverPrefab;
    public int totalNumberOfClovers = 50; // Total number of clovers

    [Range(0, 100)]
    public float percentageOfFourLeafClovers = 10.0f; // Percentage of clovers that should be 4-leaf
    public GameObject groundPlane; // Assign the pre-created ground plane in the inspector
    public float minDistanceFromCamera = 0.2f; // Minimum distance from the camera
    public float initialGroundPlaneHeight = 1.0f; // Initial distance below the camera

    public float zOffset = 1f;

    private void Awake()
    {
        groundPlane.SetActive(false); // Ensure the ground plane is initially inactive
    }

    public void Spawn()
    {
        SpawnClovers();
        groundPlane.SetActive(true);
        AdjustGroundPlanePosition(initialGroundPlaneHeight);
    }

    public void CloseGame()
    {
        groundPlane.SetActive(false);
    }

    void Update()
    {
        // Adjust the ground plane if the camera gets too close
        if (
            Camera.main.transform.position.y - groundPlane.transform.position.y
            < minDistanceFromCamera
        )
        {
            groundPlane.transform.position = new Vector3(
                groundPlane.transform.position.x,
                Camera.main.transform.position.y - minDistanceFromCamera,
                groundPlane.transform.position.z
            );
        }
    }

    private void AdjustGroundPlanePosition(float height)
    {
        groundPlane.transform.position = new Vector3(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y - height,
            Camera.main.transform.position.z + zOffset
        );
        groundPlane.transform.rotation = Quaternion.identity;
    }

    private void SpawnClovers()
    {
        int numberOfFourLeafClovers = Mathf.RoundToInt(
            totalNumberOfClovers * (percentageOfFourLeafClovers / 100)
        );
        int numberOfThreeLeafClovers = totalNumberOfClovers - numberOfFourLeafClovers;

        for (int i = 0; i < numberOfThreeLeafClovers; i++)
        {
            Vector3 randomPosition = GetRandomPointOnPlane();
            GameObject clover = Instantiate(
                threeLeafCloverPrefab,
                randomPosition,
                Quaternion.Euler(0, Random.Range(0, 360), 0)
            );
            clover.transform.localScale *= Random.Range(1.0f, 1.5f); // Scale up the clovers
            clover.transform.SetParent(groundPlane.transform); // Parent to ground plane
        }

        for (int i = 0; i < numberOfFourLeafClovers; i++)
        {
            Vector3 randomPosition = GetRandomPointOnPlane();
            GameObject clover = Instantiate(
                fourLeafCloverPrefab,
                randomPosition,
                Quaternion.Euler(0, Random.Range(0, 360), 0)
            );
            clover.transform.localScale *= Random.Range(1.0f, 1.5f); // Scale up the clovers
            clover.transform.SetParent(groundPlane.transform); // Parent to ground plane
        }
    }

    private Vector3 GetRandomPointOnPlane()
    {
        float halfScaleX = groundPlane.transform.localScale.x * 5; // 10 (scale) / 2
        float halfScaleZ = groundPlane.transform.localScale.z * 5; // 10 (scale) / 2

        float randomX = Random.Range(-halfScaleX, halfScaleX);
        float randomZ = Random.Range(-halfScaleZ, halfScaleZ);

        return new Vector3(
            groundPlane.transform.position.x + randomX,
            groundPlane.transform.position.y,
            groundPlane.transform.position.z + randomZ
        );
    }
}
