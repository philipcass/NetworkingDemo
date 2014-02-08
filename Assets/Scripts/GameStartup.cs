using UnityEngine;
using System.Collections;

public class GameStartup : MonoBehaviour {

	void Start(){
		if(Network.isServer){
			AddPlayer(Network.player);
			foreach(NetworkPlayer player in Toolbox.Instance.Players){
				this.networkView.RPC("AddPlayer", player, player);
			}
		}
	}

	[RPC]
	void AddPlayer(NetworkPlayer player){
		string tempPlayerString = player.ToString();
		int playerNumber = int.Parse(tempPlayerString);
		
		GameObject newPlayerTransform = (GameObject)Network.Instantiate(Resources.Load("Player"), Vector3.zero, transform.rotation, playerNumber);

		NetworkView theNetworkView = newPlayerTransform.GetComponent<MoveDude>().networkView;
		theNetworkView.RPC("SetPlayer", RPCMode.AllBuffered, player);
	}
}
