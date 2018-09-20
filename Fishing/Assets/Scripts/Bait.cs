using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct FishCatchChance
{
	public Fish fish;
}

public class Bait : MonoBehaviour
{

	public bool BaitActive = false;
    public Text win;

    public List<FishCatchChance> TargetFishes;

    private float delay;
	
	private float interval;

	public float minDeep;
	public float maxDeep;
	public float divingSpeedDown;
	public float divingSpeedUp;

    private int counter = 0;
    void Start()
    {
        win.text = "";
    }
    // Update is called once per frame
    void Update () 
	{
        if (Input.GetKeyDown("space"))
        {
            counter = counter + 1;
        }
        if (BaitActive)
		{
            win.text = "Click space";
            if (Time.time > interval + delay)
			{
				for(int i = 0; i<TargetFishes.Count; i++)
				{

                    float rnd = Random.Range(10, 30);
                    print(counter);
                    
                    if (counter > rnd)
					{
						GameObject m_fish = Instantiate(TargetFishes[i].fish.gameObject, transform.position, transform.rotation) as GameObject;
                        win.text = "PULL NOW !!";
                        Player.Instance.UpdateStage(PlayStage.Catching);
						Player.Instance.rod.CatchedFish = m_fish.GetComponent<Fish>();
						transform.SetParent(m_fish.transform);
						BaitActive = false;
					}
				}

				delay = Random.Range(0.3f, 1.0f);
                interval = Time.time;
                //counter = 0;
			}
            
        }
	}

	public void Cast(bool cast)
	{
		if(cast)
		{
			if(transform.position.y > minDeep)
				transform.Translate(Vector3.down * divingSpeedDown * Time.deltaTime);
		}
		else
		{
			if(transform.position.y < maxDeep)
				transform.Translate(Vector3.up * divingSpeedUp * Time.deltaTime);
		}
	}
}
