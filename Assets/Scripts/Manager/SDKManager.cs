using BasicScript;
using UnityEngine;
using Game.Core;
using Game.Const;
using System.Collections;
using System.Collections.Generic;
using cn.sharesdk.unity3d;

/// <summary>
/// Author: NicolasTse
/// Email: xiehaojiejob@qq.com
/// </summary>
namespace Game.Manager {

    public class SDKManager : Singleton<SDKManager> {

        public ShareSDK ssdk;

        public SDKManager() {
        }

        public void InitSDK(ShareSDK sdk) { 
            ssdk = sdk;
            //ssdk.authHandler = OnAuthResultHandler;
            //ssdk.shareHandler = OnShareResultHandler;
            //ssdk.showUserHandler = OnGetUserInfoResultHandler;
            //ssdk.getFriendsHandler = OnGetFriendsResultHandler;
            //ssdk.followFriendHandler = OnFollowFriendResultHandler;
        }

        public void Share() { 
            ShareContent content = new ShareContent();  

            //这个地方要参考不同平台需要的参数    可以看ShareSDK提供的   分享内容参数表.docx  
            content.SetText("快来和我一起玩这个游戏吧！");                            //分享文字  
            content.SetImageUrl("https://f1.webshare.mob.com/code/demo/img/4.jpg");   //分享图片  
            content.SetTitle("标题title");                                            //分享标题  
            content.SetTitleUrl("http://www.qq.com");  
            content.SetSite("Mob-ShareSDK");  
            content.SetSiteUrl("http://www.mob.com");  
            content.SetUrl("http://www.sina.com");                                    //分享网址  
            content.SetComment("描述");  
            content.SetMusicUrl("http://up.mcyt.net/md5/53/OTg1NjA5OQ_Qq4329912.mp3");//分享类型为音乐时用  
            content.SetShareType(ContentType.Webpage);  
  
            ssdk.ShowPlatformList(null, content, 100, 100);                      //弹出分享菜单选择列表  
            //ssdk.ShowShareContentEditor(PlatformType.WeChat, content);                 //指定平台直接分
        }

        // 分享结果回调  
        void ShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result) {   
            ////成功  
            //if (state == ResponseState.Success)  
            //{  
            //    message.text =("share result :");  
            //    message.text = (MiniJSON.jsonEncode(result));   
            //}  
            ////失败  
            //else if (state == ResponseState.Fail)  
            //{  
            //    message.text = ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);  
            //}  
            ////关闭  
            //else if (state == ResponseState.Cancel)   
            //{  
            //    message.text = ("cancel !");  
            //}  
        }  
    }
}