using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireCell : MonoBehaviour
{
    private bool bIsBurning;
    public int CurrentHealth;
    private float BurningCounter;
    private List<GameObject> TrackedDemons;

    public int MaxHealth = 250;
    public float StartBurningThreshold = 2.0f;

    public static int BurningCells = 0;
	public AudioSource StartedBurningSFX;
	public AudioSource StoppedBurningSFX;

    private CircleCollider2D CollisionComp;
    private ParticleSystem FireFXComp;
    private SpriteRenderer SpriteComp;

    public bool IsBurning
    {
        get { return bIsBurning; }
        set
        {
            bIsBurning = value;
            FireFXComp.enableEmission = bIsBurning;
            SpriteComp.enabled = bIsBurning;

            if (bIsBurning)
            {
                ++BurningCells;
            }
            else
            {
                BurningCells = Mathf.Max(BurningCells-1, 0);
            }
        }
    }


	// Use this for initialization
	void Start ()
    {
        CollisionComp = GetComponent<CircleCollider2D>();
        FireFXComp = GetComponent<ParticleSystem>();
        SpriteComp = GetComponent<SpriteRenderer>();

        TrackedDemons = new List<GameObject>();

        IsBurning = false;
        CurrentHealth = MaxHealth;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!IsBurning & CurrentHealth > 0)
        {
            UpdateTrackedDemons();
            UpdateSpreading();

            if (BurningCounter > StartBurningThreshold)
            {
                IsBurning = true;
				StartedBurningSFX.Play();
            }
        }
        else
        {
            if (CurrentHealth <= 0)
            {
                IsBurning = false;
                Invoke("ResetHealth", 10.0f);
            }
        }
	}

    void ResetHealth()
    {
        if (!IsBurning)
        {
            CurrentHealth = MaxHealth;
			StoppedBurningSFX.Play ();
        }
        else
        {
            Invoke("ResetHealth", 10.0f);
        }
    }

    void UpdateSpreading()
    {
        if (!gameObject || !gameObject.transform || !gameObject.transform.parent) { return; }
        GameObject Parent = gameObject.transform.parent.gameObject;

        float nearestCellDist = float.MaxValue;
        FireCell nearestCell = null;
        for (int i=0; i< Parent.transform.childCount;++i)
        {
            FireCell cell = Parent.transform.GetChild(i).gameObject.GetComponent<FireCell>();

            if (cell != this)
            {
                float magnitude = (cell.transform.position - transform.position).magnitude;
                if (magnitude < nearestCellDist)
                {
                    nearestCellDist = magnitude;
                    nearestCell = cell;
                }
            }
        }

        if (nearestCell && nearestCell.IsBurning)
        {
            BurningCounter += Time.deltaTime;
        }
    }
    void UpdateTrackedDemons()
    {
        foreach(GameObject obj in TrackedDemons)
        {
            BurningCounter += Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            TrackedDemons.Add(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            TrackedDemons.Remove(collision.gameObject);
        }
    }
}
