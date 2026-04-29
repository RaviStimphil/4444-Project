using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnnouncementText : MonoBehaviour
{
    void OnEnable(){
        SharedEvents.unitDeath += DeathAnnounce;
        SharedEvents.startRound += ClearText;
    }
    void OnDisable(){
        SharedEvents.unitDeath -= DeathAnnounce;
        SharedEvents.startRound -= ClearText;
    }
    public TextMeshProUGUI newsText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DeathAnnounce(GameObject unit){
        Debug.Log("Got it");
        if(unit.name == "Agent"){
            newsText.text = "Agent2 is the winner! Give a round of applause!";
        }else{
            newsText.text = "Agent1 is the winner! Raise your glass for the win!";
        }
    }
    public void ClearText(){
        newsText.text = "";
    }
}
