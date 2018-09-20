using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayStage{Idle, Cast, Pull, Catching, Catched}

public class Player : MonoBehaviour 
{
	public static Player Instance;

    public Text win;
    public Text start;

    public PlayStage Stage;
	public Rod rod;

	public Marker Marker;

	void Start()
	{
		Instance = this;
        win.text = "";
        start.text = "";
    }

	void Update () 
	{
		switch(Stage)
		{
            case PlayStage.Idle:
                break;
            case PlayStage.Cast:
                start.text = "Prees R & Click right button";
                Casting();
                rod.Catching = false;
                rod.Bait.Cast(false);
                break;
            case PlayStage.Catching:
                start.text = "";
                rod.Casted = false;
                rod.Catching = true;
                break;
            case PlayStage.Catched:
                win.text = "You win!!!";
                break;
        }
	}

	public void UpdateStage(PlayStage newStage)
	{
		Stage = newStage;
	}

	void Casting()
	{
		if(Input.GetMouseButton(1))
		{
            start.text = "";
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit))
			{
				if(hit.collider.tag == "Water")
				{
					Marker.transform.position = hit.point;
				}
			}
		}
		if(Input.GetMouseButtonUp(1))
		{
			rod.Cast(Marker.transform.position);
			UpdateStage(PlayStage.Pull);
            start.text = "";
        }
	}


}
