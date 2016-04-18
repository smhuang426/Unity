using UnityEngine;
using System.Collections;
using System.Threading;

using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.IO.Ports;

public class ComPort : MonoBehaviour {

	[SerializeField]
	private int PortNumber = 0;

	[SerializeField]
	private int baudrate = 0;

	[SerializeField]
	private Parity parity;

	[SerializeField]
	private int dataBits = 0;

	[SerializeField]
	private StopBits stopBits;

	Thread _ComportThread;
	TestCom com;
	// Use this method for initialization.
	void Start () {

		com = new TestCom ();
		com.Init (PortNumber,baudrate,parity,dataBits,stopBits);

		_ComportThread = new Thread(com.ReadData);
		_ComportThread.Start ();
	}

	void OnApplicationQuit() {
		com.Stop ();
		_ComportThread.Abort ();
		_ComportThread.Join ();
	}
}


class TestCom
{
	private SerialPort sp;
	private string tempString = "";

	public void Init (int num, int baudrate, Parity type, int dataBit, StopBits stopBits) {

		sp = new SerialPort( string.Format("COM{0}",num), 
			baudrate, 
			type, 
			dataBit, 
			stopBits);

		sp.ReadTimeout = 50;
		sp.WriteTimeout = 50;

		sp.Open();
	}

	public void Stop (){
		sp.Close ();
	}

	public void ReadData(){

		while (sp.IsOpen) {
			tempString = "";

			try {
				byte tempByte = (byte)sp.ReadByte ();
				while (tempByte != 0xff) {
					tempString += ((char)tempByte);
					tempByte = (byte)sp.ReadByte ();
				}

			} catch  {

			}

			/*Recieve Data , You can do process this part when you got data*/
			if (tempString != "") {
				Debug.Log (tempString);
			}
		}
	}
}