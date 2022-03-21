using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLineScript : MonoBehaviour
{
    public Text positionText;
    public Text scoreText;
    public void SetTexts(string position, string score)
    {
        positionText.text = position;
        scoreText.text = score;
    }
}
