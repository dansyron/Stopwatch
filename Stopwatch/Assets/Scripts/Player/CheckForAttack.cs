using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckForAttack : MonoBehaviour {

    BoxCollider targetField;

    //timer refill trail
    public GameObject refillTrail;

    void Awake()
    {
        GameManager.Instance.Attack = this;
    }

	// Use this for initialization
	void Start ()
    {
        targetField = GetComponent<BoxCollider>();
        targetField.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Toggles the box collider componet
    /// </summary>
    public void ToggleCollider()
    {
        targetField.enabled = !targetField.enabled;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            //create refill Trail
            Instantiate(refillTrail, transform.position, new Quaternion(0,0,0,0));
            //refillTrail.GetComponent<TimeRefill>().setStart(transform.position);

            //destroy enemy
            Destroy(col.gameObject);
            GameManager.Instance.Timer.ChangeTime(3f);

            AudioManager.instance.PlaySingle(SoundEffect.SuccessfulStab);
            AudioManager.instance.PlaySingle(SoundEffect.TimeIncrement);
        }
    }
}
