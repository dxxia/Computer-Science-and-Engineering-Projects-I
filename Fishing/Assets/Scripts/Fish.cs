using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fish : MonoBehaviour 
{
	public Transform pointer;

    public Text win;

	public Transform PlayerPos;

	public float CurrentSpeed;

	public float RotationSpeed;

	public float ChangeDirMinStep;
	public float ChangeDirMaxStep;
	public float CurrentDirStep;

	private float m_changeDirStepTemp;

	public bool cathcing = false;
    private int strength = 2;

	// Use this for initialization
	void Start () 
	{
		PlayerPos = Player.Instance.transform;
		pointer.SetParent(null);
        if (win != null)
            win.text = "fuck why????????";
    }
	
	// Update is called once per frame
	void Update ()
	{
		float distanceToPlayer = Vector3.Distance(transform.position, PlayerPos.position);
		if(cathcing)
		{
            if (Input.GetKeyDown("up"))
                strength = strength + 1;
            if (Input.GetKeyDown("down"))
            {
                strength = strength - 1;
                if (strength <= 2)
                    strength = 2;
            }
            Catching(strength);
        }
		else
		{
			transform.Translate(Vector3.forward * Time.deltaTime * CurrentSpeed);

			var targetRotation = Quaternion.LookRotation(transform.position - pointer.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
			if(Time.time > m_changeDirStepTemp + CurrentDirStep)
			{
				var heading = transform.position + PlayerPos.position;
				var distance = heading.magnitude;
				var direction = heading / distance; // This is now the normalized direction.
				Vector3 pos = direction + Random.insideUnitSphere * -distance;
				pos.y = PlayerPos.position.y;
				pointer.position = pos;
				CurrentDirStep = Random.Range(ChangeDirMinStep, ChangeDirMaxStep);
				m_changeDirStepTemp = Time.time;
			}
		}
		if(distanceToPlayer < 0.1f)
		{
			transform.GetChild(1).SetParent(null);
			//Player.Instance.UpdateStage(PlayStage.Cast);
            //Player.Instance.Spinning.Catching = false;
            Player.Instance.UpdateStage(PlayStage.Catched);
            Destroy(gameObject);
		}
	
	}

	public void Catching(float power)
	{
		if(!PlayerPos)
			return;
		transform.Translate(Vector3.forward * Time.deltaTime * power);

		var targetRotation = Quaternion.LookRotation(PlayerPos.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
		if(Time.time > m_changeDirStepTemp + CurrentDirStep)
		{
			var heading = transform.position + PlayerPos.position;
			var distance = heading.magnitude;
			var direction = heading / distance; // This is now the normalized direction.
			Vector3 pos = direction + Random.insideUnitSphere * -distance;
			pos.y = PlayerPos.position.y;
			pointer.position = pos;
			CurrentDirStep = Random.Range(ChangeDirMinStep, ChangeDirMaxStep);
			m_changeDirStepTemp = Time.time;
		}
	}
}
