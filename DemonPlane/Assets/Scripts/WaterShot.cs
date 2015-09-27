using UnityEngine;
using System.Collections;

public class WaterShot : MonoBehaviour {

	public Vector3 Direction;
	public float currentSpeed;
	public float shrink_frequency;
	private float shrink;
	private bool bLowEnoughToHit;

	// Use this for initialization
	void Start () {

		shrink = 1.0f;
		Invoke ("Sink", shrink_frequency);
		bLowEnoughToHit = false;

	}
	
	// Update is called once per frame
	void Update () {

		transform.position += Direction * currentSpeed * Time.deltaTime;

	
	}
	void Sink()
	{
		shrink -= 0.01f;

		if(shrink < 0.5) bLowEnoughToHit=true;

		if (shrink > 0)
			transform.localScale = new Vector3 (shrink, shrink, 1);
		else
			Die ();
		Invoke ("Sink", shrink_frequency);

	}
	void OnTriggerEnter2D(Collider2D collision) 
	{
		if (collision.gameObject.tag == "enemy") {
			print ("HIT");
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			int Damage =player.GetComponent<DropWater> ().WaterBulletDamage;
			HealthBar demon = collision.GetComponent<HealthBar>();
			DemonBehavior demon_behavior = collision.GetComponent<DemonBehavior>();
			demon.Health -= Damage;
			demon_behavior.Hit();


			Die ();
		}
	}



	void Die()
	{

		Destroy(gameObject);
	}
}
