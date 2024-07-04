using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TrinketUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject fourLeafCloverImage;

    [SerializeField]
    private GameObject rabbitsFootImage;

    [SerializeField]
    private GameObject horseshoeImage;

    private Dictionary<GameState, GameObject> trinketImages;

    private void Awake()
    {
        trinketImages = new Dictionary<GameState, GameObject>
        {
            { GameState.FourLeafCloverFound, fourLeafCloverImage },
            { GameState.RabbitsFootFound, rabbitsFootImage },
            { GameState.HorseshoeFound, horseshoeImage }
        };

        ToggleAllTrinkets(false);

        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void Start()
    {
        GameManager.RegisterDialogueCallback("leprechaun_35", ToggleOffAllTrinkets);
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    public IEnumerator ToggleOffAllTrinkets()
    {
        ToggleAllTrinkets(false);
        yield return null;
    }

    public void ToggleAllTrinkets(bool state)
    {
        fourLeafCloverImage.SetActive(state);
        rabbitsFootImage.SetActive(state);
        horseshoeImage.SetActive(state);
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (trinketImages.TryGetValue(state, out GameObject trinket))
        {
            trinket.SetActive(true);
        }
    }
}
