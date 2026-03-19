using UnityEngine;
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

        for (int i = 0; i < rayCount; i++)
        {
            // Spread rays across an angle
            float angle = -viewAngle / 2f + (viewAngle / (rayCount - 1)) * i;

            Vector2 dir = Quaternion.Euler(0, 0, angle) * transform.right;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewDistance, rayMask);

            float hitType = 0f;   // what we hit
            float distance = 1f;  // normalized distance

            if (hit.collider != null)
            {
                distance = hit.distance / viewDistance;

                if (hit.collider.TryGetComponent<BattlerAgent>(out _))
                {
                    hitType = 1f; // enemy
                }
                else if (hit.collider.TryGetComponent<Wall>(out _))
                {
                    hitType = -1f; // wall
                }
                else if (hit.collider.TryGetComponent<Projectile>(out _))
                {
                    hitType = -0.5f; // bullet (danger)
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

        int shoot = actions.DiscreteActions[0];

        if(shoot == 1)
        {
            Vector2 dir = (targetPos.position - transform.position).normalized;
            shooter.Shoot(dir);
        }
        controller.Move(new Vector2(moveX, moveY));   
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
