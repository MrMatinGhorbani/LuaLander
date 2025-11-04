
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private TextMeshProUGUI nextButtonTextMesh;
    [SerializeField] private Button nextButton;

    private Action nextButtonOnClickAction;

    private void Awake()
    {
        nextButton.onClick.AddListener(() =>
        {

            nextButtonOnClickAction();

        });
       


    }
    private void Start()
    {
        Lander.instance.OnLanded += Lander_OnLanded;
        Hide();
    }
    private void Update()
    {
        if (GameInput.instance.IsUiActionPressed())
        {
            nextButtonOnClickAction();
        }
    }
    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {

        if (e.landingType == Lander.LandingType.Success)
        {
            titleTextMesh.text = "SUCCESSFUL LANDING!";
            nextButtonTextMesh.text = "NEXT LEVEL";
            nextButtonOnClickAction = GameManager.Instance.GoToNextLevel;

        }
        else
        {
            titleTextMesh.text = "<color=#ff0000>CRASH!</color>";
            nextButtonTextMesh.text = "RETRY";
            nextButtonOnClickAction = GameManager.Instance.RetryLevel;
        }
        statsTextMesh.text =
          Mathf.Round(e.landingSpeed * 2f) + "\n" +
          Mathf.Round(e.dotVector * 100) + "\n" +
            "X" + e.scoreMultiplier + "\n" +
            e.score;
        Show();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
