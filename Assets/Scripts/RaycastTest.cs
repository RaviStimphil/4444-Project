using UnityEngine;

public class RaycastTest : MonoBehaviour
{
    Ray ray;
    float maxDistance = 100;
    public LayerMask layersToHit;

    void Start(){
        Physics2D.queriesHitTriggers = true;
        ray = new Ray(transform.position, transform.up);
        CheckForColliders();
        Debug.Log("STIHNT");
    }
    void CheckForColliders(){
        
        Vector2 direction = transform.up;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, layersToHit);
        Debug.DrawRay(transform.position, transform.up * maxDistance, Color.red);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name + " was hit");
        }
    }
}
