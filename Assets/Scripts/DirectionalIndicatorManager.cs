using UnityEngine;
using UnityEngine.UI;

public class DirectionalIndicatorManager : MonoBehaviour
{
    public Image leftArrow;
    public Image rightArrow;

    private Transform leprechaun;

    void Update()
    {
        if (GameManager.GetGameState() != GameState.CatchLeprechaun)
        {
            leftArrow.gameObject.SetActive(false);
            rightArrow.gameObject.SetActive(false);
            return;
        }

        UpdateArrows();
    }

    private void UpdateArrows()
    {
        if (leprechaun == null)
        {
            Leprechaun foundLeprechaun = FindObjectOfType<Leprechaun>();
            if (foundLeprechaun != null)
            {
                leprechaun = foundLeprechaun.transform;
            }
            else
            {
                return;
            }
        }

        Vector3 screenPoint = Camera.main.WorldToScreenPoint(leprechaun.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < Screen.width && screenPoint.y > 0 && screenPoint.y < Screen.height;

        if (onScreen)
        {
            leftArrow.gameObject.SetActive(false);
            rightArrow.gameObject.SetActive(false);
            return;
        }

        Vector3 direction = Camera.main.transform.InverseTransformPoint(leprechaun.position);

        // Reset the arrows to inactive
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);

        // Check horizontal direction
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
