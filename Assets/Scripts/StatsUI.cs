using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject speedUpArrowGameObject;
    [SerializeField] private GameObject speedDownArrowGameObject;
    [SerializeField] private GameObject speedRightArrowGameObject;
    [SerializeField] private GameObject speedLeftArrowGameObject;
    [SerializeField] private Image fuelBarImage;
    private void Update()
    {
        UpdatStatsTextMesh();
    }
    private void UpdatStatsTextMesh()
    {
        speedUpArrowGameObject.SetActive(Lander.instance.GetSpeedY() >= 0f);
        speedDownArrowGameObject.SetActive(Lander.instance.GetSpeedY() < 0f);
        speedRightArrowGameObject.SetActive(Lander.instance.GetSpeedX() >= 0f);
        speedLeftArrowGameObject.SetActive(Lander.instance.GetSpeedX() < 0f);

        statsTextMesh.text =
            GameManager.Instance.GetCurrentLevel() + "\n" +
            GameManager.Instance.GetScore() + "\n" +
                           Mathf.Round(GameManager.Instance.GetTime()) + "\n" +
                        Mathf.Abs(Mathf.Round(Lander.instance.GetSpeedX() * 10)) + "\n" +
                        Mathf.Abs(Mathf.Round(Lander.instance.GetSpeedY() * 10));

        fuelBarImage.fillAmount = Lander.instance.GetNormalizedFuel();
    }
}
