using UnityEngine;
using System.Collections;

public class MoveDude : MonoBehaviour {
	public NetworkPlayer theOwner;
	void Awake ()
	{
		if (Network.isClient)
			enabled = false;
	}
	

	// Use this for initialization
	void Start () {
	
	}

	[RPC]
	public void SetPlayer (NetworkPlayer player)
	{
		theOwner = player;
		if (player == Network.player)
			enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (theOwner != null && Network.player == theOwner) {

			Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
			float speed = 5;
			transform.Translate(speed * moveDir * Time.deltaTime);
		}
	}

	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting) {
			Vector3 pos = transform.position;          
			stream.Serialize (ref pos);
		} else {
			Vector3 posReceive = Vector3.zero;
			stream.Serialize (ref posReceive);
			transform.position = posReceive;
		}
	}

}
