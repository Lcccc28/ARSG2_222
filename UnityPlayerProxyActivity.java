package com.sykj.arsg2;

import android.app.Activity;
import android.bluetooth.BluetoothAdapter;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.widget.Toast;

/**
 * @deprecated Use UnityPlayerActivity instead.
 */
public class UnityPlayerProxyActivity extends Activity
{
	@Override protected void onCreate (Bundle savedInstanceState)
	{
		Log.w("Unity", "UnityPlayerNativeActivity has been deprecated, please update your AndroidManifest to use UnityPlayerActivity instead");
		super.onCreate(savedInstanceState);
/*
		Intent intent = new Intent(this, com.DefaultCompany.ARSG.UnityPlayerActivity.class);
		intent.addFlags(Intent.FLAG_ACTIVITY_NO_ANIMATION);
		Bundle extras = getIntent().getExtras();
		if (extras != null)
			intent.putExtras(extras);
		startActivity(intent);
*/
		
        BluetoothAdapter mBluetoothAdapter = BluetoothAdapter  
                .getDefaultAdapter();  
        if (mBluetoothAdapter == null) {  
            Toast.makeText(this, "本机没有找到蓝牙硬件或驱动！", Toast.LENGTH_SHORT).show();  
            finish();  
        }  
        // 如果本地蓝牙没有开启，则开启  
        if (!mBluetoothAdapter.isEnabled()) {  
            // 我们通过startActivityForResult()方法发起的Intent将会在onActivityResult()回调方法中获取用户的选择，比如用户单击了Yes开启，  
            // 那么将会收到RESULT_OK的结果，  
            // 如果RESULT_CANCELED则代表用户不愿意开启蓝牙  
            Intent mIntent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);  
            startActivityForResult(mIntent, 1);  
            // 用enable()方法来开启，无需询问用户(实惠无声息的开启蓝牙设备),这时就需要用到android.permission.BLUETOOTH_ADMIN权限。  
            // mBluetoothAdapter.enable();  
            // mBluetoothAdapter.disable();//关闭蓝牙  
        } else {
        	StartGame();
        }
	}
	
	private void StartGame() {
		Intent intent = new Intent(this, com.sykj.arsg2.UnityPlayerActivity.class);
		intent.addFlags(Intent.FLAG_ACTIVITY_NO_ANIMATION);
		Bundle extras = getIntent().getExtras();
		if (extras != null)
			intent.putExtras(extras);
		startActivity(intent);
	}
	
	@Override  
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {  
        // TODO Auto-generated method stub  
        super.onActivityResult(requestCode, resultCode, data);  
        if (requestCode == 1) {  
            if (resultCode == RESULT_OK) {  
            	
                Toast.makeText(this, "蓝牙已经开启", Toast.LENGTH_SHORT).show();  
            } else if (resultCode == RESULT_CANCELED) {  
                Toast.makeText(this, "不允许蓝牙开启", Toast.LENGTH_SHORT).show();  
                finish();  
            }  
        }  
        StartGame();
    }  
}
