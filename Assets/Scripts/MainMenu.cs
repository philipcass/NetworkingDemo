using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	private SimpleServerClient SSC;
	bool _amClient = false;

	// Use this for initialization
	void Start () {
		this.SSC = Toolbox.Instance.GetOrAddComponent<SimpleServerClient>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	string serverIP = "192.168.1.0";
	void OnGUI () {
		if(this.SSC.ServerIp == null && !_amClient){
			if (GUI.Button (new Rect (Screen.width/2-100,Screen.height/2-100,100,100), "Create Game")) {
				NetworkConnectionError status = this.SSC.StartGameServer();
				if(status == NetworkConnectionError.NoError){
					Debug.Log("Created Server!");

				}
			}
			if (GUI.Button (new Rect (Screen.width/2+100,Screen.height/2-100,100,100), "Join Game")) {
				_amClient = true;
				this.SSC.StartGameClient();
			}
		}else if(!_amClient){
			GUI.TextArea (new Rect (Screen.width/2,Screen.height/2,100,100), string.Format("Server IP: {0}", this.SSC.ServerIp));
			if (GUI.Button (new Rect (Screen.width/2-100,Screen.height/2-100,100,100), "Start Game")) {
				this.networkView.RPC("LoadLevel", RPCMode.All);
			}	
		}
		if(_amClient){
			serverIP = GUI.TextField (new Rect (Screen.width/2,Screen.height/2,100,100), serverIP);
			this.SSC.ServerIp = serverIP;

		}
	}

	[RPC]
	void LoadLevel(){
		Application.LoadLevel("arena");
	}

	void OnPlayerConnected(NetworkPlayer player)
	{
		if(Network.isServer)
			Toolbox.Instance.Players.Add(player);
	}

}
