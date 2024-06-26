using UnityEngine;

public class Clover : MonoBehaviour
{
    public float checkInterval = 0.25f;
    private Renderer cloverRenderer;
    public bool isFourLeafClover = false;

    void Start()
    {
        cloverRenderer = GetComponent<Renderer>();
        InvokeRepeating(nameof(CheckVisibility), 0, checkInterval);
    }

    void CheckVisibility()
    {
        if (IsInView())
        {
            EnableClover();
        }
        else
        {
            DisableClover();
        }
    }

    bool IsInView()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(planes, cloverRenderer.bounds);
    }

    void EnableClover()
    {
        cloverRenderer.enabled = true;
    }

    void DisableClover()
    {
        cloverRenderer.enabled = false;
    }

    public void OnPointerDown()
    {
        CloverGame cloverGame = CloverGame.Instance;
        cloverGame.HandleCloverClick(this);
    }
}
