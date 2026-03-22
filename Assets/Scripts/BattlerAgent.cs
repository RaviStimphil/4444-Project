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

    public int rayCount = 5;
    public float viewAngle = 90f;
    public float viewDistance = 10f;
    public LayerMask rayMask;

    public float rotationSpeed = 180f; // degrees per second
    public float currentAngle = 0f;

    public Transform firePoint;
    public float firePointDistance = 0.5f;
    /*public float rotationSpeed = 40f;
    public Vector2 facingDirection;
    public float firePointDistance = 0.5f;
*/

    void OnEnable(){
        SharedEvents.damageTaken += RewardForDamage;
    }
    void OnDisable(){
        SharedEvents.damageTaken -= RewardForDamage;
    }
    public override void Initialize()
    {
        controller = GetComponent<AgentController>();
        shooter = GetComponent<Shooting>();
    }
    
    public override void OnEpisodeBegin(){
        transform.localPosition = respawnPoint;
    }
    public void RewardForDamage(DamagePackage pack){
        //In case I want to make deployables... I really shouldn't.
        float rewardMultiplier = 1f; 
        //Take care of friendly fire or ally healing first.
        //Damage comes in negative.
        if(pack.TargetAlignment() == pack.SourceAlignment()){
            if(pack.TargetAlignment() == this.gameObject.GetComponent<UnitStats>().align){
                if(pack.source == this.gameObject){
                    SetReward(pack.damagePercent * 3f); //If the agent does the action, it gets more reward.
                    //If the agent did 30% of an allied agent HP, it is sent as -0.3
                    //-0.3 * 3 = -0.9 reward. Agent should learn not to damage ally.
                }else{
                    SetReward(pack.damagePercent); //If the agent didn't do the action, it gets some, but not as much.
                }
            }else{
                SetReward(-pack.damagePercent); //If the agent is not involved, it gets its reward. 
                
            }
        }
        //If the agent or allies is the target, negative reward if taking damage, positive vince vesa.
        else if(pack.TargetAlignment() == this.gameObject.GetComponent<UnitStats>().align){
            if(pack.target == this.gameObject){
                SetReward(pack.damagePercent * 3f);
                //Agent should learn to avoid damage. 
            }else{
                SetReward(pack.damagePercent);
            }
        }
        //Allied agents and self dealing damage is good. 
        else if(pack.SourceAlignment() == this.gameObject.GetComponent<UnitStats>().align){
            if(pack.source == this.gameObject){
                SetReward(-pack.damagePercent * 3f);
            }else{
                SetReward(-pack.damagePercent);
            }
        }
        //If there's a third faction for some reason... 
        //Enemies attacking each other is good. 
        else{
            SetReward(-pack.damagePercent);
        }
    }
    public override void CollectObservations(VectorSensor sensor){
         sensor.AddObservation(transform.localPosition);
         //sensor.AddObservation(targetPos.localPosition);

        for (int i = 0; i < rayCount; i++)
        {
            // Spread rays across an angle
            float angle = -viewAngle / 2f + (viewAngle / (rayCount - 1)) * i;

            Vector2 dir = Quaternion.Euler(0, 0, angle) * transform.up;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewDistance, rayMask);

            float hitType = 0f;   // what we hit
            float distance = 1f;  // normalized distance

            if (hit.collider != null)
            {
                distance = hit.distance / viewDistance;

                if (hit.collider.TryGetComponent<BattlerAgent>(out _) && GetComponent<Collider>().gameObject != this.gameObject)
                {
                    if(hit.collider.TryGetComponent<UnitStats>(out _)){
                        if(hit.collider.GetComponent<UnitStats>().align != this.gameObject.GetComponent<UnitStats>().align){
                            hitType = 1f; // enemy
                        }
                        else if(hit.collider.GetComponent<UnitStats>().align == this.gameObject.GetComponent<UnitStats>().align){
                            hitType = -1f; //ally
                        }
                        
                    }
                    
                }
                else if (hit.collider.TryGetComponent<Wall>(out _))
                {
                    hitType = 0f; // wall
                }
                else if (hit.collider.TryGetComponent<Projectile>(out _))
                {
                    Projectile projectile = hit.collider.GetComponent<Projectile>();
                    if(projectile.source == null){
                        Debug.Log(hit.collider.gameObject.name + " doesn't have a source as a projectile.");
                        return;
                    }
                    else if(projectile.TellAlignment() != this.gameObject.GetComponent<UnitStats>().align){
                        if(projectile.friendlyFire >= 0){
                            hitType = 0.8f; //Enemy Bullet Dangerous.
                        }
                        else{
                            hitType = 0.6f; //Enemy Bullet Harmless. 
                        }
                    }
                    else{
                        if(projectile.friendlyFire == 0){
                            hitType = -0.8f; //Ally Bullet Dangerous.
                        }
                        else{
                            //Assumption is that ally bullets that affect only ally
                            //will not be harmful for the agent. 
                            hitType = -0.6f; //Ally Bullet Harmless. 
                        }
                    }
                    
                }
                if(hit.collider.TryGetComponent<UnitStats>(out _)){
                    sensor.AddObservation((float) hit.collider.GetComponent<UnitStats>().currentHP/hit.collider.GetComponent<UnitStats>().maxHP);
                    
                }
            }

            // Add to ML observations
            sensor.AddObservation(hitType);
            sensor.AddObservation(distance);
            

            // Debug visualization
            Debug.DrawRay(transform.position, dir * viewDistance, Color.red);
        }
    }
    public override void OnActionReceived(ActionBuffers actions){
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float rotateZ = actions.ContinuousActions[2];
        int shoot = actions.DiscreteActions[0];

        
        controller.Move(new Vector2(moveX, moveY));
        Rotate(rotateZ);
        Vector3 direction = transform.up; // forward in 2D
        firePoint.position = transform.position + direction * firePointDistance;
        if(shoot == 1)
        {
            Vector2 dir = (targetPos.position - transform.position).normalized;
            shooter.Shoot(dir, firePoint);
        }
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
