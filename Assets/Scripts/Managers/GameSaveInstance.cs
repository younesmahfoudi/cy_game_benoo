using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveInstance : MonoBehaviour
{
	public enum Difficulty { EASY, NORMAL };

	public struct DifficultySettings {
		public float spawnerMinFrequency;
		public float spawnerMaxFrequency;
		public float zombiesMinSpeed;
		public float zombiesMaxSpeed;
		public int gemDropRating;

		public DifficultySettings Init(float spawnMinFreq, float spawnMaxFreq, float zombMinSpeed, float zombMaxSpeed, int gemDropRate)
        {
			spawnerMinFrequency = spawnMinFreq;
			spawnerMaxFrequency = spawnMaxFreq;
			zombiesMinSpeed = zombMinSpeed;
			zombiesMaxSpeed = zombMaxSpeed;
			gemDropRating = gemDropRate;

			return this;
		}
	}

	private DifficultySettings _normalDifficulty = new DifficultySettings().Init(0.3f, 0.9f, 3, 6, 10);
	private DifficultySettings _easyDifficulty = new DifficultySettings().Init(0.3f, 0.5f, 1, 4, 5);

	public int gemCount;
	public List<int> scoresLevel1;
	public List<int> scoresLevel2;
	public List<int> scoresLevel3;
	public DifficultySettings currentDifficultySettings;
	public Difficulty currentDifficultyEnum;

	// Singleton instance.
	public static GameSaveInstance Instance = null;

	// Initialize the singleton instance.
	private void Awake()
	{
		if (Instance == null)
		{
			//fetch data saved between launches of game
			currentDifficultySettings = _normalDifficulty;
			currentDifficultyEnum = Difficulty.NORMAL;
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	public void ChangeDifficulty(Difficulty dif)
    {
		switch (dif)
        {
			case Difficulty.EASY:
				currentDifficultyEnum = Difficulty.EASY;
				currentDifficultySettings = _easyDifficulty;
				break;
			case Difficulty.NORMAL:
				currentDifficultyEnum = Difficulty.NORMAL;
				currentDifficultySettings = _normalDifficulty;
				break;
		}
    }

	public void ResetScores()
    {
		scoresLevel1.Clear();
		scoresLevel2.Clear();
		scoresLevel3.Clear();
    }

	static int SortByScore(int score1, int score2)
    {
		return score2.CompareTo(score1);
    }

	public void AddScore(int score, string level)
    {
		if (level == LevelNames.MansionLevel)
		{
			scoresLevel1.Add(score);
			scoresLevel1.Sort(SortByScore);
		}
		if (level == LevelNames.CemeteryLevel)
		{
			scoresLevel2.Add(score);
			scoresLevel2.Sort(SortByScore);
		}
		if (level == LevelNames.UnderGroundCityLevel)
		{
			scoresLevel3.Add(score);
			scoresLevel3.Sort(SortByScore);
		}
	}
}
