using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private MusicManager musicManager;

    [SerializeField] private Button soundDecreaseButton;
    [SerializeField] private Button soundIncreaseButton;
    [SerializeField] private Button musicDecreaseButton;
    [SerializeField] private Button musicIncreaseButton;
    [SerializeField] private Button mainMenuButton;

    [SerializeField] private TextMeshProUGUI soundVolumeText;
    [SerializeField] private TextMeshProUGUI musicVolumeText;

    private void Awake()
    {
        soundDecreaseButton.onClick.AddListener(() =>
        {
            soundManager.DecreaseVolume();
            UpdateText();
        });

        soundIncreaseButton.onClick.AddListener(() =>
        {
            soundManager.IncreaseVolume();
            UpdateText();
        });

        musicDecreaseButton.onClick.AddListener(() =>
        {
            musicManager.DecreaseVolume();
            UpdateText();
        });

        musicIncreaseButton.onClick.AddListener(() =>
        {
            musicManager.IncreaseVolume();
            UpdateText();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });
    }

    private void Start()
    {
        UpdateText();
        gameObject.SetActive(false);
    }

    private void UpdateText() 
    {
        soundVolumeText.SetText(Mathf.RoundToInt(soundManager.GetVolume() * 10).ToString());
        musicVolumeText.SetText(Mathf.RoundToInt(musicManager.GetVolume() * 10).ToString());
    }

    public void ToggleVisible()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
