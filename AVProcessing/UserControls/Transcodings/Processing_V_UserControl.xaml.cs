using AVProcessing.ViewModel.TranscodingsViewModel;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AVProcessing.UserControls.Transcodings
{
    /// <summary>
    /// Processing_V_UserControl.xaml 的交互逻辑
    /// </summary>
    public partial class Processing_V_UserControl : UserControl
    {
        public Processing_V_UserControl()
        {
            InitializeComponent();

            PV_ViewModel viewmodel = new PV_ViewModel();
            DataContext = viewmodel;
        }

        //滑动条转移路由
        private void listbox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;

                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }
    }
}
