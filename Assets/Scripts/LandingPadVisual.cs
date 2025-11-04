using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LandingPadVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreMultiplierTextMeshPro;
    private void Awake()
    {
        LandingPad landingPad = GetComponent<LandingPad>();
        scoreMultiplierTextMeshPro.text="X"+landingPad.GetScoreMultiplier();
    }
}
