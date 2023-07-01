using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Sets new instance if animator
        Animator anim = GetComponent<Animator>();

        //gets current state of animation in the gameobject the script is attached to
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        //plays animation at a random time of 0 -2.5 seconds
        anim.Play(state.fullPathHash, -1, Random.Range(0.0f, 2.5f));
    }
}