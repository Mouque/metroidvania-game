using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public int score;
    public TMP_Text scoreText;
    public static GameController instance;

    // Start is called before the first frame update    
    void Awake()
    {
        instance = this;
    }

    public void GetCoin()
    {
        score++;
        scoreText.text = "x" + score.ToString();
    }

}
