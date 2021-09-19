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

	void Start () {
		ConnectToTcpServer();
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
						Debug.Log("server message received as: " + serverMessage);
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
}
