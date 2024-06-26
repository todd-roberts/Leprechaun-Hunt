using System.Collections;
using UnityEngine;

public class CloverGame : MonoBehaviour
{
    public static CloverGame Instance;

    public float inspectDuration = 2.0f; // Duration of the inspection

    private CloverSpawner _cloverSpawner;

    [SerializeField]
    private CloverInspector _cloverInspector;

    private void Awake()
    {
        Instance = this;
        _cloverSpawner = GetComponent<CloverSpawner>();
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.FindFourLeafClover)
        {
            StartGame();
        } else if (state == GameState.FourLeafCloverFound)
        {
            CloseGame();
        }
    }

    private void CloseGame() {
        Destroy(gameObject);
    }

    private void StartGame()
    {
        _cloverSpawner.Spawn();
    }

    public void HandleCloverClick(Clover clover)
    {
        if (_cloverInspector.IsInspecting())
            return;

        _cloverInspector.ShowInspector(clover.isFourLeafClover);
        
        if (clover.isFourLeafClover)
        {
            StartCoroutine(Finish());
        }

        Destroy(clover.gameObject);
    }

     public IEnumerator Finish() {
        yield return new WaitUntil(() => !_cloverInspector.IsInspecting());
        GameManager.SetGameState(GameState.FourLeafCloverFound);
    }
}
