using AVProcessing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AVProcessing.UserControls
{
    /// <summary>
    /// ScreeningUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ScreeningUserControl : UserControl
    {
        Timer movieTimer = new Timer();
        public ScreeningUserControl()
        {
            InitializeComponent();

            ScreeningViewModel viewmodel = new ScreeningViewModel();

            DataContext = viewmodel;
        }

        private void playMovie_MediaOpened(object sender, RoutedEventArgs e)
        {
            movieTimer.Interval = 1000;
            movieTimer.Elapsed += MovieTimerEvent;
            movieTimer.Start();
            movieNDTime.Text = playMovie.NaturalDuration.TimeSpan.ToString();
        }

        private void MovieTimerEvent(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                moviePositionTime.Text = playMovie.Position.ToString(@"hh\:mm\:ss");

                LyricSlider.Value = MovieTImeToSliderValue(playMovie.NaturalDuration.TimeSpan, playMovie.Position);
            });
        }

        public int MovieTImeToSliderValue(TimeSpan endTime, TimeSpan positionTime)
        {
            int EndallSecounds = endTime.Hours * 3600 + endTime.Minutes * 60 + endTime.Seconds;

            int positionAllSecounds = positionTime.Hours * 3600 + positionTime.Minutes * 60 + positionTime.Seconds;

            return System.Convert.ToInt32(System.Convert.ToInt32(LyricSlider.Maximum) * positionAllSecounds / EndallSecounds);
        }

        public TimeSpan SliderValueToMovieTIme(int value, int maximum)
        {
            TimeSpan endTime = playMovie.NaturalDuration.TimeSpan;

            int endTimeAllSecounds = endTime.Hours * 3600 + endTime.Minutes * 60 + endTime.Seconds;

            int allSecounds = endTimeAllSecounds * value / maximum;

            TimeSpan newTimespan = new TimeSpan(0, 0, allSecounds);

            return newTimespan;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void LyricSlider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            playMovie.Position = SliderValueToMovieTIme(Convert.ToInt32(LyricSlider.Value), Convert.ToInt32(LyricSlider.Maximum));
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            playMovie.Pause();
            Panel.SetZIndex(stopButton, 1);
            stopButton.Opacity = 0;
            Panel.SetZIndex(startButton, 2);
            startButton.Opacity = 1;
            movieTimer.Stop();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            playMovie.Play();
            Panel.SetZIndex(stopButton, 2);
            stopButton.Opacity = 1;
            Panel.SetZIndex(startButton, 1);
            startButton.Opacity = 0;
            movieTimer.Start();
        }
    }
}
