using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProgressBar : MonoBehaviour
{
    [SerializeField] float maximum;
    [SerializeField] float current;
    [SerializeField] Image mask;

    PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    void Update()
    {
        maximum = player.butterflyMaxTimer;
        current = player.butterflyCurrentTimer;
        GetCurrentFill();
    }
    void GetCurrentFill()
    {
        float fillAmount = current / maximum;
        mask.fillAmount = fillAmount;
    }
}
