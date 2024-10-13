using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private List<char> gameWords = new List<char>();
    public List<GameObject> letterModel = new List<GameObject>();
    private char currentWord;
    private int currentIndex;
    private GameObject currentLetterModel;
    public GameObject hint;
    public Vector3 offset;

    private float timer = 60f;
    public float timeLimit = 60f;
    public TextMeshProUGUI timerText;

    private int countCorrect = 0;

    public GameObject scorePanel;
    public TextMeshProUGUI scoreText;
    //public TextMeshProUGUI correctCountText;

    public TextMeshProUGUI countdownText;
    //public TextMeshProUGUI gameScoreText;
    //public GameObject instructions;

    private bool timerRunning = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        for (char letter = 'A'; letter <= 'Y'; letter++)
        {
            if (letter == 'J') continue;
            gameWords.Add(letter);
        }
    }

    private void Update()
    {
        if (timer > 0 && timerRunning)
        {
            timerText.text = Mathf.FloorToInt(timer).ToString();
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            ShowScore();
        }
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        //instructions.SetActive(false);
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        //yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(true);
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.text = "START";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
        //instructions.SetActive(false);

        timer = timeLimit;
        timerRunning = true;
        NextRound();
    }

    public void GestureRecognized(string gestureName)
    {
        if (gestureName == currentWord.ToString())
        {
            AddCountCorrect();
            Destroy(currentLetterModel);
            NextRound();
        }

    }

    public void AddCountCorrect()
    {
        countCorrect++;
        //gameScoreText.text = countCorrect.ToString();
    }

    public void NextRound()
    {
        StopAllCoroutines();
        hint.SetActive(false);
        currentIndex = Random.Range(0, gameWords.Count);
        currentLetterModel = Instantiate(letterModel[currentIndex], Camera.main.transform.position + offset, Quaternion.identity);
        currentLetterModel.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        currentWord = gameWords[currentIndex];
        StartCoroutine(ShowHint());
    }

    public IEnumerator ShowHint()
    {
        yield return new WaitForSeconds(5f);
        hint.SetActive(true);
    }

    public void ReplayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowScore()
    {
        scorePanel.SetActive(true);
        int score = (countCorrect * 10);
        scoreText.text = "You scored " + score.ToString() + " points";
        //correctCountText.text = "CORRECT: " + countCorrect.ToString();
    }

   
}
