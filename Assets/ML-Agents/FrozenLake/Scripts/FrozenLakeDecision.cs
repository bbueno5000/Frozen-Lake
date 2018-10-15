using MLAgents;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class FrozenLakeDecision : MonoBehaviour, Decision
{
    private int action = -1;
    // Number of steps to lower e to eMin
    private int annealingSteps = 2000;
    // Initial epsilon value for random action selection
    private float e = 1;
    // Lower bound of epsilon
    private float eMin = 0.1f;
    // Discount factor for calculating Q-target
    private float gamma = 0.99f;

    private int lastState;
    // The rate at which to update the value estimates given a reward
    private float learning_rate = 0.5f;
    // The matrix containing the values estimates
    public float[][] q_table;

    public float[] Decide(List<float> vectorObs,
    List<Texture2D> visualObs, float reward, bool done, List<float> memory)
    {
        return new float[0];
    }

    /// <summary>
    /// Picks an action to take from its current state.
    /// </summary>
    ///
    /// <returns>
    /// The action choosen by the agent's policy
    /// </returns>
    ///
    public float[] GetAction()
    {
        action = q_table[lastState].ToList().IndexOf(q_table[lastState].Max());

        if (Random.Range(0f, 1f) < e)
            action = Random.Range(0, 3);

        if (e > eMin)
            e = e - ((1f - eMin) / (float) annealingSteps);

        GameObject.Find("ETxt").GetComponent<Text>().text = "Epsilon: " + e.ToString("F2");
        float currentQ = q_table[lastState][action];
        GameObject.Find("QTxt").GetComponent<Text>().text = "Current Q-value: " + currentQ.ToString("F2");
        return new float[1] {action};
    }

    /// <summary>
    /// Gets the values stored within the Q table.
    /// </summary>
    ///
    /// <returns>
    /// The average Q-values per state.
    /// </returns>
    ///
    public float[] GetValue()
    {
        float[] value_table = new float[q_table.Length];

        for (int i = 0; i < q_table.Length; i++)
            value_table[i] = q_table[i].Average();

        return value_table;
    }

    public List<float> MakeMemory(List<float> vectorObs,
    List<Texture2D> visualObs, float reward, bool done, List<float> memory)
    {
        return new List<float>();
    }

    public void SendParameters(EnvironmentParameters environmentParameters)
    {
        q_table = new float[environmentParameters.state_size][];
        action = 0;

        for (int i = 0; i < environmentParameters.state_size; i++)
        {
            q_table [i] = new float[environmentParameters.action_size];

            for (int j = 0; j < environmentParameters.action_size; j++)
                q_table[i][j] = 0.0f;
        }
    }

    /// <summary>
    /// Updates the value estimate matrix given a new experience (state, action, reward).
    /// </summary>
    ///
    /// <param name="state">
    /// The environment state the experience happened in.
    /// </param>
    ///
    /// <param name="reward">
    /// The reward recieved by the agent from the environment for it's action.
    /// </param>
    ///
    /// <param name="done">
    /// Whether the episode has ended
    /// </param>
    ///
    public void SendState(List<float> state, float reward, bool done)
    {
        int nextState = Mathf.FloorToInt(state.First());

        if (action != -1)
        {
            if (done == true)
            {
                q_table[lastState][action] += learning_rate * (
                    reward - q_table[lastState][action]);
            }
            else
            {
                q_table[lastState][action] += learning_rate * (
                    reward + gamma * q_table[nextState].Max() - q_table[lastState][action]);
            }
        }

        lastState = nextState;
    }
}
