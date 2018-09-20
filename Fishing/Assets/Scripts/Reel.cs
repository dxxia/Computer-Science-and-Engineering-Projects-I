using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reel : MonoBehaviour
{

    public Animator anim_reel;
    // Use this for initialization
    void Start()
    {
        anim_reel = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            anim_reel.Play("rotate");
        }
    }
}
