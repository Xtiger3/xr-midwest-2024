using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private List<char> gameWords = new List<char>();
    public List<GameObject> letterModel = new List<GameObject>();
    public List<Sprite> letterHint = new List<Sprite>();
    private char currentWord;
    private int currentIndex;
    private GameObject currentLetterModel;
    //public GameObject hint;

    public GameObject hintCanvas;
    public Vector3 offset;

    private float timer = 60f;
    public float timeLimit = 60f;
    public TextMeshProUGUI timerText;

    private int countCorrect = 0;

    public GameObject scorePanel;
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI countdownText;

    private bool timerRunning = false;

    public AudioSource audioSource; 

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
        if (timer < 10 && timerRunning)
        {
            audioSource.pitch = 1.5f;
        }
        if (timer <= 0)
        {
            ShowScore();
            audioSource.Stop();
        }
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
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
    }

    public void NextRound()
    {
        StopAllCoroutines();
        //hintCanvas.SetActive(false);
        currentIndex = Random.Range(0, gameWords.Count);
        currentLetterModel = Instantiate(letterModel[currentIndex], Camera.main.transform.position + offset, Quaternion.identity);
        currentLetterModel.transform.localScale = new Vector3(-0.75f, 0.75f, 0.75f);
        currentLetterModel.transform.position += new Vector3(0, -.5f, 0);
        currentWord = gameWords[currentIndex];
        StartCoroutine(ShowHint());
    }

    public IEnumerator ShowHint()
    {
        yield return new WaitForSeconds(5f);
        GameObject hintObject = Instantiate(hintCanvas, currentLetterModel.transform);
        hintObject.transform.GetChild(0).GetComponent<Image>().sprite = letterHint[currentIndex];
        hintObject.transform.position += new Vector3(.6f, -.25f, 0);
    }

    public void ShowScore()
    {
        scorePanel.SetActive(true);
        int score = (countCorrect * 10);
        scoreText.text = "You scored " + score.ToString() + " points";
    }

   
}
