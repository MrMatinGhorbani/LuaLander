using System.Collections;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private static int currentLevelNumber = 1;
    [SerializeField] private List<GameLevel> gameLevelsList;
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    private int score;
    private float time;
    private bool isTimerActive;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Lander.instance.OnCoinPickedUp += Lander_OnCoinPickedUp;
        Lander.instance.OnLanded += Lander_OnLanded;
        Lander.instance.OnStateChanged += Lander_OnStateChanged;

        LoadCurrentLevel();
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        isTimerActive = e.stateEvent == Lander.State.Normal;
        if (e.stateEvent == Lander.State.Normal)
        {

            cinemachineCamera.Follow = Lander.instance.transform;
            CinemachineCameraZoom2D.Instance.SetNormalOrthographicSize();
        }
    }

    private void Update()
    {
        if (isTimerActive)
            time += Time.time;
    }
    private void LoadCurrentLevel()
    {
        foreach (GameLevel gameLevel in gameLevelsList)
        {
            if (gameLevel.GetLevelNumber() == currentLevelNumber)
            {
                GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
                Lander.instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
                cinemachineCamera.Follow = spawnedGameLevel.GetCameraStartTargetTransform();
                CinemachineCameraZoom2D.Instance.SetTargetOrthographicSize(spawnedGameLevel.GetzoomedOutOrthographicSize());
            }
        }
    }
    public void GoToNextLevel()
    {
        currentLevelNumber++;
        SceneManager.LoadScene(0);
    }
    public void RetryLevel()
    {
        SceneManager.LoadScene(0);
    }
    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score);
    }

    private void Lander_OnCoinPickedUp(object sender, System.EventArgs e)
    {
        AddScore(500);
    }

    private void AddScore(int addScoreAmount)
    {
        score += addScoreAmount;
        Debug.Log(score);
    }
    public int GetScore()
    {
        return score;
    }
    public float GetTime()
    {
        return time;
    }
    public int GetCurrentLevel()
    {
        return currentLevelNumber;
    }
}
