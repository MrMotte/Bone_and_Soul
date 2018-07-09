using UnityEngine;
using System.Collections;

 public class Eichoernli: MonoBehaviour {
 
 
 public Transform[] waypoint;        
 public float patrolSpeed = 3f;     
 public bool  loop = true;      
 public float dampingLook= 6.0f;          
 public float pauseDuration = 0;   
 public GameObject modell;
  
 private bool canWalk;
 private float curTime;
 private int currentWaypoint = 0;
 private CharacterController character;
  
 void  Start (){
  
     character = GetComponent<CharacterController>();
 }

 void  Update (){
  
		if(currentWaypoint < waypoint.Length){
			if (canWalk) {
				patrol ();
			}
     }else{    
         if(loop){
             currentWaypoint=0;
         } 
     }
 }
  
 void  patrol (){
  
     Vector3 target = waypoint[currentWaypoint].position;
     target.y = transform.position.y; // Keep waypoint at character's height
     Vector3 moveDirection = target - transform.position;
  
     if(moveDirection.magnitude < 0.5f){
         if (curTime == 0)
             curTime = Time.time; // Pause over the Waypoint
         if ((Time.time - curTime) >= pauseDuration){
             currentWaypoint++;
             curTime = 0;
         }
     }else{        
         var rotation= Quaternion.LookRotation(target - transform.position);
         transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampingLook);
         character.Move(moveDirection.normalized * patrolSpeed * Time.deltaTime);
     }  
 }
		
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			canWalk = true;
			modell.SetActive (true);
			Invoke ("Restart", 10f);
		}
	}

	void Restart (){

		transform.position = waypoint [0].position;
		modell.SetActive (false);
		canWalk = false;
		currentWaypoint = 0;
	}
 }