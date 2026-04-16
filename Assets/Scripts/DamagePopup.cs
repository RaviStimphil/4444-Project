using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DamagePopup : MonoBehaviour
{
    public TextMeshPro textMesh;

    public static DamagePopup ShowReward(float amount, GameObject source, GameObject textThing){
        float randomRange = 0.1f;
        Vector3 randomOffset = new Vector3(
            Random.Range(-randomRange, randomRange), 
            Random.Range(-randomRange, randomRange),
            0  
        );
        Transform damagePopupTransform = Instantiate(textThing.transform, source.transform.position + randomOffset, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.textMesh.color = new Color(0.75f, 0.75f, 0.75f, 1f);
        damagePopup.Setup(amount);       

        return damagePopup;
    }
    void Awake(){
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(float amount){
        if(amount < 0){
            textMesh.SetText(amount.ToString());
        }else{
            textMesh.SetText("+" + amount.ToString());
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LifeTime());
    }

    // Update is called once per frame
    void Update()
    {
        float moveYSpeed = 5f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
    }
    public IEnumerator LifeTime(){
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
