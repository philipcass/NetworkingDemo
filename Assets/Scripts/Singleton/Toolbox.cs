using UnityEngine;
using System.Collections.Generic;

public class Toolbox : Singleton<Toolbox> {
	public List<NetworkPlayer> Players{
		get;
		protected set;
	}
	protected Toolbox () {} // guarantee this will be always a singleton only - can't use the constructor!
	
	void Awake () {
		DontDestroyOnLoad(this);
		this.Players = new List<NetworkPlayer>();
		this.GetOrAddComponent<SimpleServerClient>();
	}
	
	// (optional) allow runtime registration of global objects
	static public UnityEngine.Component RegisterComponent<Component> () {
		return Instance.GetOrAddComponent<UnityEngine.Component>();
	} 
}
