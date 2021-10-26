using MicroPOS;
using MicroPOS.Control;
using MicroPOS.Helper;
using MicroPOS.Model;
using MicroPOS.RestAPI;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;

namespace MicroPOS.View
{
    public partial class pagePhoneRecharge04 : Page
    {
        private ucPriceButton m_selButton;

        public pagePhoneRecharge04()
        {
            this.InitializeComponent();
            base.Loaded += new RoutedEventHandler(this.OnPageLoaded);
        }

        private void _btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null)
            {
                mainWindow.GotoHome(true);
            }
        }

        private void _btnNext_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null)
            {
                if (this._metroStackPanel.Tag == null)
                {
                    mainWindow.showDMessageBox("Escolha o valor que deseja realizar a recarga", "RECARGA DE CELULAR", DMessageBoxButtons.OK);
                    return;
                }
                mainWindow.m_naviService.Navigate(new pagePhoneRecharge05());
            }
        }

        private async Task<object> GetMobileNetworkAsync(params object[] p_params)
        {
            object rechargeAmountOnMobile;
            if ((int)p_params.Length == 2)
            {
                string pParams = (string)p_params[0];
                string str = (string)p_params[1];
                rechargeAmountOnMobile = await MainWindow.RestProvider.GetRechargeAmountOnMobile(pParams, str);
            }
            else
            {
                rechargeAmountOnMobile = null;
            }
            return rechargeAmountOnMobile;
        }

        private async void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            this._txtPageSubject.Text = "";
            this._txtPageSubject.Visibility = System.Windows.Visibility.Hidden;
            this._metroStackPanel.Tag = 0;
            RechargeInfo tag = (RechargeInfo)this._buttonPanel.Tag;
            string numberFromMaskText = PosHelper.GetNumberFromMaskText(tag.PhoneNumber);
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            MainWindow.ProcessWithWaitDlg processWithWaitDlg = new MainWindow.ProcessWithWaitDlg(this.GetMobileNetworkAsync);
            object[] networkType = new object[] { tag.NetworkType, numberFromMaskText };
            List<double> nums = (List<double>)await mainWindow.showDProgressBoxAsync("Request API..", processWithWaitDlg, networkType);
            if (nums != null && nums.Count > 0)
            {
                this.ss_initMetroPanel(nums);
            }
            
         
        }

        private void OnPriceTile_Clicked(object sender, RoutedEventArgs e)
        {
            ucPriceButton solidColorBrush = (ucPriceButton)sender;
            solidColorBrush._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextSelColor);
            if (this.m_selButton != null)
            {
                this.m_selButton._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
            }
            this.m_selButton = solidColorBrush;
            this._metroStackPanel.Tag = this.m_selButton.Tag;

            RechargeInfo RcgInfo = (RechargeInfo)this._buttonPanel.Tag;

            if (_metroStackPanel.Tag != null && !string.IsNullOrEmpty(_metroStackPanel.Tag.ToString()))
            {
                try
                {
                    RcgInfo.RcgAmount = double.Parse(_metroStackPanel.Tag.ToString());
                }
                catch
                {
                }
            }

            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null)
            {
                if (this._metroStackPanel.Tag == null)
                {
                    mainWindow.showDMessageBox("Escolha o valor que deseja realizar a recarga", "RECARGA DE CELULAR", DMessageBoxButtons.OK);
                    return;
                }
                mainWindow.m_naviService.Navigate(new pagePhoneRecharge05());
            }
        }

        protected void ss_initMetroPanel(List<double> w_listValue)
        {
            this._metroStackPanel.Children.Clear();
            double num = 420;
            double num1 = 20;
            double num2 = 30;
            int num3 = 3;
            int num4 = 2;
            double num5 = num / (double)num3 - num2;
            double num6 = 950 / (double)num4 - num1;
            WrapPanel wrapPanel = new WrapPanel()
            {
                Orientation = Orientation.Vertical,
                Height = num
            };
            for (int i = 0; i < w_listValue.Count; i++)
            {
                ucPriceButton _ucPriceButton = new ucPriceButton()
                {
                    Tag = w_listValue[i].ToString()
                };
                _ucPriceButton._tileBorder.Background = new SolidColorBrush(PosHelper.ThemeBackColor);
                _ucPriceButton._tileText.Text = PosHelper.GetCurrencyString(w_listValue[i]);
                _ucPriceButton._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
                _ucPriceButton.Width = num6;
                _ucPriceButton.Height = num5;
                _ucPriceButton.Margin = new Thickness(0, 0, num1, num2);
                _ucPriceButton.MouseUp += new MouseButtonEventHandler(this.OnPriceTile_Clicked);
                wrapPanel.Children.Add(_ucPriceButton);
            }
            this._metroStackPanel.Children.Add(wrapPanel);
        }

        private void _btnBack_Click(object sender, RoutedEventArgs e)
        {

            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null && mainWindow.m_naviService != null && mainWindow.m_naviService.CanGoBack)
            {
                mainWindow.m_naviService.GoBack();
            }
        }
    }
}