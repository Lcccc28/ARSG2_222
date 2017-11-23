using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Game.Manager;

public class BlueToothMotor : MonoBehaviour {

	static BlueToothMotor instance;
	public const string broadcastUUID = "180d";
	public const string serviceUUID = "ae00";
	public const string characteristicUUID = "ae02";
	public static readonly byte[] saveTable = {0x5B,0x3C,0x6E,0x57,0xF3,0xA5};

	public static BlueToothMotor Instance {
		get {
			return instance;
		}
	}
	public States State {get { return state; }}

	public enum States
	{
		None,
		Init,
		Scan,
		Connect,
		Subscribe,	// 已经连上了在收数据
	}

	private float timeout = 0f;
	private States state = States.None;
	private List<String> illegalDevices = new List<String> ();
	private String deviceAddress = "";
	private byte[] addressXor = new byte[6];
	private byte lastData = 0x00;
	private float restarttime = 30f;

	void Awake () {
		instance = this;
		// 防止载入新场景时被销毁
		DontDestroyOnLoad(instance.gameObject);    
	}

	// Use this for initialization
	void Start () {
		StartProcess ();

	}

//	public GUISkin skin;
//
//	void OnGUI(){
//		GUI.skin = skin;
//		GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
//		myButtonStyle.fontSize = 20;
//		GUI.Button (new Rect (10, 0, 300, 100), string.Format ("sta:{0}  id:{1}", State, deviceAddress), myButtonStyle);
//		GUI.Button (new Rect (10, 0, 300, 200), string.Format ("xor:{0}", BitConverter.ToString(addressXor)), myButtonStyle);
//		GUI.Button (new Rect (10, 0, 300, 300), string.Format ("data:{0}", lastData.ToString()), myButtonStyle);
//		GUI.Button (new Rect (10, 0, 300, 400), string.Format ("timeout:{0}", timeout), myButtonStyle);
//	}

	
	// Update is called once per frame
	void Update () {
		if (state != States.Subscribe){
			if (restarttime <= Time.realtimeSinceStartup) {
				Disconnect (deviceAddress);
			}
		}

		if (timeout != 0 && timeout <= Time.realtimeSinceStartup) {
			timeout = 0;
			switch (state) {
			case States.None:
				break;
			case States.Init:
				StartProcess ();
				break;
			case States.Scan:
				Time.timeScale = 0;
				Alert.Show("Can't fine the ArGun, Scaning...", null, false);
				// TODO 这里暂停事件
				BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (new string[]{BluetoothLEHardwareInterface.FullUUID(broadcastUUID)}, (_address, _name) => {
					if (!illegalDevices.Contains (_address)) {
						BluetoothLEHardwareInterface.StopScan ();
						deviceAddress = _address;
						Alert.Show("Connectting the ar gun : "+_name, null, false);
						SetState (States.Connect, 0.2f);
					}
				});
				break;

			case States.Connect:
				BluetoothLEHardwareInterface.ConnectToPeripheral (deviceAddress, null, null, (_address, _serviceUUID, _characteristicUUID) => {
					if (IsEqual (serviceUUID, _serviceUUID) && IsEqual (characteristicUUID, _characteristicUUID)) {
#if UNITY_ANDROID
						string[] addressArr = _address.Split (':');
						for (int i = 0; i < addressArr.Length; i++) {
							addressXor [i] = (byte)(Convert.ToByte (addressArr [i], 16) ^ saveTable [i]);
						}
#endif
						Alert.Show("connect success!", (_value) => { SetState (States.Subscribe, 1f); });
					}
				}, (_address) => {
                    SetState(States.Scan, 0.5f);
				});
				break;

			case States.Subscribe:
				// TODO 这里恢复事件
				Time.timeScale = 1f;
				BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (deviceAddress, serviceUUID, characteristicUUID, null, (_address, _characteristicUUID, _bytes) => {
					if (_bytes.Length == addressXor.Length + 1) {
#if UNITY_ANDROID
						for (int i = 0; i < addressXor.Length; i++) {
							if (!_bytes [i].Equals (addressXor [i])) {
								Disconnect (_address);
								return;
							}
						}
#endif
						ReadData (_bytes [addressXor.Length]);
					} else {
						Disconnect (_address);
					}
				});
				break;
			}
		}

	}
		
	void StartProcess ()
	{
		Alert.Show("Bluetooth initialization will fail if Bluetooth settings are not initialized", null, false);
		BluetoothLEHardwareInterface.Initialize (true, false, () => {
			SetState (States.Scan, 0.2f);
		}, (error) => {
			// Console.Write ("蓝牙初始化失败: " + error + "，等待重试");
			Alert.Show("Initialize Faild, Please open the Bluetooth", (_value) => {
                BluetoothLEHardwareInterface.DeInitialize(null);
                SetState (States.Init, 0.2f);
            });
		});
	}

	void Disconnect(string address){
		BluetoothLEHardwareInterface.UnSubscribeCharacteristic (address, serviceUUID, characteristicUUID, null);
		BluetoothLEHardwareInterface.DisconnectPeripheral(address, null);
        // BluetoothLEHardwareInterface.DeInitialize(null);
        // illegalDevices.Add(address);
        SetState(States.Scan, 0.5f);
    }

	void SetState (States newState, float newtimeout)
	{
		state = newState;
		timeout = Time.realtimeSinceStartup + newtimeout;
        restarttime = Time.realtimeSinceStartup + 30f;
    }

	bool IsEqual(string uuid1, string uuid2)
	{
		if (uuid1.Length == 4)
			uuid1 = BluetoothLEHardwareInterface.FullUUID (uuid1);
		if (uuid2.Length == 4)
			uuid2 = BluetoothLEHardwareInterface.FullUUID (uuid2);

		return (uuid1.ToUpper().CompareTo(uuid2.ToUpper()) == 0);
	}

	void ReadData(byte bytes){
		int index0 = GetBitValue (bytes, lastData, 0);
		switch (index0) {
		case 0:
			KeyInputManager.Instance.BtnAUp();
			break;
		case 1:
			KeyInputManager.Instance.BtnADown();
			break;
		default:
			break;
		}

		int index1 = GetBitValue (bytes, lastData, 1);
		switch (index1) {
		case 1:
            KeyInputManager.Instance.BtnCDown();        
			break;
		default:
			break;
		}

		int index2 = GetBitValue (bytes, lastData, 2);
		switch (index2) {
		case 1:
            KeyInputManager.Instance.BtnBDown();
            break;
		default:
			break;
		}
		lastData = bytes;

	}

	public static int GetBitValue(byte value, byte lastData, ushort index)
	{
		if (index > 7) throw new ArgumentOutOfRangeException("index"); //索引出错
		int i = (value >> index) & 1;
		int lasti = (lastData >> index) & 1;
		if (i == lasti)
			return -1;
		else
			return i;
	}

}
