using System;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace UwpCamButton
{
    class Clock : INotifyPropertyChanged
    {
        public Clock()
        {
            //každou vteřinu systém změní čas a dojde k notifikaci změny
            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += tick;
            timer.Start();
        }

        public DateTime? StartTime { get; set; }

        public DateTime RelativeTime
        {
            get
            {
                if (!StartTime.HasValue) return DateTime.Now;

                DateTime RelTime = StartTime.Value;
                RelTime.AddMinutes(-StartTime.Value.Minute);
                RelTime.AddSeconds(-StartTime.Value.Second);

                return RelTime;
            }
        }

        /// <summary>
        /// Zobrazí aktuální čas v předdefinovaném formátu.
        /// Výstupní hodnota, je string.
        /// </summary>
        public string CurentRealTime
        {
            get
            {
                if (!StartTime.HasValue) return DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");

                DateTime RelTime = StartTime.Value;
                RelTime.AddMinutes(-StartTime.Value.Minute);
                RelTime.AddSeconds(-StartTime.Value.Second);

                return RelTime.ToString("yyyy.MM.dd HH:mm:ss");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged ;
        //každou vteřinu systém změní čas a dojde k notifikaci změny
        private void tick(object sender, object e)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("CurentRelTime"));
        }
    }
}
