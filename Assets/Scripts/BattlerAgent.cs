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
    public Vector3 respawnPoint;
    public float pendingRewards;

    private Collider2D col;
    private Rigidbody2D rb;
    private int aliveLayer;
    private int deadLayer;

    private bool isDead = false;

    public float rotationSpeed = 180f; // degrees per second
    public float currentAngle = 0f;

    public Vector3 firePoint;
    public float firePointDistance = 0.5f;

    public List<AttackBehavior> basicOptions;
    public List<AttackBehavior> specialOptions;

    public AttackBehavior basicAttack;
    public AttackBehavior specialAttack; 

    private float agentBasic;
    private float agentSpecial;

    public GameObject floatingText;
        
    public bool canAttack = true;
    public bool canMove = true;

    public int rayCount = 5;
    public float viewAngle = 60f;
    public float viewDistance = 25f;
    public LayerMask rayMask;

    public float maxRewardRange;
    public float minRewardRange;




    public override void Initialize()
    {
        controller = GetComponent<AgentController>();

        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        aliveLayer = LayerMask.NameToLayer("Solid");
        deadLayer = LayerMask.NameToLayer("Dead");
        
    }
    public void UpdateAbility()
    {
        // Pick random basic attack
        int basicIndex = UnityEngine.Random.Range(0, basicOptions.Count);
        AttackBehavior basicChoice = basicOptions[basicIndex];
        agentBasic = (float) basicIndex / basicOptions.Count;

        basicAttack = Instantiate(basicChoice);
        basicAttack.Initialize(gameObject);

        minRewardRange = basicAttack.minRewardRange;
        maxRewardRange = basicAttack.maxRewardRange;
        // Pick random special attack
        int specialIndex = UnityEngine.Random.Range(0, specialOptions.Count);
        AttackBehavior specialChoice = specialOptions[specialIndex];
        agentSpecial = (float) specialIndex / specialOptions.Count;

        specialAttack = Instantiate(specialChoice);
        specialAttack.Initialize(gameObject);

        minRewardRange = Math.Min(basicAttack.minRewardRange, specialAttack.minRewardRange);
        maxRewardRange = Math.Max(basicAttack.maxRewardRange, specialAttack.maxRewardRange);
    }
    void FixedUpdate()
    {
        if(basicAttack){
            basicAttack.Tick(Time.deltaTime);
        }
        if(specialAttack){
            specialAttack.Tick(Time.deltaTime);
        }
        
    }

    public override void OnEpisodeBegin(){
        transform.localPosition = respawnPoint;
        SharedEvents.BeginRound();
        this.gameObject.GetComponent<UnitStats>().ResetStats();
        currentAngle = 0f;
        UpdateAbility();
        ToggleActing(true);
        //Need to reset cooldown and add death...
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetPos.localPosition);
        sensor.AddObservation(agentBasic);
        sensor.AddObservation(agentSpecial);
        sensor.AddObservation(basicAttack.GetCooldownNormalized());
        sensor.AddObservation(specialAttack.GetCooldownNormalized());
        if(isDead){
            sensor.AddObservation(-1);
        }else{
            sensor.AddObservation(1);
        }
        if(!canAttack){
            sensor.AddObservation(0f);
        }else{
            sensor.AddObservation(1f);
        }
        
        for (int i = 0; i < rayCount; i++)
        {
            //Need to make two sets of rays. 
            // Spread rays across an angle
            float angle = -viewAngle / 2f + (viewAngle / (rayCount - 1)) * i;

            Vector2 dir = Quaternion.Euler(0, 0, angle) * transform.up;
            Vector3 offset = new Vector3(0, 0.75f, 0);
            RaycastHit2D solidHit = Physics2D.Raycast(transform.position + offset, dir, viewDistance, LayerMask.GetMask("Solid"));
            RaycastHit2D projectileHit = Physics2D.Raycast(transform.position + offset, dir, viewDistance, LayerMask.GetMask("Projectile"));
            float solidHitType = 0f;   // what we hit
            float solidDistance = 1f;  // normalized distance

            float projectileHitType = 0f;   
            float projectileDistance = 1f;

            if (solidHit.collider != null)
            {
                solidDistance = solidHit.distance / viewDistance;

                if (solidHit.collider.TryGetComponent<BattlerAgent>(out _) && solidHit.collider.gameObject != this.gameObject)
                {
                    if(solidHit.collider.TryGetComponent<UnitStats>(out _)){
                        if(solidHit.collider.GetComponent<UnitStats>().align != this.gameObject.GetComponent<UnitStats>().align){
                            solidHitType = 1f; // enemy
                        }
                        else if(solidHit.collider.GetComponent<UnitStats>().align == this.gameObject.GetComponent<UnitStats>().align){
                            solidHitType = -1f; //ally
                        }
                        
                    }
                    
                }
                else if (solidHit.collider.TryGetComponent<Wall>(out _))
                {
                    solidHitType = 0.25f; // wall
                }
            }
            if(projectileHit.collider != null) {
                solidDistance = projectileHit.distance / viewDistance;
                if (projectileHit.collider.TryGetComponent<Projectile>(out _))
                {
                    Projectile projectile = projectileHit.collider.GetComponent<Projectile>();
                    if(projectile.source == null){
                        Debug.Log(projectileHit.collider.gameObject.name + " doesn't have a source as a projectile.");
                        return;
                    }
                    else if(projectile.TellAlignment() != this.gameObject.GetComponent<UnitStats>().align){
                        if(projectile.friendlyFire >= 0){
                            projectileHitType = 0.8f; //Enemy Bullet Dangerous.
                        }
                        else{
                            projectileHitType = 0.6f; //Enemy Bullet Harmless. 
                        }
                    }
                    else{
                        if(projectile.friendlyFire == 0){
                            projectileHitType = -0.8f; //Ally Bullet Dangerous.
                        }
                        else{
                            //Assumption is that ally bullets that affect only ally
                            //will not be harmful for the agent. 
                            projectileHitType = -0.6f; //Ally Bullet Harmless. 
                        }
                    }
                }   
                
                
            }

            // Add to ML observations
            sensor.AddObservation(projectileHitType);
            sensor.AddObservation(projectileDistance);
            sensor.AddObservation(solidHitType);
            sensor.AddObservation(solidDistance);

            // Debug visualization
            Debug.DrawRay(transform.position, dir * viewDistance, Color.red);
        }
    }
    public override void OnActionReceived(ActionBuffers actions){
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float rotateZ = actions.ContinuousActions[2];
        int shoot = actions.DiscreteActions[0];
        int ability = actions.DiscreteActions[1];
        AddReward(pendingRewards);
        if(pendingRewards != 0){
            DamagePopup.ShowReward(pendingRewards, this.gameObject, floatingText);
        }
        
        pendingRewards = 0;
        
        if(shoot == 1 && canAttack)
        {
            basicAttack.DoAction();
        }
        if(ability == 1 && canAttack){
            specialAttack.DoAction();
            Debug.Log("It used special move");
        }
        Rotate(rotateZ);
        if(canMove){
            controller.Move(new Vector2(moveX, moveY));   
        }
        
    }
    public void Rotate(float amount){
        currentAngle += amount * rotationSpeed * Time.deltaTime;

        // Apply rotation
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
    public void ToggleDeath(bool dead){
        col.enabled = !dead;
        ToggleActing(dead);
        rb.simulated = !dead;
        isDead = dead;
        if(dead){
            rb.linearVelocity = Vector2.zero;
            gameObject.layer = deadLayer;
            
        }else{
            gameObject.layer = aliveLayer;
        }
    }
    public void ToggleActing(bool canAct){
        if(canAct){
            canAttack = true;
            canMove = true;
        }
        else{
            canAttack = false;
            canMove = false;
        }
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
        DamagePopup.ShowReward(amount, this.gameObject, floatingText);
    }
    public void PendingRewardAdd(float amount){
        pendingRewards += amount; 
    }
    
}
