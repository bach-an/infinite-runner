using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Text scoreText;



    private void Update()
    {
        float curScore = float.Parse(scoreText.text);
        float playerPos = player.position.z;

        if (curScore < playerPos)
        {
            scoreText.text = playerPos.ToString("0");
        }
    }

}

