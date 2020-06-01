using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using AppVoice.Models;
using Android.Media;
using System.Threading.Tasks;
using Android.Content.Res;
using System.IO;
using Xamarin.Essentials;
using System.Timers;

namespace AppVoice
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        // Declare ihm elements
        private TextView TvMessage;
        private Permission Permission;
        private Button BtVibrate;
        private Button BtVoice1;
        private Button BtVoice2;
        private Button BtVoiceOff;
        private Vibrate Vibrate;


        //Declare globals
        private int Count;
        private int VoiceChoice;
        private int CountShake;
        private int CountSequences;
        private bool DroppedPhoneTable;
        private bool DroppedPhoneStatus;
        private bool Reset;


        // Declare sensors
        private AccelerometerReader AccelerometerReader;
        private GyroscopeReader GyroscopeReader;
        private OrientationReader OrientationReader;

        // Declare media player
        private MediaPlayer MediaPlayer;
        private bool MediaPlayerReading;

        // Declare timeCounters
        private Timer BaseTimerCounter;
        private Timer WalkTimerCounter;
        private Timer RollTimerCounter;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Set sensors instances
            this.AccelerometerReader = new AccelerometerReader();
            this.GyroscopeReader = new GyroscopeReader();
            this.OrientationReader = new OrientationReader();
            this.Vibrate = new Vibrate();

            // Init media player
            this.MediaPlayer = new MediaPlayer();
            this.MediaPlayerReading = false;

            // Find ihm selectors
            this.TvMessage = FindViewById<TextView>(Resource.Id.tv_message);
            this.BtVoice1 = FindViewById<Button>(Resource.Id.bt_voice_1);
            this.BtVoice2 = FindViewById<Button>(Resource.Id.bt_voice_2);
            this.BtVoiceOff = FindViewById<Button>(Resource.Id.bt_voice_off);
            this.BtVibrate = FindViewById<Button>(Resource.Id.bt_vibrate);

            this.Count = 0;
            this.CountSequences = 0;
            this.VoiceChoice = 0;
            this.CountShake = 0;
            this.DroppedPhoneTable = false;
            this.DroppedPhoneStatus = false;
            this.Reset = false;
            
            

            // Callbacks
            this.BtVibrate.Click += this.OnClick_TestVibrate;
            this.BtVoice1.Click += this.OnClick_Voice1;
            this.BtVoice2.Click += this.OnClick_Voice2
                ;
            this.BtVoiceOff.Click += this.OnClick_VoiceOff;

            if (!this.AccelerometerReader.Started)
            {
                this.AccelerometerReader.ToggleAccelerometer();
            }
        }

        protected override void OnRestart()
        {
            base.OnRestart();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            this.Permission = new Permission();
            this.Permission.RequestAsync();
            Console.WriteLine("PERMISSIONS APP" + this.Permission.CheckStatusAsync());

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void StartPlayer(int file)
        {
            this.MediaPlayer.Reset();
            var fd = global::Android.App.Application.Context.Resources.OpenRawResourceFd(file);
            this.MediaPlayer.Prepared += (s, e) =>
            {
                this.MediaPlayer.Start();
            };

            this.MediaPlayer.Completion += (s, e) =>
            {
            };

            this.MediaPlayer.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
            this.MediaPlayer.Prepare();
            this.MediaPlayer.Start();
            this.MediaPlayerReading = true;
        }

        public async void OnClick_Voice1(object sender, System.EventArgs e)
        {
            this.TvMessage.Text = "Vous avez selectionné la voix 1";
            this.VoiceChoice = 1;
            this.InitSequence();

        }

        public void OnClick_Voice2(object sender, System.EventArgs e)
        {
            this.TvMessage.Text = "Vous avez selectionné la voix 2";
            this.VoiceChoice = 2;
            this.InitSequence();
            
        }

        public void OnClick_VoiceOff(object sender, System.EventArgs e)
        {

            this.TvMessage.Text = "Choisir la voix 1 ou 2 pour commencer";
            this.VoiceChoice = 0;
            this.Reset = false;
            this.DroppedPhoneTable = false;
            this.DroppedPhoneStatus = false;
            this.InitSequence();
        }
        public void OnClick_TestVibrate(object sender, System.EventArgs e)
        {
            this.Count++;
            if (this.Count <= 1)
            {
                this.Vibrate.Start();
                this.BtVibrate.Text = "Annuler";

            } else
            {
                this.Count = 0;
                this.Vibrate.Stop();
                this.BtVibrate.Text = "Test Vibration";
            }
         
        }


        private void InitSequence()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                
                if (!this.AccelerometerReader.Started)
                {
                    this.AccelerometerReader.ToggleAccelerometer();
                }

                if (!this.GyroscopeReader.Started)
                {
                    this.GyroscopeReader.ToggleGyroscope();
                }

                if (!this.OrientationReader.Started)
                {
                    this.OrientationReader.ToggleOrientationSensor();
                }

                if (CountSequences == 0)
                {
                    this.TvMessage.Text = "Posez rapidement votre smartphone";
                    Accelerometer.ReadingChanged += this.FirstSequence;
                }

                if (CountSequences == 1)
                {
                    this.TvMessage.Text = "Je suis la";
                    OrientationSensor.ReadingChanged += this.SecondSequence;
                }
            });
        }

        private void FirstSequence(object sender, AccelerometerChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Drop the phone on table
                if (this.AccelerometerReader.AccZ > 1.5 && !this.DroppedPhoneTable && !this.DroppedPhoneStatus && !this.MediaPlayerReading)
                {
                    this.TvMessage.Text = "Reprenez votre Smartphone";
                    this.DroppedPhoneTable = true;
                }

                // Take the phone and begin 
                if (!this.DroppedPhoneStatus && this.DroppedPhoneTable && this.AccelerometerReader.AccZ < 0 && !this.MediaPlayerReading && !this.Reset)
                {
                    this.TvMessage.Text = "Salut !";
                    this.DroppedPhoneStatus = true;
                    this.DroppedPhoneTable = false;
                    this.StartPlayer(Resource.Raw.Voice01_01);
                    this.CountSequences++;
                    this.InitSequence();
                }

                // Drop again phone to stop and reset
                
                if (this.AccelerometerReader.AccZ > 1.5 && this.DroppedPhoneStatus && !this.Reset && !this.AccelerometerReader.Shaked)
                {
                    this.TvMessage.Text = "A Bientôt";
                    this.DroppedPhoneTable = true;
                    this.Reset = true;
                    if (this.DroppedPhoneStatus && this.DroppedPhoneTable && this.VoiceChoice == 1 && this.Reset)
                    {  
                        this.StartPlayer(Resource.Raw.Voice01_09);
                    }

                    if (this.DroppedPhoneStatus && this.DroppedPhoneTable && this.VoiceChoice == 2 && this.Reset)
                    {
               
                        //this.StartPlayer(Resource.Raw.Voice02_09);
                    }

                    this.CountSequences = 0;
                }
            });
        }

        private void SecondSequence(object sender, OrientationSensorChangedEventArgs e)
        {
            Console.WriteLine(e.Reading.Orientation + "fffffff");
        }

        private void ThirdSequence()
        {
            
        }

        private void FourthSequence()
        {
            
        }

        private void FithSequence()
        {
            
        }

        private void SixSequence()
        {
            
        }

        private void SevenSequence()
        {
            
        }

        private void EightSequence()
        {
           
        }
    }
}