using System;
using prjShotMonitor.Actions;
using System.Media;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;

namespace prjShotMonitor
{
    public class CActionManager
    {
        class CActionManagerTimer : System.Timers.Timer
        {
            public string action_code { get; set; }

            public CActionManagerTimer(string p_action_code)
                : base()
            {
                action_code = p_action_code;
            }
        }

        // "AC" prefix is for "Action Code"
        // Properties.Settings.Default items
        // CShotAction.action_code
        // keys in actionList
        // keys in timerList
        public const string AC_SCREEN = "S";
        public const string AC_WEBCAM = "W";

        public OrderedDictionary actionList = new OrderedDictionary();
        public OrderedDictionary timerList = new OrderedDictionary();

        public bool PlaySound { get; set; }
        // заполнять после заполнения PlaySound
        private string _SoundLocation;
        public string SoundLocation
        {
            get { return _SoundLocation; }
            set
            {
                _SoundLocation = value;
                if (!System.IO.File.Exists(value))
                {
                    PlaySound = false;
                }
            }
        }

        public CActionManager()
        {
            PlaySound = Properties.Settings.Default.PlaySound;
            if (PlaySound)
            {
                // SoundLocation заполнять после заполнения PlaySound
                SoundLocation = Properties.Settings.Default.SoundLocation;
            }

            add2list(new CScreenShotAction(AC_SCREEN));
            add2list(new CWebcamShotAction(AC_WEBCAM));
        }

        public void Start()
        {
            foreach (DictionaryEntry timerEntry in timerList)
            {
                (timerEntry.Value as CActionManagerTimer).Start();
            }
        }

        public void Stop()
        {
            foreach (DictionaryEntry timerEntry in timerList)
            {
                (timerEntry.Value as CActionManagerTimer).Stop();
            }
        }

        public void Shot()
        {
            foreach (DictionaryEntry timerEntry in timerList)
            {
                timer_Elapsed(timerEntry.Value, null);
            }
        }

        private void add2list(CShotAction action)
        {
            // заполняем actionList и timerList
            // сначала таймер - важно (т.к. действия с таймером в set_settings())
            CActionManagerTimer timer = new CActionManagerTimer(action.action_code);
            timer.Elapsed += timer_Elapsed;
            timerList.Add(timer.action_code, timer);

            action.settings = set_settings(action.action_code);
            actionList.Add(action.action_code, action);
        }

        private void timer_Elapsed(object sender, EventArgs e)
        {
            do_action((sender as CActionManagerTimer).action_code);
        }

        private void do_action(string action_code)
        {
            if (actionList.Contains(action_code))
            {
                CShotAction action = (CShotAction)actionList[action_code];
                // фоткаем или чё там
                action.do_action();

                // играть звук
                if (PlaySound)
                {
                    SoundPlayer player = new SoundPlayer();
                    player.SoundLocation = SoundLocation;
                    player.Play();
                }
            }
        }

        private CShotActionSettings set_settings(string action_code)
        {
            // стоп таймер
            CActionManagerTimer timer = (CActionManagerTimer)timerList[action_code];
            bool b_timer_enabled_state = false;
            if (timer != null)
            {
                b_timer_enabled_state = timer.Enabled;
                if (b_timer_enabled_state) timer.Stop();
            }
            // применить настройки
            CShotActionSettings ActionSettings = new CShotActionSettings();
            PropertyInfo[] props = typeof(CShotActionSettings).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                prop.SetValue(ActionSettings, Properties.Settings.Default[prop.Name + action_code]);
            }
            // таймер, гоу
            if (timer != null)
            {
                timer.Interval = ActionSettings.TimerInterval;
                if (b_timer_enabled_state) timer.Start();
            }
            return ActionSettings;
        }
    }
}