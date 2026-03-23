using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class BattlerAgent : Agent
{
    public Transform targetPos;
    public float moveSpeed;
    private AgentController controller;
    public Shooting shooter;
    public Vector3 respawnPoint;

    public float rotationSpeed = 180f; // degrees per second
    public float currentAngle = 0f;

    public Transform firePoint;
    public float firePointDistance = 0.5f;

    public override void Initialize()
    {
        controller = GetComponent<AgentController>();
        shooter = GetComponent<Shooting>();
    }
    
    public override void OnEpisodeBegin(){
        transform.localPosition = respawnPoint;
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetPos.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions){
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float rotateZ = actions.ContinuousActions[2];
        int shoot = actions.DiscreteActions[0];

        
        if(shoot == 1)
        {
            Vector2 dir = transform.up;
            shooter.Shoot(dir);
        }
        Rotate(rotateZ);
        controller.Move(new Vector2(moveX, moveY));   
    }
    public void Rotate(float amount){
        currentAngle += amount * rotationSpeed * Time.deltaTime;

        // Apply rotation
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        //continuousActions[0] = Input.GetAxisRaw("Horizontal");
        //continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
    public void RewardSet(float amount){
        SetReward(amount);
    }
    private void OnTriggerEnter2D(Collider2D other){
        if(other.TryGetComponent<Goal>(out Goal goal)){
            SetReward(+1f);
            EndEpisode(); 
        }
        if(other.TryGetComponent<Wall>(out Wall wall)){
            SetReward(-1f);
            EndEpisode(); 
        }
        if(other.TryGetComponent<Projectile>(out Projectile projectile)){
            //EndEpisode(); 
        }
    }
}
