using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScoreDisplay : MonoBehaviour
{
    [SerializeField] private BaseRoom _baseRoom;
    [SerializeField] private TMP_Text scoreText;
    private int score;

    private void Start()
    {
        score = _baseRoom.Score;
    }

    private void OnEnable()
    {
        _baseRoom.OnScoreChanged += DisplayScore;
    }

    private void OnDisable()
    {
        _baseRoom.OnScoreChanged -= DisplayScore;
    }

    private void DisplayScore()
    {
        if(score < 10)
        {
            scoreText.text = "0" + score;
        }
        else
        {
            scoreText.text = score.ToString();
        }
    }
}
