// Modified from https://gist.github.com/danielbierwirth/0636650b005834204cb19ef5ae6ccedb

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPClient : MonoBehaviour {
	#region private members
	private TcpClient socketConnection;
	private Thread clientReceiveThread;
	#endregion

    private float lastShot;

	void Start () {
		ConnectToTcpServer();
		lastShot = Time.time;
	}

	/// <summary>
	/// Setup socket connection.
	/// </summary>
	private void ConnectToTcpServer () {	
		try {
			clientReceiveThread = new Thread (new ThreadStart(ListenForData));	
			clientReceiveThread.IsBackground = true;
			clientReceiveThread.Start();
		}
		catch (Exception e) {	
			Debug.Log("On client connect exception " + e);
		}
	}

	/// <summary>
	/// Runs in background clientReceiveThread; Listens for incomming data.
	/// </summary>
	private void ListenForData() {
		try {
			socketConnection = new TcpClient("192.168.162.236", 80);
			SendMessage("Connected to server\n");
			Debug.Log("Connected to client");
			Byte[] bytes = new Byte[1024];
			while (true) {
				// Get a stream object for reading	
				using (NetworkStream stream = socketConnection.GetStream()) {
					int length;
					// Read incomming stream into byte arrary.
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) {
						var incommingData = new byte[length];
						Array.Copy(bytes, 0, incommingData, 0, length);
						// Convert byte array to string message.
						string serverMessage = Encoding.ASCII.GetString(incommingData);
                        if (serverMessage.Contains("sword") && Player.Instance.currentEquipped != EQUIPPED.SWORD) {
							Debug.Log("Sword equipped");
							UnityMainThreadDispatcher.Instance().Enqueue(EquipSwordFromMainThread());
                            //Player.Instance.EquipSword();
                        } else if (serverMessage.Contains("shield") && Player.Instance.currentEquipped != EQUIPPED.SHIELD) {
							Debug.Log("Shield equipped");
							UnityMainThreadDispatcher.Instance().Enqueue(EquipShieldFromMainThread());
                            //Player.Instance.EquipShield();
                        } else if (serverMessage.Contains("crossbow") && Player.Instance.currentEquipped != EQUIPPED.CROSSBOW) {
							Debug.Log("Crossbow equipped");
							UnityMainThreadDispatcher.Instance().Enqueue(EquipCrossbowFromMainThread());
                            //Player.Instance.EquipCrossbow();
                        } else if (serverMessage.Contains("shoot")) {
							Debug.Log("Shooting");
							UnityMainThreadDispatcher.Instance().Enqueue(ShootFromMainThread());
							//Player.Instance.crossbow.GetComponent<Crossbow>().Shoot();
							//lastShot = Time.time;
						}
						//Debug.Log("server message received as: " + serverMessage);
					}
				}
			}
		}
		catch (SocketException socketException) {
			Debug.Log("Socket exception: " + socketException);
		}
	}

	private void SendMessage(string message) {
		if (socketConnection == null) {
			return;
		}
		try {
			// Get a stream object for writing.		
			NetworkStream stream = socketConnection.GetStream();
			if (stream.CanWrite) {
				string clientMessage = message;
				// Convert string message to byte array.
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
				// Write byte array to socketConnection stream.
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
				Debug.Log("Client sent message - should be received by server");
			}
		}
		catch (SocketException socketException) {
			Debug.Log("Socket exception: " + socketException);
		}
	}

	private IEnumerator EquipSwordFromMainThread()
	{
		Player.Instance.EquipSword();
		yield return null;
	}

	private IEnumerator EquipShieldFromMainThread()
	{
		Player.Instance.EquipShield();
		yield return null;
	}

	private IEnumerator EquipCrossbowFromMainThread()
	{
		Player.Instance.EquipCrossbow();
		yield return null;
	}

	private IEnumerator ShootFromMainThread()
	{
		Player.Instance.crossbow.GetComponent<Crossbow>().Shoot();
		yield return null;
	}
}
