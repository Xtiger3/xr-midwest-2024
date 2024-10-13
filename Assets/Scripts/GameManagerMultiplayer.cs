using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManagerMultiplayer : MonoBehaviour
{
    public static GameManagerMultiplayer Instance { get; private set; }
    public ServerCommunicator server;

    private List<char> gameWords = new List<char>();
    public List<GameObject> letterModel = new List<GameObject>();
    public List<Sprite> letterHint = new List<Sprite>();
    private char currentWord;
    private int currentIndex;
    private GameObject currentLetterModel;

    public Image hintCanvas;
    public Vector3 offset;

    private float timer = 65f;
    private bool timerRunning = false;
    private bool gameStarted = false;

    public AudioSource audioSource;

    public int health;

    // UI elements
    public GameObject sartPanel;
    public GameObject waitPanel;
    public GameObject gamePanel;
    public GameObject endPanel;
    public GameObject timerGameObject;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;


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
        //if (timer > 0 && timerRunning)
        //{
        //    timerText.text = Mathf.FloorToInt(timer).ToString();
        //    timer -= Time.deltaTime;
        //}
        if (health < 20 && timerRunning)
        {
            audioSource.pitch = 1.5f;
        }
        if (timer <= 0)
        {
            //ShowScore();
            audioSource.Stop();
        }

        //currentLetterModel.transform.position = Camera.main.transform.position + offset;
    }

    private void Start()
    {
        //StartGame();
    }

    public void StartGame()
    {
        //StartCoroutine(Init());
    }

    public void GestureRecognized(string gestureName)
    {
        if (gestureName == currentWord.ToString())
        {
            server.SendCorrectGesture();
            Destroy(currentLetterModel);
            NextRound();
        }

    }

    public void UpdateTimer(int timeLeft)
    {
        if (timeLeft > 60)
        {
            countdownText.gameObject.SetActive(true);
            timerGameObject.SetActive(false);
            gamePanel.SetActive(true);
            if (timeLeft > 61)
            {
                countdownText.text = (timeLeft - 61).ToString();
            }
            else
            {
                countdownText.text = "START";
            }
        }
        else
        {
            if (!gameStarted)
            {
                gameStarted = true;
                NextRound();
            }
            countdownText.gameObject.SetActive(false);
            timerGameObject.SetActive(true);
            timerText.text = timeLeft.ToString();
            timer = timeLeft;
        }
    }

    public void NextRound()
    {
        StopAllCoroutines();
        //hintCanvas.SetActive(false);
        hintCanvas.gameObject.SetActive(false);
        currentIndex = Random.Range(0, gameWords.Count);
        currentLetterModel = Instantiate(letterModel[currentIndex], Camera.main.transform.position + offset, Quaternion.identity);
        currentLetterModel.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        currentWord = gameWords[currentIndex];
        StartCoroutine(ShowHint());
    }

    public IEnumerator ShowHint()
    {
        yield return new WaitForSeconds(5f);
        //GameObject hintObject = Instantiate(hintCanvas, currentLetterModel.transform);
        hintCanvas.gameObject.SetActive(true);
        hintCanvas.sprite = letterHint[currentIndex];
        //hintObject.transform.position += new Vector3(.6f, -.25f, 0);
    }

    public void ShowScore(string res)
    {
        timerRunning = false;
        endPanel.SetActive(true);
        scoreText.text = res;
    }
}
