using MicroPOS;
using MicroPOS.Control;
using MicroPOS.Helper;
using MicroPOS.Model;
using MicroPOS.RestAPI;
using NLog;
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
    public partial class pageBuyGiftCard02 : Page
    {
        private ucPriceButton m_selButton;

        public pageBuyGiftCard02()
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
                if (this.m_selButton == null)
                {
                    mainWindow.showDMessageBox("por favor selecione preço", "COMPRAR GIFT CARD", DMessageBoxButtons.OK);
                    return;
                }
                if (mainWindow.m_naviService != null)
                {
                    mainWindow.m_naviService.Navigate(new pageBuyGiftCard03());
                }
            }
        }

        private async Task<object> GetGiftCardAmountAsync(params object[] p_params)
        {
            object giftCardAmount;
            if (p_params.Length != 0)
            {
                string pParams = (string)p_params[0];
                giftCardAmount = await MainWindow.RestProvider.GetGiftCardAmount(pParams);
            }
            else
            {
                giftCardAmount = null;
            }
            return giftCardAmount;
        }

        private async void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            this._txtPageSubject.Text = "";
            this._txtPageSubject.Visibility = System.Windows.Visibility.Hidden;
            GiftCardInfo tag = (GiftCardInfo)this._mainGrid.Tag;
            PosHelper.Logger.Info(string.Format("GiftCardInfo : {0}", tag));
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            List<double> nums = null;
            MainWindow.ProcessWithWaitDlg processWithWaitDlg = new MainWindow.ProcessWithWaitDlg(this.GetGiftCardAmountAsync);
            object[] cardType = new object[] { tag.CardType };
            nums = (List<double>)await mainWindow.showDProgressBoxAsync("Request API...", processWithWaitDlg, cardType);
            Console.WriteLine(string.Format("GetGiftCardAmount Count (2) {0}", nums.Count));
            if (nums != null && nums.Count > 0)
            {
                Console.WriteLine("Call ss_initMetroPanel");
                this.ss_initMetroPanel(nums);
            }
        }

        private void OnPriceTile_Clicked(object sender, RoutedEventArgs e)
        {
            ucPriceButton solidColorBrush = (ucPriceButton)sender;
            if (this.m_selButton != null)
            {
                this.m_selButton._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
            }
            solidColorBrush._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextSelColor);
            this.m_selButton = solidColorBrush;
            this._metroStackPanel.Tag = this.m_selButton.Tag;

            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();


            if (mainWindow != null)
            {
                if (this.m_selButton == null)
                {
                    mainWindow.showDMessageBox("por favor selecione preço", "COMPRAR GIFT CARD", DMessageBoxButtons.OK);
                    return;
                }
                var _cardInfo = _buttonPanel.Tag as GiftCardInfo;
                var p_amount = this._metroStackPanel.Tag.ToString();
                if (!string.IsNullOrEmpty(p_amount))
                {
                    try
                    {
                        _cardInfo.Amount = double.Parse(p_amount);
                    }
                    catch
                    {
                    }
                }

                if (mainWindow.m_naviService != null)
                {
                    mainWindow.m_naviService.Navigate(new pageBuyGiftCard03());
                }
            }
        }

        protected void ss_initMetroPanel(List<double> w_listValue)
        {
            this._metroStackPanel.Children.Clear();
            double num = 80;
            double num1 = 200;
            double num2 = 475;
            double num3 = 20;
            double num4 = 30;
            int num5 = 3;
            int num6 = 2;
            int num7 = w_listValue.Count<double>();
            if (num7 <= 3)
            {
                num6 = num7;
                num5 = 1;
            }
            else if (num7 >= 9)
            {
                num6 = 3;
                num5 = (int)Math.Ceiling((double)num7 / 3);
            }
            else
            {
                num6 = 2;
                num5 = (int)Math.Ceiling((double)num7 / 2);
            }
            double num8 = num2 / (double)num5 - num4;
            double num9 = 1045 / (double)num6 - num3;
            if (num8 < num)
            {
                num8 = num;
                num4 = num2 / (double)num5 - num;
            }
            if (num8 > num1)
            {
                num8 = num1;
            }
            WrapPanel wrapPanel = new WrapPanel()
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Height = (num8 + num4) * (double)num5
            };
            for (int i = 0; i < w_listValue.Count; i++)
            {
                ucPriceButton _ucPriceButton = new ucPriceButton();
                _ucPriceButton._tileBorder.Background = new SolidColorBrush(PosHelper.ThemeBackColor);
                _ucPriceButton._tileText.Text = PosHelper.GetCurrencyString(w_listValue[i]);
                _ucPriceButton._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
                _ucPriceButton.Tag = w_listValue[i].ToString();
                _ucPriceButton.Width = num9;
                _ucPriceButton.Height = num8;
                _ucPriceButton.Margin = new Thickness(0, 0, num3, num4);
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