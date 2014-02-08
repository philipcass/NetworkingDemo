using UnityEngine;
using System.Collections;

public class SimpleServerClient : MonoBehaviour {
	public int ServerPort = 5000;
	public string ServerIp{
		get;
		set;
	}
	

	public NetworkConnectionError StartGameServer ()
	{
		// the Unity3d way to become a server
		NetworkConnectionError init_status = Network.InitializeServer (10, ServerPort, false);
		Debug.Log ("status: " + init_status);

		this.ServerIp = Network.player.ipAddress;
		return init_status;
	}

	public void StartGameClient ()
	{
		StartCoroutine(MakeConnection ());
	}
	
	IEnumerator MakeConnection ()
	{
		// continues after we get server's address
		while (this.ServerIp == null)
			yield return new WaitForSeconds (1);
		
		while (Network.peerType == NetworkPeerType.Disconnected||Network.peerType == NetworkPeerType.Connecting)
		{
			Debug.Log ("connecting: " + this.ServerIp +":"+ this.ServerPort);
			
			// the Unity3d way to connect to a server
			NetworkConnectionError error;
			error = Network.Connect (this.ServerIp, this.ServerPort);
			
			Debug.Log ("status: " + error);
			yield return new WaitForSeconds (1);
		}
		StopCoroutine("MakeConnection");
		Debug.Log("connected!");
	}
}
