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
    public float pendingRewards;

    public float rotationSpeed = 180f; // degrees per second
    public float currentAngle = 0f;

    public Transform firePoint;
    public float firePointDistance = 0.5f;

    public int rayCount = 5;
    public float viewAngle = 90f;
    public float viewDistance = 15f;

    public GameObject floatingText;
        
    

    public override void Initialize()
    {
        controller = GetComponent<AgentController>();
        shooter = GetComponent<Shooting>();
    }
    
    public override void OnEpisodeBegin(){
        transform.localPosition = respawnPoint;
        this.gameObject.GetComponent<UnitStats>().ResetStats();
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.localPosition);
        //sensor.AddObservation(targetPos.localPosition);
        sensor.AddObservation(transform.up);
        float hitType = 0f;  
        float distance = 1f; 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, viewDistance);
        if (hit.collider != null)
        {
            distance = hit.distance / viewDistance;

            if (hit.collider.TryGetComponent<BattlerAgent>(out _))
            {
                AddReward(0.01f);
                hitType = 1f; 
            }
            else if (hit.collider.TryGetComponent<Wall>(out _))
            {
                hitType = -1f;
            }
        }
        sensor.AddObservation(hitType);
        sensor.AddObservation(distance);
        Debug.DrawRay(transform.position, transform.up * viewDistance, Color.red);
    }
    public override void OnActionReceived(ActionBuffers actions){
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float rotateZ = actions.ContinuousActions[2];
        int shoot = actions.DiscreteActions[0];
        AddReward(pendingRewards);
        pendingRewards = 0;
        
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
    public void RewardForDeath(GameObject unit){

    }
    public void RewardForDamage(DamagePackage pack){

    }
    public void RewardSet(float amount){
        SetReward(amount);
        DamagePopup.ShowReward(amount, this.gameObject, floatingText);
    }
    public void RewardAdd(float amount){
        AddReward(amount);
    }
    public void PendingRewardAdd(float amount){
        pendingRewards += amount; 
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
