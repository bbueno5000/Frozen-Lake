using MLAgents;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class EnvironmentParameters
{
    public List<string> action_descriptions { get; set; }
    public int action_size { get; set; }
    public string action_space_type { get; set; }
    public string env_name { get; set; }
    public int num_agents { get; set; }
    public int observation_size { get; set; }
    public int state_size { get; set; }
    public string state_space_type { get; set; }
}

public class FrozenLakeAcademy : Academy
{
    public List<GameObject> actorObjs;
    public FrozenLakeDecision decision;
    public EnvironmentParameters envParameters;
    private int gridSize;
    private int numGoals;
    private int numObstacles;
    private int[] objectPositions;
    public string[] players;

    /// <summary>
    /// Specifies the academy behavior when being reset
    /// (i.e. at the completion of a global episode).
    /// </summary>
    ///
    public override void AcademyReset()
    {
        foreach (GameObject actor in actorObjs)
            DestroyImmediate(actor);

        actorObjs = new List<GameObject>();

        for (int i = 0; i < players.Length; i++)
        {
            int x = (objectPositions[i]) / gridSize;
            int y = (objectPositions[i]) % gridSize;

            GameObject actorObj = (GameObject) GameObject.Instantiate(Resources.Load(players[i]));

            actorObj.transform.position = new Vector3(x, 0.0f, y);
            actorObj.name = players[i];
            actorObjs.Add(actorObj);
        }
    }

    /// <summary>
    /// Specifies the academy behavior at every step of the environment.
    /// </summary>
    ///
    public override void AcademyStep()
    {

    }

    /// <summary>
    /// Restarts the learning process with a new Grid.
    /// </summary>
    ///
    public void BeginNewGame()
    {
        int gridSizeSet = (GameObject.Find("Dropdown").GetComponent<Dropdown>().value + 1) * 5;
        numGoals = 1;

        numObstacles = Mathf.FloorToInt((gridSizeSet * gridSizeSet) / 10f);
        gridSize = gridSizeSet;

        foreach (GameObject actor in actorObjs)
            DestroyImmediate(actor);

        SetUp();
        decision = new FrozenLakeDecision();
        decision.SendParameters(envParameters);
        AcademyReset();
    }

    /// <summary>
    /// Gets the agent's current position and transforms it into a discrete integer state.
    /// </summary>
    ///
    public List<float> CollectState()
    {
        List<float> state = new List<float>();

        foreach (GameObject actor in actorObjs)
        {
            if (actor.tag == "agent")
            {
                float point = (gridSize * actor.transform.position.x) + actor.transform.position.z;
                state.Add(point);
            }
        }

        return state;
    }

    /// <summary>
    /// Initializes the academy and environment.
    /// Called during the waking-up phase of the environment
    /// before any of the scene objects/agents have
    /// been initialized.
    /// </summary>
    ///
    public override void InitializeAcademy()
    {
        BeginNewGame();
    }

    /// <summary>
    /// Resizes the grid to the specified size.
    /// </summary>
    ///
    public void SetEnvironment()
    {
        GameObject.Find("Plane").transform.localScale = new Vector3(gridSize / 10.0f, 1f, gridSize / 10.0f);
        GameObject.Find("Plane").transform.position = new Vector3((gridSize - 1) / 2f, -0.5f, (gridSize - 1) / 2f);
        GameObject.Find("sN").transform.localScale = new Vector3(1, 1, gridSize + 2);
        GameObject.Find("sS").transform.localScale = new Vector3(1, 1, gridSize + 2);
        GameObject.Find("sN").transform.position = new Vector3((gridSize - 1) / 2f, 0.0f, gridSize);
        GameObject.Find("sS").transform.position = new Vector3((gridSize - 1) / 2f, 0.0f, -1);
        GameObject.Find("sE").transform.localScale = new Vector3(1, 1, gridSize + 2);
        GameObject.Find("sW").transform.localScale = new Vector3(1, 1, gridSize + 2);
        GameObject.Find("sE").transform.position = new Vector3(gridSize, 0.0f, (gridSize - 1) / 2f);
        GameObject.Find("sW").transform.position = new Vector3(-1, 0.0f, (gridSize - 1) / 2f);

        HashSet<int> numbers = new HashSet<int>();

        while (numbers.Count < players.Length)
            numbers.Add(Random.Range(0, gridSize * gridSize));

        objectPositions = numbers.ToArray();
    }

    /// <summary>
    /// Established the Grid.
    /// </summary>
    ///
    public void SetUp()
    {
        envParameters = new EnvironmentParameters() {
            observation_size = 1,
            state_size = gridSize * gridSize,
            action_descriptions = new List<string>() { "Up", "Down", "Left", "Right" },
            action_size = 4,
            env_name = "FrozenLake",
            action_space_type = "discrete",
            state_space_type = "discrete",
            num_agents = 1
        };

        List<string> playersList = new List<string>();
        actorObjs = new List<GameObject>();

        for (int i = 0; i < numObstacles; i++)
            playersList.Add("pit");

        for (int i = 0; i < numGoals; i++)
            playersList.Add("goal");

        players = playersList.ToArray();
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        cam.transform.position = new Vector3((gridSize - 1), gridSize, -(gridSize - 1) / 2f);
        cam.orthographicSize = (gridSize + 5f) / 2f;
        SetEnvironment();
    }
}
