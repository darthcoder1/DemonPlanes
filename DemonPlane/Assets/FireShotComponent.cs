using UnityEngine;
using System.Collections;

public class FireShotComponent : MonoBehaviour {

    public Vector3 Direction;
    
    public float Speed = 5;
    public float Lifetime = 15;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        Lifetime -= Time.deltaTime;
        if (Lifetime <= 0.0f)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += Direction * Speed * Time.deltaTime;
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("Die");
            Destroy(gameObject);
        }
    }
}
