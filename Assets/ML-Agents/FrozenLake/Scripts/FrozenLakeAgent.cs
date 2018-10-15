using MLAgents;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class FrozenLakeAgent : Agent
{
    private Transform target;

    /// <summary>
    /// Specifies the agent behavior at every step
    /// based on the provided action.
    /// </summary>
    ///
    /// <param name="vectorAction">
    /// Vector action.
    /// Note that for discrete actions, the provided array will be of length 1.
    /// </param>
    ///
    /// <param name="textAction">
    /// Text action.
    /// </param>
    ///
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        SetReward(-0.05f);
        // 0 - Forward, 1 - Backward, 2 - Left, 3 - Right
        switch ((int) vectorAction[0])
        {
            case 0:
            {
                Collider[] blockTest = Physics.OverlapBox(new Vector3(
                    this.transform.position.x, 0,
                    this.transform.position.z + 1),
                    new Vector3(0.3f, 0.3f, 0.3f));
                if (blockTest.Where(col => col.gameObject.tag == "wall").ToArray().Length == 0)
                    this.transform.position = new Vector3(
                        this.transform.position.x, 0,
                        this.transform.position.z + 1);
                break;
            }
            case 1:
            {
                Collider[] blockTest = Physics.OverlapBox(new Vector3(
                    this.transform.position.x, 0,
                    this.transform.position.z - 1),
                    new Vector3(0.3f, 0.3f, 0.3f));
                if (blockTest.Where(col => col.gameObject.tag == "wall").ToArray().Length == 0)
                    this.transform.position = new Vector3(
                        this.transform.position.x, 0,
                        this.transform.position.z - 1);
                break;
            }
            case 2:
            {
                Collider[] blockTest = Physics.OverlapBox(new Vector3(
                    this.transform.position.x - 1, 0,
                    this.transform.position.z),
                    new Vector3(0.3f, 0.3f, 0.3f));
                if (blockTest.Where(col => col.gameObject.tag == "wall").ToArray().Length == 0)
                    this.transform.position = new Vector3(
                        this.transform.position.x - 1, 0,
                        this.transform.position.z);
                break;
            }
            case 3:
            {
                Collider[] blockTest = Physics.OverlapBox(new Vector3(
                    this.transform.position.x + 1, 0,
                    this.transform.position.z),
                    new Vector3(0.3f, 0.3f, 0.3f));
                if (blockTest.Where(col => col.gameObject.tag == "wall").ToArray().Length == 0)
                    this.transform.position = new Vector3(
                        this.transform.position.x + 1, 0,
                        this.transform.position.z);
                break;
            }
            default:
                break;
        }

        Collider[] hitObjects = Physics.OverlapBox(
            this.transform.position, new Vector3(0.3f, 0.3f, 0.3f));

        if (hitObjects.Where(col => col.gameObject.tag == "goal").ToArray().Length == 1)
        {
            AddReward(1);
            Done();
        }

        if (hitObjects.Where(col => col.gameObject.tag == "pit").ToArray().Length == 1)
        {
            AddReward(-1);
            Done();
        }

        // GameObject.Find("RTxt").GetComponent<Text>().text = string.Format(
        //     "Episode Reward: {0}", gameObject.GetCumulativeReward().ToString("F2"));
    }

    /// <summary>
    /// Specifies the agent behavior when done and
    /// <see cref="AgentParameters.resetOnDone"/> is false.
    ///
    /// This method can be used to remove the agent from the scene.
    /// </summary>
    ///
    public override void AgentOnDone()
    {

    }

    /// <summary>
    /// Specifies the agent behavior when being reset,
    /// which can be due to the agent or Academy being done
    /// (i.e. completion of local or global episode).
    /// </summary>
    ///
    public override void AgentReset()
    {
        ResetReward();
    }

    /// <summary>
    /// Collects the (vector, visual, text) observations of the agent.
    /// The agent observation describes the current environment
    /// from the perspective of the agent.
    /// </summary>
    ///
    /// <remarks>
    /// Simply, an agents observation is any environment information that helps
    /// the Agent acheive its goal. For example, for a fighting Agent, its
    /// observation could include distances to friends or enemies, or the
    /// current level of ammunition at its disposal.
    /// Recall that an Agent may attach vector, visual or textual observations.
    /// Vector observations are added by calling the provided helper methods:
    ///     - <see cref="AddVectorObs(int)"/>
    ///     - <see cref="AddVectorObs(float)"/>
    ///     - <see cref="AddVectorObs(Vector3)"/>
    ///     - <see cref="AddVectorObs(Vector2)"/>
    ///     - <see cref="AddVectorObs(float[])"/>
    ///     - <see cref="AddVectorObs(List{float})"/>
    ///     - <see cref="AddVectorObs(Quaternion)"/>
    /// Depending on your environment, any combination of these helpers can
    /// be used. They just need to be used in the exact same order each time
    /// this method is called and the resulting size of the vector observation
    /// needs to match the observationSize attribute of the linked Brain.
    /// Visual observations are implicitly added from the cameras attached to
    /// the Agent.
    /// Lastly, textual observations are added using
    /// <see cref="SetTextObs(string)"/>.
    /// </remarks>
    ///
    public override void CollectObservations()
    {
        // float distanceToTarget = Vector3.Distance(
        //     this.transform.position, target.position);

        // AddVectorObs(distanceToTarget);
        AddVectorObs(0);
    }

    /// <summary>
    /// Initializes the agent, called once when the agent is enabled. Can be
    /// left empty if there is no special, unique set-up behavior for the
    /// agent.
    /// </summary>
    ///
    /// <remarks>
    /// One sample use is to store local references to other objects in the
    /// scene which would facilitate computing this agents observation.
    /// </remarks>
    ///
    public override void InitializeAgent()
    {
        // target = GameObject.Find("goal").transform;
    }
}
