using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance { get; private set; }

    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI wavesSurvivedText;

    private void Awake()
    {
        Instance = this;

        retryButton.onClick.AddListener(() => {
            GameSceneManager.Load(GameSceneManager.Scene.GameScene);
        });

        mainMenuButton.onClick.AddListener(() => {
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        wavesSurvivedText.SetText($"You Survived {EnemyWaveManager.Instance.GetWaveNumber()} Waves!");
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
