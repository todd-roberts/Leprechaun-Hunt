using UnityEngine;
using UnityEngine.UI;

public class DirectionalIndicatorManager : MonoBehaviour
{
    public Image leftArrow;
    public Image rightArrow;

    public Transform leprechaun;

    void Update()
    {
        if (GameManager.GetGameState() != GameState.CatchLeprechaun) {
            return;
        }
        UpdateArrows();
    }

    private void UpdateArrows()
    {
        if (leprechaun == null) {
            leprechaun = FindObjectOfType<Leprechaun>().transform;
        }
        
        Vector3 direction = leprechaun.position - Camera.main.transform.position;

        // Reset the arrows to inactive
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);

        // Check horizontal direction
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            if (direction.x > 0)
            {
                rightArrow.gameObject.SetActive(true);
            }
            else
            {
                leftArrow.gameObject.SetActive(true);
            }
        }
    }
}
