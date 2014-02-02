using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


public class UdpState
	
{
	public IPEndPoint e ;
	public UdpClient u ;
}


public class ElectDiscover: MonoBehaviour {
	
	private int server_port = 5000;
	private string server_ip;
	
	// multicast
	private int startup_port = 5100;
	private IPAddress group_address = IPAddress.Parse ("224.0.0.224");

	// Use this for initialization
	void Start () {
		StartCoroutine(SendCandidate());
		RecieveCandidates();
	}

	IEnumerator SendCandidate ()
	{
		// multicast send setup
		UdpClient udp_client = new UdpClient ();
		udp_client.JoinMulticastGroup (group_address);
		IPEndPoint remote_end = new IPEndPoint (group_address, startup_port);
		
		// sends multicast
		while (true)
		{
			byte[] buffer = Encoding.ASCII.GetBytes (SystemInfo.deviceUniqueIdentifier);
			udp_client.Send (buffer, buffer.Length, remote_end);
			yield return new WaitForSeconds (1);
		}
	}

	void RecieveCandidates ()
	{
		IPEndPoint remote_end = new IPEndPoint (IPAddress.Any, startup_port);
		UdpClient udp_client = new UdpClient (remote_end);
		udp_client.JoinMulticastGroup (group_address);

		UdpState s = new UdpState();
		s.e = remote_end;
		s.u = udp_client;

		// async callback for multicast
		udp_client.BeginReceive (new AsyncCallback (ServerLookup), s);
	}

	void ServerLookup (IAsyncResult ar)
	{
		UdpClient udp_client = (UdpClient)((UdpState)(ar.AsyncState)).u;
		IPEndPoint remote_end = (IPEndPoint)((UdpState)(ar.AsyncState)).e;
		var receiveBytes = udp_client.EndReceive (ar, ref remote_end);
		
		server_ip = remote_end.Address.ToString ();
		Debug.Log ("Server: " + System.Text.Encoding.ASCII.GetString(receiveBytes));
		udp_client.BeginReceive (new AsyncCallback (ServerLookup), (UdpState)(ar.AsyncState));

	}

}
