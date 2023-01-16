using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.SocialPlatforms;
/// <summary>
/// script simulating charater movement behaviour
/// </summary>
public class PersonWalk : MonoBehaviour {
    /// <summary>
    /// animation number assigned to character
    /// </summary>
    public int animationNumber;
    /// <summary>
    /// animation speed multiplier
    /// </summary>
    private float walkingSpeedMultiplier = 1f;
    /// <summary>
    /// character rotation speed
    /// </summary>
    private float rotSpeed = 0f;
    /// <summary>
    /// are obstacles in front of character
    /// </summary>
    private bool obstacleInFront;
    /// <summary>
    /// are obstacles in on the right of character
    /// </summary>
    private bool obstacleOnTheRight;
    /// <summary>
    /// are obstacles in on the left of character
    /// </summary>
    private bool obstacleOnTheLeft;
    /// <summary>
    /// rays detecting obstacles in front of character 
    /// </summary>
    private Ray[] frontRays = new Ray[3];
    /// <summary>
    /// rays detecting obstacleson the left character
    /// </summary>
    private Ray[] leftRays = new Ray[10];
    /// <summary>
    /// rays detecting obstacleson the right character
    /// </summary>
    private Ray[] rightRays = new Ray[10];
    /// <summary>
    /// rotation towards which character is heading
    /// </summary>
    private Quaternion targetRotation;
    /// <summary>
    /// animation controller
    /// </summary>
    private Animator anim;
    /// <summary>
    /// position towards character is heading
    /// </summary>
    public GameObject targetPoint;
    /// <summary>
    /// character movement behaviour strategy
    /// </summary>
    IMovementStrategy strategy { get; set; }

    /// <summary>
    /// gets animation controller, sets animation speed multiplier with ranodm value between [0.9;1.1]  
    /// </summary>
    void Start () 
	{
		anim = GetComponent<Animator>();
		walkingSpeedMultiplier = Random.Range(0.9f, 1.1f);
	}
    /// <summary>
    ///  sets rays in adequate rotation with origin point at character position 
    /// </summary>
    void SetRays()
	{
		frontRays[0] = new Ray(gameObject.transform.position + Vector3.up, gameObject.transform.forward);
		frontRays[1] = new Ray(gameObject.transform.position + Vector3.up, Quaternion.AngleAxis(5, Vector3.up) * gameObject.transform.forward);
		frontRays[2] = new Ray(gameObject.transform.position + Vector3.up, Quaternion.AngleAxis(-5, Vector3.up) * gameObject.transform.forward);
		for(int i=0; i<10; i++)
		{
			leftRays[i] = new Ray(gameObject.transform.position + Vector3.up, Quaternion.AngleAxis(-(i+2)*5, Vector3.up) * gameObject.transform.forward);
			rightRays[i] = new Ray(gameObject.transform.position + Vector3.up, Quaternion.AngleAxis((i+2)*5, Vector3.up) * gameObject.transform.forward);
		}
	}
    /// <summary>
    /// applies gravity to character 
    /// </summary>
    void ApplyGravity()
	{
		gameObject.transform.Translate(0.0f, -0.001f, 0.0f);
	}
    /// <summary>
    ///  sets animation speed based on fram rate 
    /// </summary>
    void SetAnimationSpeed()
	{
		anim.speed = walkingSpeedMultiplier  * Mathf.Min(1f, (0.01666667f/Time.deltaTime));
    }

	/// <summary>
	/// detects obstacles within array of rays
	/// </summary>
	/// <param name="rays">array of rays to detect obstacle with</param>
	/// <param name="range">range at which obstacles are detected</param>
	/// <returns>true if at least one ray detected obstacle, false otherwise</returns>
	bool DetectObstacleWithRays(Ray[] rays, float range)
	{
		for(int i =0; i<rays.Length; i++)
		{
			if(Physics.Raycast(rays[i], range))
				return true;
		}
		return false;
	}

    /// <summary>
    /// sets adequate movement strategy
    /// </summary>
    void SetStrategy()
	{
		if(obstacleInFront && obstacleOnTheLeft && obstacleOnTheRight)
			strategy = new GoToPOIStrategy();
		else if(obstacleOnTheLeft && obstacleOnTheRight)
			strategy = new MoveStraightStrategy();
		else if(obstacleOnTheRight)
			strategy = new TurnLeftStrategy();
		else if(obstacleOnTheLeft)
			strategy = new TurnRightStrategy();
		else if(obstacleInFront)
		{
			float random = Random.Range(0.0f, 1.0f);
			if(random < 0.5)
				strategy = new TurnLeftStrategy();
			else
				strategy = new TurnRightStrategy();
		}
		else // no obstacles
			strategy = new GoToPOIStrategy();
	}

    /// <summary>
    /// detects obstacles using all rays
    /// </summary>
    void DetectObstacles()
	{
		obstacleInFront = DetectObstacleWithRays(frontRays, 1f);
		obstacleOnTheLeft = DetectObstacleWithRays(leftRays, 0.8f);
		obstacleOnTheRight = DetectObstacleWithRays(rightRays, 0.8f);
	}

    /// <summary>
    ///  moves character based on strategy
    /// </summary>
    void MovePerson()
	{
		strategy.MoveCharacter(ref rotSpeed, gameObject.transform, targetRotation);
	}

    /// <summary>
    ///  sets rotation towards which character is heading
    /// </summary>
    void SetMovementAngle()
	{
		targetRotation = Quaternion.LookRotation(new Vector3(targetPoint.transform.position.x - gameObject.transform.position.x, 0.0f, targetPoint.transform.position.z - gameObject.transform.position.z));
	}

    /// <summary>
    /// chooses new target position at random once current target is reached, despawns character if target was one of spawning points
    /// </summary>
    void ChangeTargetPoint()
	{
		if(Vector3.Distance(gameObject.transform.position, targetPoint.transform.position) < 2f)
		{
			if(targetPoint.tag == "Respawn")
			{
				Destroy(gameObject);
				CrowdManager.peopleOnScene--;
			}
			ConnectionList connectedPoints = targetPoint.GetComponent<ConnectionList>();
			if(connectedPoints)
			{
				GameObject newTarget = connectedPoints.pointsArray[Random.Range(0,connectedPoints.pointsArray.Length)];
				if(newTarget)
					targetPoint = newTarget;
			}
		}
	}

    // Update is called once per frame
    /// <summary>
    /// sets rays, animation speed, heading direction, applies gravty, detects obstacles and sets strategy
    /// </summary>
    void Update() 
	{
		if (anim) 
		{
			SetMovementAngle();

			SetRays();

			anim.SetFloat("Speed", 1f);
			anim.SetFloat("Blend", (float)animationNumber);

			SetAnimationSpeed();

			ChangeTargetPoint();

			DetectObstacles();

			SetStrategy();

			MovePerson();

			ApplyGravity();
		}
	}
}
