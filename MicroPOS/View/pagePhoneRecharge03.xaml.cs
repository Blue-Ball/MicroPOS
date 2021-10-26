using MicroPOS;
using MicroPOS.Control;
using MicroPOS.Helper;
using MicroPOS.Model;
using MicroPOS.RestAPI;
using System;
using System.CodeDom.Compiler;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace MicroPOS.View
{
    public partial class pagePhoneRecharge03 : Page
    {
        private ucSimButton m_selButton;

        public pagePhoneRecharge03()
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
                if (this._metroStackPanel.Tag == null || string.IsNullOrWhiteSpace((string)this._metroStackPanel.Tag))
                {
                    mainWindow.showDMessageBox("Escolha a Operadora que deseja realizar a recarga.", "RECARGA DE CELULAR", DMessageBoxButtons.OK);
                    return;
                }
                if (mainWindow.m_naviService != null)
                {
                    mainWindow.m_naviService.Navigate(new pagePhoneRecharge04());
                }
            }
        }

        private async Task<object> GetMobileNetworkAsync(params object[] p_params)
        {
            return await MainWindow.RestProvider.GetMobileNetwork();
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            this._metroStackPanel.Tag = "";
            this._txtPageSubject.Text = "";
            this._txtPageSubject.Visibility = System.Windows.Visibility.Hidden;
            this.ss_initMetroPanel();
        }

        private void OnSimTile_Clicked(object sender, RoutedEventArgs e)
        {
            if (this.m_selButton != null)
            {
                this.m_selButton._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
            }
            ucSimButton solidColorBrush = (ucSimButton)sender;
            solidColorBrush._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextSelColor);
            RTFee tag = (RTFee)solidColorBrush.Tag;
            this._metroStackPanel.Tag = string.Format("{0}&{1}&{2}", solidColorBrush._tileText.Text, tag.fixed_amount, tag.percentual_amount);
            this.m_selButton = solidColorBrush;

            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null)
            {
                if (this._metroStackPanel.Tag == null || string.IsNullOrWhiteSpace((string)this._metroStackPanel.Tag))
                {
                    mainWindow.showDMessageBox("Escolha a Operadora que deseja realizar a recarga.", "RECARGA DE CELULAR", DMessageBoxButtons.OK);
                    return;
                }


                RechargeInfo RcgInfo = (RechargeInfo)this._buttonPanel.Tag;

                if (_metroStackPanel.Tag != null && !string.IsNullOrEmpty(_metroStackPanel.Tag.ToString()))
                {
                    string networkType = _metroStackPanel.Tag.ToString();
                    try
                    {
                        string[] strArrays = networkType.Split(new char[] { '&' });
                        RcgInfo.NetworkType = strArrays[0];
                        double num = 0;
                        double.TryParse(strArrays[1], out num);
                        RcgInfo.FeeFixed = num;
                        num = 0;
                        double.TryParse(strArrays[2], out num);
                        RcgInfo.FeePercent = num;
                    }
                    catch
                    {
                        Console.WriteLine(string.Concat("******* ExecuteSetNetworkType : ", networkType));
                    }
                }


                if (mainWindow.m_naviService != null)
                {
                    mainWindow.m_naviService.Navigate(new pagePhoneRecharge04());
                }
            }
        }

        protected async void ss_initMetroPanel()
        {
            this._metroStackPanel.Children.Clear();
            double num = 950;
            double num1 = 420;
            double num2 = 200;
            double num3 = 450;
            int num4 = 2;
            int num5 = 2;
            double num6 = (num - num3 * (double)num5) / (double)num5;
            double num7 = (num1 - num2 * (double)num4) / (double)num4;
            WrapPanel wrapPanel = new WrapPanel()
            {
                Orientation = Orientation.Vertical,
                Height = num1
            };
            RTNetworkArray rTNetworkArray = (RTNetworkArray)await Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>().showDProgressBoxAsync("Request API..", new MainWindow.ProcessWithWaitDlg(this.GetMobileNetworkAsync), null);
            if (rTNetworkArray != null)
            {
                string[] strArrays = rTNetworkArray.network;
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    string str = strArrays[i];
                    ucSimButton _ucSimButton = new ucSimButton()
                    {
                        Tag = rTNetworkArray.fee[0]
                    };
                    _ucSimButton._tileText.Text = str;
                    _ucSimButton._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
                    _ucSimButton._tileBorder.Background = new SolidColorBrush(PosHelper.ThemeBackColor);
                    _ucSimButton.Margin = new Thickness(0, 0, num6, num7);
                    string str1 = "pack://application:,,,/MicroPOS;component/Images/ico_sim_claro.png";
                    string upper = str.ToUpper();
                    if (upper != null)
                    {
                        if (upper == "CLARO")
                        {
                            str1 = "pack://application:,,,/MicroPOS;component/Images/ico_sim_claro.png";
                        }
                        else if (upper == "OI")
                        {
                            str1 = "pack://application:,,,/MicroPOS;component/Images/ico_sim_oi.png";
                        }
                        else if (upper == "TIM")
                        {
                            str1 = "pack://application:,,,/MicroPOS;component/Images/ico_sim_tim.png";
                        }
                        else if (upper == "VIVO")
                        {
                            str1 = "pack://application:,,,/MicroPOS;component/Images/ico_sim_vivo.png";
                        }
                    }
                    _ucSimButton._tileIcon.Source = new BitmapImage(new Uri(str1));
                    _ucSimButton.MouseUp += new MouseButtonEventHandler(this.OnSimTile_Clicked);
                    wrapPanel.Children.Add(_ucSimButton);
                }
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