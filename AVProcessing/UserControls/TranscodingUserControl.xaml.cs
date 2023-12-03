using AVProcessing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AVProcessing.UserControls
{
    /// <summary>
    /// TranscodingUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class TranscodingUserControl : UserControl
    {
        public TranscodingUserControl()
        {
            InitializeComponent();

            TranscodingViewModel viewmodel = new TranscodingViewModel();

            DataContext = viewmodel;

            viewmodel.RegisteredContent("Processing_V_UserControl");
        }

        private void StackPanel_Click(object sender, RoutedEventArgs e)
        {
            Button clickbutton = e.OriginalSource as Button;
            lowborder.Width = clickbutton.ActualWidth;

            Rect xx = VisualTreeHelper.GetDescendantBounds(clickbutton);
            Rect itembos = clickbutton.TransformToAncestor(sender as FrameworkElement).TransformBounds(xx);

            DoubleAnimation db1 = new DoubleAnimation();
            db1.To = itembos.X;
            db1.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            db1.EasingFunction = new BackEase() { Amplitude = 0.3, EasingMode = EasingMode.EaseOut };

            borderTrans.BeginAnimation(TranslateTransform.XProperty, db1);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lowborder.Width = firstButton.ActualWidth;
        }
    }
}
