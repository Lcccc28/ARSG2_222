using UnityEngine;
using System.Collections.Generic;
using System;

public class TestScript2 : MonoBehaviour
{
	public GUISkin skin;

	private byte[] data;
	private BluetoothDeviceScript bluetoothDeviceScript;

    private string deviceUUID = "";
    private Dictionary<string, string> devices = new Dictionary<string, string>();
	private Dictionary<string, List<string>> serviceUUIDs = new Dictionary<string, List<string>>();
	private string selectServiceUUID = "";
	private string selectCharacteristicUUID = "";
	private string recvName = "";
	private string recvData = "";
	private string sendData = "";


	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnGUI ()
	{
		
		GUI.skin = skin;
		GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
		myButtonStyle.fontSize = 20;

        if (bluetoothDeviceScript != null && bluetoothDeviceScript.DiscoveredDeviceList != null && devices.Count > 0) {
            int i = 0;
            foreach (KeyValuePair<string, string> kv in devices)
            {
				if (GUI.Button(new Rect(310, 100 * (i + 1), 300, 100), string.Format("{0} {1}", kv.Key, kv.Value), myButtonStyle))
                {
                    if (deviceUUID != "")
                        BluetoothLEHardwareInterface.DisconnectPeripheral(deviceUUID, null);

                    deviceUUID = kv.Key;
                    serviceUUIDs.Clear();
                    BluetoothLEHardwareInterface.StopScan();
                    BluetoothLEHardwareInterface.ConnectToPeripheral(kv.Key, null, null
                        , (address, serviceUUID, characteristicUUID) =>
                        {
							if (!serviceUUIDs.ContainsKey(serviceUUID)){
								List<string> charList = new List<string> {characteristicUUID};
								serviceUUIDs.Add(serviceUUID, charList);
							}else {
								serviceUUIDs[serviceUUID].Add(characteristicUUID);
							}
                        }
                    );
                }
                i++;
            }
        }

		int j = 0;
		foreach (KeyValuePair<string, List<string>> kv in serviceUUIDs)
		{
			foreach (string c in kv.Value) {
				if (GUI.Button(new Rect(610, 100 * (j + 1), 1200, 100), string.Format("srv:{0} char:{1}", kv.Key, c), myButtonStyle))
				{
					selectServiceUUID = kv.Key;
					selectCharacteristicUUID = c;
				}
				j++;
			}

		}

		if (GUI.Button (new Rect (10, 0, 300, 50), "DeInitialize", myButtonStyle))
			BluetoothLEHardwareInterface.DeInitialize (null);
		
		if (GUI.Button (new Rect (10, 50, 300, 100), "Initialize Central", myButtonStyle))
			bluetoothDeviceScript = BluetoothLEHardwareInterface.Initialize (true, false, null, null);

		if (GUI.Button(new Rect(10, 150, 300, 100), "Scan for Any", myButtonStyle))
        {
            devices.Clear();
            serviceUUIDs.Clear();
            BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
            {
                devices[address] = name;
            });
        }

//		if (GUI.Button (new Rect (10, 250, 300, 50), "Stop Scan"))
//			BluetoothLEHardwareInterface.StopScan ();

		if (GUI.Button (new Rect (10, 250, 300, 100), BitConverter.ToString(new byte[]{1,2,10})+"Read Characteristic", myButtonStyle) && bluetoothDeviceScript != null && bluetoothDeviceScript.DiscoveredDeviceList != null && bluetoothDeviceScript.DiscoveredDeviceList.Count > 0)
			BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (deviceUUID, selectServiceUUID, selectCharacteristicUUID, null, (address, characteristicUUID, data) =>
				{
					recvName = address;
					recvData = BitConverter.ToString(data);
				}
			);
		GUI.TextArea(new Rect (10, 350, 300, 100), recvName + "\n" + recvData);


		sendData = GUI.TextField (new Rect (10, 550, 300, 100), sendData, 64);
		if (GUI.Button (new Rect (10, 450, 300, 100), "Write Characteristic", myButtonStyle) && bluetoothDeviceScript != null && bluetoothDeviceScript.DiscoveredDeviceList != null && bluetoothDeviceScript.DiscoveredDeviceList.Count > 0)
		{
			if (sendData == null) {
				data = new byte[64];
				for (int i = 0; i < 64; ++i)
					data [i] = (byte)i;
			} else {
				data = System.Text.Encoding.Default.GetBytes (sendData);
			}

			BluetoothLEHardwareInterface.WriteCharacteristic (deviceUUID, selectServiceUUID, selectCharacteristicUUID, data, data.Length, true, null);
		}

	}
}
