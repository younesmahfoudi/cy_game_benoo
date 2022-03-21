using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject fadeGO;
    public AudioClip MenuMusicAC;
    private bool _isGameStarting = false;
    public GameObject scoreBoard;
    public GameObject MansionLevelContent;
    public GameObject CemeteryLevelContent;
    public GameObject UndergroundCityLevelContent;
    public GameObject ScoreLinePrefab;

    void Start()
    {
        SoundManager.Instance.PlayMusic(MenuMusicAC);
        fadeGO.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (_isGameStarting && fadeGO.GetComponent<GDTFadeEffect>().HasFinished())
            SceneManager.LoadScene(LevelNames.MansionLevel);
    }

    public void StartGame()
    {
        fadeGO.GetComponent<GDTFadeEffect>().firstToLast = false;
        fadeGO.GetComponent<GDTFadeEffect>().disableDelay = 1;
        fadeGO.SetActive(true);
        _isGameStarting = true;
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene(LevelNames.settingsMenu, LoadSceneMode.Additive);
    }

    public void OpenScoreBoard()
    {
        scoreBoard.SetActive(true);
        ResetBoards();
        UpdateBoard(MansionLevelContent, LevelNames.MansionLevel);
        UpdateBoard(CemeteryLevelContent, LevelNames.CemeteryLevel);
        UpdateBoard(UndergroundCityLevelContent, LevelNames.UnderGroundCityLevel);
    }

    public void ResetScores()
    {
        GameSaveInstance.Instance.ResetScores();
    }

    public void ResetBoards()
    {
        foreach (Transform line in MansionLevelContent.transform)
            Destroy(line.gameObject);
        foreach (Transform line in UndergroundCityLevelContent.transform)
            Destroy(line.gameObject);
        foreach (Transform line in CemeteryLevelContent.transform)
            Destroy(line.gameObject);
    }

    private void UpdateBoard(GameObject board, string level)
    {
        List<int> scores = GameSaveInstance.Instance.scoresLevel1;

        if (level == LevelNames.MansionLevel)
            scores = GameSaveInstance.Instance.scoresLevel1;
        else if (level == LevelNames.CemeteryLevel)
            scores = GameSaveInstance.Instance.scoresLevel2;
        else if (level == LevelNames.UnderGroundCityLevel)
            scores = GameSaveInstance.Instance.scoresLevel3;

        int index = 1;
        scores.ForEach(element =>
        {
            GameObject obj = Instantiate(ScoreLinePrefab, board.transform);
            obj.GetComponent<ScoreLineScript>().SetTexts(index.ToString() + ".", element.ToString());
            Vector2 pos = obj.GetComponent<RectTransform>().anchoredPosition;
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x, pos.y - (29 * (index - 1)));


            index++;
        });
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
