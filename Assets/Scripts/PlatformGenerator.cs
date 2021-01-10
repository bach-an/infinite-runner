using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlatformGenerator : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private Transform platformPrefab;
    [SerializeField] private Transform startingPlatform;
    
    // determine platform distance
    [SerializeField] private float distanceMean = 14;
    [SerializeField] private float distanceStd = 1;

    // determine scale size of platforms
    [SerializeField] private float scaleMean = 5;
    [SerializeField] private float scaleStd = 2;

    // determine height of platforms
    [SerializeField] private float heightMean = 0;
    [SerializeField] private float heightStd = 2;

    private readonly System.Random rng = new System.Random();
    private PlayerMovement playerMovement;

    // an array list of platforms and an index to keep track of which one player
    // is on
    private List<Transform> platforms = new List<Transform>();

    private void Awake()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        platforms.Add(startingPlatform);

        // Make function for this //

        generatePlatform();
    }

    private void Update()
    {
        int playerIdx = getPlayersPlatform();
        if (playerIdx == platforms.Count - 1)
        {
            generatePlatform();
        }
    }

    private void generatePlatform()
    {
        Transform lastPlatform = platforms[platforms.Count - 1];

        float randZ = SampleGaussian(rng, distanceMean, distanceStd);
        float randScale = SampleGaussian(rng, scaleMean, scaleStd);
        float randY = SampleGaussian(rng, heightMean, heightStd);

        Transform platform = Instantiate(platformPrefab);
        Vector3 position = platform.localPosition;
        Vector3 scale = platform.localScale;
        position.z = randZ + platform.localPosition.z + 
            platform.localScale.z / 2 + lastPlatform.localPosition.z;
        position.y = randY;
        scale.z = randScale;
        platform.localPosition = position;
        platform.localScale = scale;
        platforms.Add(platform);
    }

    // return the index of the corresponding platform that the player is on
    // can only return one int
    private int getPlayersPlatform()
    {
        Collider[] collisions = playerMovement.PlayersPlatform();
        int playerIdx;

        // if the player is not in the air
        if(collisions.Length == 1)
        {
            playerIdx = platforms.IndexOf(collisions[0].transform, 0);
            return playerIdx;
        }

        return -1;
    }

    // https://gist.github.com/tansey/1444070
    private float SampleGaussian(System.Random random, float mean, float std)
    {
        // The method requires sampling from a uniform random of (0,1]
        // but Random.NextDouble() returns a sample of [0,1).
        float x1 = 1 - (float)random.NextDouble();
        float x2 = 1 - (float)random.NextDouble();

        float y1 = Mathf.Sqrt((float)(-2.0 * Mathf.Log(x1))) 
            * Mathf.Cos((float)(2.0 * (float)Mathf.PI * x2));
        return y1 * std + mean;
    }

}
