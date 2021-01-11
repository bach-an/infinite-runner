using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlatformManager: MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private Transform platformPrefab;
    [SerializeField] private Transform startingPlatform;
    
    // determine platform distance
    [SerializeField] private float distanceMean = 14f;
    [SerializeField] private float distanceStd = 1f;

    // determine scale size of platforms
    [SerializeField] private float scaleMean = 5f;
    [SerializeField] private float scaleStd = 2f;

    // determine height of platforms
    [SerializeField] private float heightMean = 0f;
    [SerializeField] private float heightStd = 0.5f;

    // how often the player will get a moving platform
    [SerializeField, Range(0f, 0.9999f)] private float movingPlatformSpawnRate = 0.1f;

    // the range of height that the platforms will move:
    // [-movingPlatformRange, movingPlatformRange]
    [SerializeField, Range(0f, 20f)] private float movingPlatformRange = 20f;

    // how fast the platforms are moving
    [SerializeField] private float movingPlatformSpeed = 5f;

    // for comparing floats
    private float floatTolerance = 0.01f;

    private readonly System.Random rng = new System.Random();
    private PlayerMovement playerMovement;

    // an array list of platforms and an index to keep track of which one player
    // is on
    private List<Platform> platforms = new List<Platform>();

    private void Awake()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        // starting platform doesn't move
        Platform start = new Platform(startingPlatform, false, Direction.Still);
        platforms.Add(start);
        GeneratePlatform();
    }

    private void Update()
    {
        int playerIdx = getPlayersPlatform();
        List<Platform> movingPlatforms = platforms.FindAll(e => e.IsPlatformMoving());
        if (playerIdx == platforms.Count - 1)
        {
            GeneratePlatform();
        }
        movingPlatforms.ForEach(e => Move(e));

    }

    // move the corresponding platform
    private void Move(Platform platform)
    {
        Vector3 location = platform.GetTransform().position;
        Direction dir = platform.GetDir();

        // if the platform has reached the max or min height
        // change the direction
        if (Mathf.Abs(location.y - movingPlatformRange) <= floatTolerance ||
            Mathf.Abs(location.y + movingPlatformRange) <= floatTolerance)
        {
            if(dir == Direction.Down)
            {
                dir = Direction.Up;
            }
            else if(dir == Direction.Up)
            {
                dir = Direction.Down;
            }
        }

        switch (dir)
        {
            case Direction.Up:
                location.y += movingPlatformSpeed * Time.deltaTime;
                break;
            case Direction.Down:
                location.y -= movingPlatformSpeed * Time.deltaTime;
                break;
            default:
                break;
        }
        platform.SetDir(dir);
        platform.GetTransform().position = location;

        
    }

    private void GeneratePlatform()
    {
        // is the platform going to move?
        bool isMoving = movingPlatformSpawnRate >= rng.NextDouble();
        Direction[] movingDirections = new[] { Direction.Down, Direction.Up };

        // if the platform is not moving, set dir to still, else pick 
        // direction randomly
        Direction initDir = !isMoving ? Direction.Still :
            movingDirections[rng.Next(movingDirections.Length)];

        // get the transform of the last platform in the list
        Transform lastPlatform = platforms[platforms.Count - 1].GetTransform();

        float randZ = SampleGaussian(rng, distanceMean, distanceStd);
        float randScale = SampleGaussian(rng, scaleMean, scaleStd);
        float randY = SampleGaussian(rng, heightMean, heightStd);

        Transform platformTransform = Instantiate(platformPrefab);
        
        Vector3 position = platformTransform.localPosition;
        Vector3 scale = platformTransform.localScale;
        position.z = randZ + platformTransform.localPosition.z +
            platformTransform.localScale.z / 2 + lastPlatform.localPosition.z;
        position.y = randY;
        scale.z = randScale;
        platformTransform.localPosition = position;
        platformTransform.localScale = scale;

        Platform platform = new Platform(platformTransform, isMoving, initDir);
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
            List<Transform> platformsTransforms = GetPlatformTransforms(platforms);

            playerIdx = platformsTransforms.IndexOf(collisions[0].transform, 0);
            return playerIdx;
        }
        return -1;
    }

    private List<Transform> GetPlatformTransforms(List<Platform> platforms)
    {
        List<Transform> platformTransforms = new List<Transform>();
        foreach (Platform p in platforms)
        {
            platformTransforms.Add(p.GetTransform());
        }
        return platformTransforms;
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


    // class to represent a platform and whether or not it's
    // moving
    private class Platform
    {
        // the gameobject's transform
        private Transform platform;

        // whether or not the platform is moving
        private bool isMoving;

        // which way the platform is moving
        private Direction dir;

        public Platform(Transform platform, bool isMoving, Direction initDir)
        {
            this.platform = platform;
            this.isMoving = isMoving;
            this.dir = initDir;
        }

        // getter for the gameobject
        public Transform GetTransform()
        {
            return this.platform;
        }

        // getter for movement
        public bool IsPlatformMoving()
        {
            return this.isMoving;
        }

        // getter for movement direction
        public Direction GetDir()
        {
            return this.dir;
        }

        // set which way the platform is moving
        public void SetDir(Direction newDir)
        {
            this.dir = newDir;
        }
    }

    // represents which direction the platform is moving
    // (if at all)
    enum Direction
    {
        Up = 1,
        Still = 0,
        Down = -1
    }


}

