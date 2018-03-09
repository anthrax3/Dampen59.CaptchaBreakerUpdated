using OQ.MineBot.PluginBase;
using OQ.MineBot.PluginBase.Base;
using OQ.MineBot.PluginBase.Base.Plugin;
using OQ.MineBot.PluginBase.Classes;
using OQ.MineBot.PluginBase.Classes.Base;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Dampen59CaptchaBreaker
{
    //Created by Dampen59 updated by xSlapppz |
    [Plugin(1, "Dampen59CaptchaBreaker", "AutoCaptcha Solver")]
    public class PluginCore : IStartPlugin
    {
        public static ConcurrentDictionary<ILocation, object> beingMined = new ConcurrentDictionary<ILocation, object>();
        private string ClientVersion = "1.1.1.0";
        private CancelToken stopToken = new CancelToken();
        private IPlayer player;

        public string GetName()
        {
            return "CaptchaBreaker";
        }

        public string GetDescription()
        {
            return "Auto Captcha Solver";
        }

        public string GetVersion()
        {
            return this.ClientVersion;
        }

        public new IPluginSetting[] Setting { get; set; } = new IPluginSetting[3]
        {
      (IPluginSetting) new StringSetting("Words that doesn't change between captcha requests :", "Some words used for requesting captcha", "Example : You need to enter a captcha."),
      (IPluginSetting) new StringSetting("Captcha request pattern :", "Captcha request, replace the captcha by %captcha%", "Example : You need to enter a captcha, please send %captcha% in the chat in order to connect."),
      (IPluginSetting) new StringSetting("Command used to send the captcha :", "If you need to do '/captcha 123' to send the captcha, just enter '/captcha' below. Leave blank if no command.", "")
        };

        public override void OnLoad(int version, int subversion, int buildversion)
        {
        }

        public void OnEnabled()
        {
            this.stopToken.Reset();
        }

        public void OnDisabled()
        {
        }

        public void Stop()
        {
            this.stopToken.Stop();
        }

        public IPlugin Copy()
        {
            return (IPlugin)this.MemberwiseClone();
        }

        public PluginResponse OnStart(IPlayer player)
        {
            this.player = player;
            // ISSUE: method pointer
            player.events.onChat += OnChat;
            // player.events.onChat += OnChat
            return new PluginResponse(true, "");
        }

        private void OnChat(IPlayer player, IChat message, byte position)
        {
            if (!message.Parsed.Contains((string)this.Setting[0].Get<string>()))
                return;
            Console.WriteLine("[DCaptchaBreaker] - Captcha request DETECTED. Solving.");
            string[] strArray1 = ((string)this.Setting[0].Get<string>()).Split(Convert.ToChar(" "));
            int index1 = 0;
            string[] strArray2 = strArray1;
            for (int index2 = 0; index2 < strArray2.Length && !(strArray2[index2] == "%captcha%"); ++index2)
                ++index1;
            Console.WriteLine("[DCaptchaBreaker] - Captcha found at position : " + (object)index1 + " .");
            string[] strArray3 = message.Parsed.Split(Convert.ToChar(" "));
            Console.WriteLine("[DCaptchaBreaker] - Captcha found, it should be : " + ((IEnumerable<string>)strArray3).ElementAt<string>(index1) + " . Sending captcha to server..");
            if ((string)this.Setting[2].Get<string>() == "")
            {
                Console.WriteLine("[DCaptchaBreaker] - No captcha command found. Sending.");
                player.functions.Chat(((IEnumerable<string>)strArray3).ElementAt<string>(index1));
            }
            else
            {
                Console.WriteLine("[DCaptchaBreaker] - Captcha command found. Sending captcha command + captcha.");
                player.functions.Chat((string)this.Setting[2].Get<string>() + " " + ((IEnumerable<string>)strArray3).ElementAt<string>(index1));
            }
        }
    }
}
