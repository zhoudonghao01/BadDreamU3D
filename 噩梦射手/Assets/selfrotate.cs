using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfrotate : MonoBehaviour {

    // Use this for initialization
    private Transform self_transform;
	void Start () {
        self_transform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        self_transform.Rotate(Vector3.up * Time.deltaTime*40, Space.Self);
	}
}
