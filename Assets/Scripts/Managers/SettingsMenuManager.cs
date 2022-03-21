using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    public GameObject easyContour;
    public GameObject normalContour;
    public Text musicText;
    public Text effectsText;
    public AudioClip effectsTest;

    private void Start()
    {
        switch (GameSaveInstance.Instance.currentDifficultyEnum)
        {
            case GameSaveInstance.Difficulty.EASY:
                easyContour.SetActive(true);
                break;
            case GameSaveInstance.Difficulty.NORMAL:
                normalContour.SetActive(true);
                break;
        }
        musicText.text = SoundManager.Instance.getMusicSoundPercent().ToString();
        effectsText.text = SoundManager.Instance.getEffectsSoundPercent().ToString();
    }

    public void UpdateMusic(float changeNbr)
    {
        SoundManager.Instance.UpdateMusicVolume(changeNbr);
        musicText.text = SoundManager.Instance.getMusicSoundPercent().ToString();
    }

    public void UpdateEffects(float changeNbr)
    {
        SoundManager.Instance.UpdateEffectsVolume(changeNbr);
        effectsText.text = SoundManager.Instance.getEffectsSoundPercent().ToString();
        SoundManager.Instance.Play(effectsTest, SoundManager.SoundPriority.IMPORTANT);
    }

    public void SetEasyDifficulty()
    {
        ResetContours();
        GameSaveInstance.Instance.ChangeDifficulty(GameSaveInstance.Difficulty.EASY);
        easyContour.SetActive(true);
    }

    public void SetNormalDifficulty()
    {
        ResetContours();
        GameSaveInstance.Instance.ChangeDifficulty(GameSaveInstance.Difficulty.NORMAL);
        normalContour.SetActive(true);
    }

    private void ResetContours()
    {
        easyContour.SetActive(false);
        normalContour.SetActive(false);
    }

    public void CloseScene()
    {
        SceneManager.UnloadSceneAsync(LevelNames.settingsMenu);
    }
}
