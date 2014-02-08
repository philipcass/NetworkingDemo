using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Network.InitializeServer(100,51234,false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
