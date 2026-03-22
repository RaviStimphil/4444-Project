using UnityEngine;

public class AgentController : MonoBehaviour
{
    public float moveSpeed;

    public void Move(Vector2 direction){
        direction = direction.normalized;

        transform.localPosition += (Vector3)direction * moveSpeed * Time.deltaTime;
    }
    
}
