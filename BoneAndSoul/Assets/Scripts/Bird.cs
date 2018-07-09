using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
{
	public Waypoint[] wayPoints;
	public Transform exitWaypoint;
	public float speed = 3f;
	public float flightSpeed = 3f;
	public float dampingLook = 1f;
	public bool isCircular;
	// Always true at the beginning because the moving object will always move towards the first waypoint
	public bool inReverse = true;

	private Waypoint currentWaypoint;
	private int currentIndex   = 0;
	private bool isWaiting     = false;
	private float speedStorage = 0;

	private bool fliegen;
	private Animator BirdAnimation;

	

	/**
	 * Initialisation
	 * 
	 */
	void Start () {
		fliegen = true;
		if(wayPoints.Length > 0) {
			currentWaypoint = wayPoints[0];
		}

		BirdAnimation = GetComponentInChildren<Animator>();
	}
	
	

	/**
	 * Update is called once per frame
	 * 
	 */
	void Update()
	{
		if(currentWaypoint != null && !isWaiting) {
			MoveTowardsWaypoint();
		}

		if (fliegen == false){
			transform.position = Vector3.MoveTowards (transform.position, exitWaypoint.transform.position, flightSpeed);
			transform.LookAt (exitWaypoint.transform.position);
		}
	}



	/**
	 * Pause the mover
	 * 
	 */
	void Pause()
	{
		isWaiting = !isWaiting;
		BirdAnimation.SetBool ("Walk", false);
	}


	
	/**
	 * Move the object towards the selected waypoint
	 * 
	 */
	private void MoveTowardsWaypoint()
	{
		// Get the moving objects current position
		Vector3 currentPosition = this.transform.position;
		
		// Get the target waypoints position
		Vector3 targetPosition = currentWaypoint.transform.position;
		
		// If the moving object isn't that close to the waypoint
		if(Vector3.Distance(currentPosition, targetPosition) > .1f) {

			// Get the direction and normalize
			Vector3 directionOfTravel = targetPosition - currentPosition;
			directionOfTravel.Normalize();
			if (fliegen) {
				//scale the movement on each axis by the directionOfTravel vector components
				transform.position = Vector3.MoveTowards (transform.position, currentWaypoint.transform.position, speed);
				Quaternion rotation = Quaternion.LookRotation(currentWaypoint.transform.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampingLook);
				BirdAnimation.SetBool ("Walk", true);;
			}
		} else {
			
			// If the waypoint has a pause amount then wait a bit
			if(currentWaypoint.waitSeconds > 0) {
				Pause();
				Invoke("Pause", currentWaypoint.waitSeconds);
			}

			// If the current waypoint has a speed change then change to it
			if(currentWaypoint.speedOut > 0) {
				speedStorage = speed;
				speed = currentWaypoint.speedOut;
			} else if(speedStorage != 0) {
				speed = speedStorage;
				speedStorage = 0;
			}

			NextWaypoint();
		}
	}



	/**
	 * Work out what the next waypoint is going to be
	 * 
	 */
	private void NextWaypoint()
	{
		if(isCircular) {
			
			if(!inReverse) {
				currentIndex = (currentIndex+1 >= wayPoints.Length) ? 0 : currentIndex+1;
			} else {
				currentIndex = (currentIndex == 0) ? wayPoints.Length-1 : currentIndex-1;
			}

		} else {
			
			// If at the start or the end then reverse
			if((!inReverse && currentIndex+1 >= wayPoints.Length) || (inReverse && currentIndex == 0)) {
				inReverse = !inReverse;
			}
			currentIndex = (!inReverse) ? currentIndex+1 : currentIndex-1;

		}

		currentWaypoint = wayPoints[currentIndex];
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			BirdAnimation.SetBool ("Fliegen", true);
			BirdAnimation.SetBool ("Walk", false);
			fliegen = false;
		}
	}
}