using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Enums containing the different difficulties of the game
    enum Difficulty {EASY, MEDIUM, HARD, NONE};

    //Text Fields for dialogue and displayed texts
    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI DisplayedWord;
    [SerializeField] private TextMeshProUGUI LevelDisplay;
    [SerializeField] private TextMeshProUGUI LivesDisplay;

    //Different buttons in the difficulty selection screen
    [Header("Difficulty Buttons")]
    [SerializeField] private Button m_EasyButton;
    [SerializeField] private Button m_MediumButton;
    [SerializeField] private Button m_HardButton;

    //Seen and new button
    [Header("Seen and New Buttons")]
    [SerializeField] private Button m_SeenButton;
    [SerializeField] private Button m_NewButton;
 
    [Header("Other Variables")]
    [SerializeField] private float m_timePerWord; //stores the duration of the word display
    
    //List of words that would be used for the memory game
    private string[] words = 
    {
        "Apple", "Ball", "Cat", "Dog", "Elephant",
        "Fish", "Giraffe", "Horse", "Ice Cream", "Jacket",
        "Kite", "Lemon", "Moon", "Nest", "Owl", "Pizza",
        "Queen", "Rainbow", "Sun", "Tree", "Umbrella",
        "Volcano", "Watermelon", "Xylophone", "Yellow", "Zebra",
        "Ant", "Butterfly", "Caterpillar", "Dolphin", "Eagle",
        "Flower", "Guitar", "House", "Igloo", "Jellyfish",
        "Kangaroo", "Lion", "Monkey", "Night", "Ocean",
        "Penguin", "Queen", "Rose", "Star", "Turtle",
        "Unicorn", "Volleyball", "Waterfall", "X-ray", "Yacht",
        "Zoo", "Airplane", "Bear", "Car", "Duck",
        "Egg", "Fox", "Gorilla", "Helicopter", "Insect",
        "Jaguar", "Kite", "Lighthouse", "Mouse", "Nurse",
        "Octopus", "Pony", "Quilt", "Rabbit", "Snail",
        "Train", "UFO", "Vase", "Whale", "Xmas",
        "Yak", "Zipper"
    };

    private int m_currentLevel; //stores the current level of the game
    private int m_remainingLives; //stores the remaining lives 0f the player
    private int m_currentWordIndex; //stores the current index in the word list
    private float m_startTime;
    private bool m_hasTimeStarted;
    private string m_newWord;
    private Difficulty m_currentDifficulty = Difficulty.NONE;

    private GameObject diffSelection;
    private Canvas gameDisplay;

    private bool m_hasGameStarted;
    private bool m_hasPlayerPicked;
    private List<string> wordList;

    private void Start()
    {

        diffSelection = GameObject.Find("Difficulty Selection Screen");
        gameDisplay = GameObject.Find("Game Display").GetComponent<Canvas>();
        gameDisplay.enabled = false;
        wordList = new List<string>();

        m_remainingLives = 0;
        m_currentLevel = 0;
        m_EasyButton.onClick.AddListener(()=>{
            m_currentDifficulty = Difficulty.EASY;
            startGame();
        });
        m_MediumButton.onClick.AddListener(()=>{
            m_currentDifficulty = Difficulty.MEDIUM;
            startGame();
        });
        m_HardButton.onClick.AddListener(()=>{
            m_currentDifficulty = Difficulty.HARD;
            startGame();
        });

        m_SeenButton.onClick.AddListener(()=>{
            if (wordList.Contains(m_newWord)){
                m_currentLevel++;
                m_hasPlayerPicked = true;
            }else{
                wordList.Add(m_newWord);
                m_remainingLives--;
                m_hasPlayerPicked = true;
            }
        });
        m_NewButton.onClick.AddListener(()=>{
            if (!wordList.Contains(m_newWord)){
                wordList.Add(m_newWord);
                m_currentLevel++;
                m_hasPlayerPicked = true;
            }else{
                m_remainingLives--;
                m_hasPlayerPicked = true;
            }
        });
    }

    private void Update() {
        LevelDisplay.text = "Level: " + m_currentLevel;
        LivesDisplay.text = "Lives: " + m_remainingLives;
        if (m_hasGameStarted && m_remainingLives > 0){
            if (m_currentWordIndex < 3){
                DisplayedWord.text = wordList[m_currentWordIndex];
                var timeElapsed = Time.time - m_startTime;
                if (timeElapsed >= m_timePerWord){
                    m_currentWordIndex++;
                    m_startTime = Time.time;
                }
            } else{
                if(m_hasPlayerPicked){
                    var randomizer = Random.Range(0,2);
                    m_newWord = randomizer == 0? words[Random.Range(0,words.Length)]: wordList[Random.Range(0,m_currentWordIndex)];
                    DisplayedWord.text = m_newWord;
                    Debug.Log(m_newWord);
                    m_hasPlayerPicked=false;
                } else {

                }
                m_SeenButton.gameObject.SetActive(true);
                m_NewButton.gameObject.SetActive(true);
                // if (!wordList.Contains(newWord)) wordList.Add(newWord);
            }
        }
        else {
            DisplayedWord.text = "Game Over";
        }
    }
    
    private void startGame(){
        diffSelection.SetActive(false);
        gameDisplay.enabled = true;
        var currD = m_currentDifficulty;
        m_remainingLives = currD == Difficulty.EASY? 5: currD == Difficulty.MEDIUM ? 3: 1;
        m_currentLevel = 1;
        m_currentWordIndex = 0;

        
        for (int i = 0; i < 3; i++){
            var newWord = words[Random.Range(0,words.Length)];
            if (!wordList.Contains(newWord)) wordList.Add(newWord);
            Debug.Log(newWord);
        }
        m_hasGameStarted = true;
        m_startTime = Time.time;
        m_hasTimeStarted = true;
        m_hasPlayerPicked = true;
    }
}