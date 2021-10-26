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

namespace MicroPOS.View
{
    /// <summary>
    /// Interaction logic for NumPad.xaml
    /// </summary>
    public partial class NumPad : UserControl
    {
        public Delegate NumaricKeypad;

        public NumPad()
        {
            InitializeComponent();
        }

        private void btnNum_Click(object sender, RoutedEventArgs e)
        {
            string strNum = ((Button)sender).Content.ToString();
            if(NumaricKeypad != null)
                NumaricKeypad.DynamicInvoke(strNum);
        }

        private void btnBackSpace_Click(object sender, RoutedEventArgs e)
        {
            if (NumaricKeypad != null)
                NumaricKeypad.DynamicInvoke("Backspace");
        }
    }
}
