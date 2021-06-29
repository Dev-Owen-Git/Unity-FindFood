using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BlueAgent : Agent
{
    [SerializeField] Rigidbody rb;
    [SerializeField] FoodButton foodButton;

    public override void OnEpisodeBegin()
    {
        rb.velocity = Vector3.zero;
        foodButton.isCanUseButton = true;
        foodButton.isCanPushButton = false;
        foodButton.food.gameObject.SetActive(false);
        transform.localPosition = new Vector3(0,1,0);
    }

public override void Heuristic(in ActionBuffers actionsOut)
{
    ActionSegment<float> ContinuousActions = actionsOut.ContinuousActions;
    ActionSegment<int> DiscreteActions = actionsOut.DiscreteActions;

    ContinuousActions[0] = Input.GetAxis("Horizontal");
    ContinuousActions[1] = Input.GetAxis("Vertical");

    DiscreteActions[0] = Input.GetKey( KeyCode.Space ) == true ? 1: 0;
}

public override void CollectObservations(VectorSensor sensor)
{
    sensor.AddObservation(foodButton.isCanUseButton == true ? 1 : 0);

    Vector3 dirToFoodButton = (foodButton.transform.position - transform.position).normalized;
    sensor.AddObservation(dirToFoodButton.x);
    sensor.AddObservation(dirToFoodButton.z);

    sensor.AddObservation(foodButton.isSpawnFood == true ? 1 : 0);

    if (foodButton.isSpawnFood == true)
    {
        Vector3 dirToFood = (foodButton.food.position - transform.position).normalized;
        sensor.AddObservation(dirToFood.x);
        sensor.AddObservation(dirToFood.z);
    }
    else
    {
        sensor.AddObservation(0);  // x
        sensor.AddObservation(0);  // z
    }
}

    public override void OnActionReceived(ActionBuffers actions)
    {
        float x = actions.ContinuousActions[0];
        float z = actions.ContinuousActions[1];

        int isPushButton = actions.DiscreteActions[0];

        float moveSpeed = 5;
        rb.velocity = new Vector3(x, rb.velocity.y, z) * moveSpeed;

        if (isPushButton == 1)
        {
            if (foodButton.isCanPushButton && foodButton.isCanUseButton)
            {
                foodButton.SpawnFood();
                SetReward(+1);
            }
        }
        SetReward(+1 / MaxStep);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if ( other.transform.TryGetComponent( out Food _food ) )    
        {
            SetReward( +1 );
            _food.EatFood();
            EndEpisode();
        }
    }
}
