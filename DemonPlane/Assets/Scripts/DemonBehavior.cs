using UnityEngine;
using System.Collections;

public class DemonBehavior : MonoBehaviour 
{

	public int VillageDamagePerSecond = 5;

	private GameObject TargetVillage;
	private HealthBar HealthBarComp;

    public bool IsFlyingDemon = false;
    public float ShootInterval = 3.0f;
    public float MaxShootDistance = 100.0f;
    public float RotationSpeed = 10.0f;

	public float WalkingSpeed;
	public double TimeToDisappear; // the time between the demon reaching 0 HP and disappearing

    public float TimeFollowingTarget = 5.0f;
    public float TimeFlyingInLastDirection = 5.0f;
	//rotation
	//private Transform target;

	private bool bSoundPlayed;
	private double TimeTillDeath;
	private float HealthScale; 
	private bool hit;
    private float lastShootTime;
    private float lastFrameAngle;

	public AudioSource DemonExtinguishSFX;
	public AudioSource DemonDefeatedSFX;

	public ParticleSystem HitFX;
	//private int numHitFX;
	private Quaternion defaultRotation;
    private Vector3 flightDirection;

    private static Object DemonHitPrefab = null;


    GameObject FindTarget()
    {
        if (IsFlyingDemon)
        {
            return GameObject.FindGameObjectWithTag("Player");
        }
        else
        { 
            GameObject[] foundVillages = GameObject.FindGameObjectsWithTag("village");
            GameObject nearestVillage = null;
            float nearestVillageDistance = float.MaxValue;
            foreach (GameObject village in foundVillages)
            {
                float dist = Vector2.Distance(village.transform.position, transform.position);
                if (nearestVillage == null || dist < nearestVillageDistance)
                {
                    nearestVillage = village;
                    nearestVillageDistance = dist;
                }
            }
            return nearestVillage;
        }

       
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("Die");
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () 
	{
        if (DemonHitPrefab == null)
        {
            DemonHitPrefab = Resources.Load("DemonHit");
        }

		HealthBarComp = GetComponent<HealthBar> ();
		TimeTillDeath = 0;
		//numHitFX = 0;
		hit = false;
		
        TargetVillage = FindTarget();
		//target = TargetVillage.transform;
		//defaultRotation = GameObject.Find ("default_rotation").transform.rotation;
		//play sound when spawning
		AudioSource audio = GetComponent<AudioSource>();
        if (audio)
        {
            audio.Play();
        }
		//DemonDefeatedSFX.Play ();

        if (IsFlyingDemon && TimeFlyingInLastDirection != 0.0f && TimeFollowingTarget != 0.0f)
        {
            Invoke("SwitchToNoTarget", TimeFollowingTarget);
        }

        Vector3 WalkDir = Vector3.Normalize(TargetVillage.transform.position - transform.position);
		
		///rotate
        if (WalkDir != Vector3.zero)
        {
            lastFrameAngle = Mathf.Atan2(WalkDir.y, WalkDir.x) * Mathf.Rad2Deg;
        }

        lastShootTime = Time.time;
	}

    void SwitchToNoTarget()
    {
        flightDirection = (TargetVillage.transform.position - transform.position).normalized;

        TargetVillage = null;
        Invoke("SwitchToTarget", TimeFlyingInLastDirection);
    }

    void SwitchToTarget()
    {
        TargetVillage = FindTarget();
        Invoke("SwitchToNoTarget", TimeFollowingTarget);
    }
	
	// Update is called once per frame
	public void Update () 
	{
		//scale demon size
		/*
		if (HealthBarComp.Health > 0) 
		{
			/*
			HealthScale=Mathf.Max((float)HealthBarComp.Health/30.0f, 0.25f); 
			if(HealthScale <1) 
			{
				transform.localScale = new Vector3(HealthScale,HealthScale,HealthScale);
			}


		} */

		if (HealthBarComp.Health < HealthBarComp.MaxHealth * 0.8f)
		{
			if(!bSoundPlayed)
			{
				bSoundPlayed=true;
				//DemonExtinguishSFX.Play();

			}
		}
		if (HealthBarComp.Health <= 0)
		{
			Die();
			return;
		}
		//move
        Vector3 currentDir = transform.TransformDirection(Vector3.down);
        Vector3 WalkDir = TargetVillage ? Vector3.Normalize(TargetVillage.transform.position - transform.position) : currentDir;
        
        //float rotAng = Vector3.Angle(currentDir, WalkDir);
        //transform.Rotate(0, 0, Mathf.Clamp(-rotAng, -MaxRotationSpeed, MaxRotationSpeed) * Time.deltaTime);

        float angle = (Mathf.Atan2(WalkDir.y, WalkDir.x) * Mathf.Rad2Deg)+90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * RotationSpeed);

        Vector3 originalPos = transform.position;
        transform.position += currentDir * WalkingSpeed * Time.deltaTime;


		if (hit)
			HitUpdate ();

        if (ShootInterval > 0.0f && (Time.time - lastShootTime) > ShootInterval)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Vector3 vecToPlayer = player.transform.position - transform.position;
            
            if (vecToPlayer.magnitude < MaxShootDistance)
            { 
                GameObject shotObj = (GameObject)Instantiate(Resources.Load("fireshot"), transform.position, transform.rotation);
                shotObj.GetComponent<FireShotComponent>().Direction = currentDir;// vecToPlayer.normalized;
                lastShootTime = Time.time;
            }
        }
	}

	void Die()
	{
        GetComponent<CircleCollider2D>().enabled = false;
		if (TimeTillDeath == 0) {
			//play sound when dieing
			DemonDefeatedSFX.Play ();
            GetComponent<Animator>().SetBool("bPlayDeadAnim", true);
			//gameObject.spriteRenderer.enabled = false;
			//CountingForDeath=true;
			TimeTillDeath = Time.time + TimeToDisappear;
		} 
		else if (Time.time > TimeTillDeath)
		{
			GameObject.Destroy (gameObject);
            GameObject.FindGameObjectWithTag("Player").SendMessage("DemonKilled");
		}
	}
	public void Hit()
	{

		//spawn a ash particle
		DemonExtinguishSFX.Play();

        HitFX.Play();
		hit = true;
	}
	void HitUpdate()
	{



	}


}
