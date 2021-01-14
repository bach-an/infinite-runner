using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Height : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Text heightText;



    private void Update()
    {
        float curScore = float.Parse(heightText.text);
        float playerPos = player.position.y - 1;

        heightText.text = playerPos.ToString("0");

    }

}

