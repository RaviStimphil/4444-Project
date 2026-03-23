using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    public Transform targetPos;
    public float moveSpeed;
    
    public override void OnEpisodeBegin(){
        transform.localPosition = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor){
         sensor.AddObservation(transform.localPosition);
         sensor.AddObservation(targetPos.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions){
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX,moveY, 0) * Time.deltaTime * moveSpeed; 
    }
    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        //continuousActions[0] = Input.GetAxisRaw("Horizontal");
        //continuousActions[1] = Input.GetAxisRaw("Verrical");
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
    }
}
