using UnityEngine;
using System.Collections;

public class MoveComponent : MonoBehaviour {

    public float Speed = 10.0f;
    public float ResetDistance = 200.0f;

    public Vector2 Direction = new Vector2(1, 0);

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.position += new Vector3(Direction.x, Direction.y, 0.0f).normalized * Speed * Time.deltaTime;

        Vector3 pos = transform.position;

        if (pos.x > ResetDistance)
        {
            pos.x = -ResetDistance;
        }
        else if (pos.x < -ResetDistance)
        {
            pos.x = ResetDistance;
        }

        if (pos.y > ResetDistance)
        {
            pos.y = -ResetDistance;
        }
        else if (pos.y < -ResetDistance)
        {
            pos.y = ResetDistance;
        }

        transform.position = pos;
	}
}
