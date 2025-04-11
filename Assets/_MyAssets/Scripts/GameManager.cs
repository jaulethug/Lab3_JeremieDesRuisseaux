using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int _score;
    private float _timeStart;
    private float[] _timePerLevel;
    private int[] _collisionsPerLevel;
    private bool _firstMovelInLevel = false;

    // Définition Singleton
    public static GameManager _instance;
    public int Score => _score; // accesseur attribut _score
    public float TimeStart => _timeStart; // accesseur attribut _timeStart
    private float[] _timeStarts;

    private void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        _score = 0;
        _firstMovelInLevel = false;
        _timePerLevel = new float[SceneManager.sceneCountInBuildSettings];
        _collisionsPerLevel = new int[SceneManager.sceneCountInBuildSettings];
        _timeStarts = new float[SceneManager.sceneCountInBuildSettings];
    }

    public void StartTime()
    {
        if (!_firstMovelInLevel)
        {
            _timeStart = Time.time;
            _timeStarts[SceneManager.GetActiveScene().buildIndex] = _timeStart;
            _firstMovelInLevel = true;
        }
    }

    public void setLevelTimeAndCollision(float endTime, int levelIndex)
    {
        _firstMovelInLevel = false;
        int collisionTotalYet = 0;

        foreach (var item in _collisionsPerLevel)
        {
            collisionTotalYet += item;
        }

        if (levelIndex != 0)
        {
            _timePerLevel[levelIndex] = endTime - (_timeStart + _timePerLevel[levelIndex]);
        }
        else
        {
            _timePerLevel[levelIndex] = endTime - _timeStart;
        }
        
        _collisionsPerLevel[levelIndex] = _score - collisionTotalYet;
    }

    public void AddScore()
    {
        _score++;
        Debug.Log("start : " + _timeStart);
        Debug.Log("temps : " + Time.time);
    }

    public void DisplayEndGameMessages()
    {
        Debug.Log("******* Fin de partie *******");
        var index = 0;
        float totalTime = 0;
        foreach (var time in _timePerLevel)
        {
            print("Pour le niveau " + (index + 1) + " : ");
            print("Temps de départ" + _timeStarts[index].ToString("f2"));
            print("Votre temps est de " + time.ToString("f2") + " secondes.");
            print("Vous avez eu " + _collisionsPerLevel[index].ToString("f2") + " collisions.");

            print("*****************************************************************************");
            totalTime += time;
            index++;
        }

        print("Temps total sans les collisions = " + totalTime);
        print("Temps total avec la pénalité des collisions = " + (totalTime + _score));
        
    }
}
