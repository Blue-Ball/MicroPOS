using MicroPOS;
using MicroPOS.Control;
using MicroPOS.Helper;
using MicroPOS.Model;
using MicroPOS.RestAPI;
using System;
using System.CodeDom.Compiler;
using System.Collections;
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
    public partial class pageBuyGiftCard01 : Page
    {
        public pageBuyGiftCard01()
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
                    mainWindow.showDMessageBox("selecione o tipo de cartão-presente", "COMPRAR GIFT CARD", DMessageBoxButtons.OK);
                    return;
                }
                if (mainWindow.m_naviService != null)
                {
                    mainWindow.m_naviService.Navigate(new pageBuyGiftCard02());
                }
            }
        }

        private async Task<object> GetGiftCardAsync(params object[] p_params)
        {
            return await MainWindow.RestProvider.GetGiftCard();
        }

        private void GiftCard_MouseUp(object sender, MouseButtonEventArgs e)
        {
            foreach (object child in this._metroStackPanel.Children)
            {
                ((ucGiftCard)child)._cardName.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
            }
            ucGiftCard solidColorBrush = (ucGiftCard)sender;
            solidColorBrush._cardName.Foreground = new SolidColorBrush(PosHelper.ThemeTextSelColor);
            RTFee tag = (RTFee)solidColorBrush.Tag;
            this._metroStackPanel.Tag = string.Format("{0}&{1}&{2}", solidColorBrush._cardName.Text, tag.fixed_amount, tag.percentual_amount);


            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null)
            {
                if (this._metroStackPanel.Tag == null || string.IsNullOrWhiteSpace((string)this._metroStackPanel.Tag))
                {
                    mainWindow.showDMessageBox("selecione o tipo de cartão-presente", "COMPRAR GIFT CARD", DMessageBoxButtons.OK);
                    return;
                }
                var _cardInfo = _buttonPanel.Tag as GiftCardInfo;
                _cardInfo.PayMethod = 0;
                string p_cardType = this._metroStackPanel.Tag.ToString();
                PosHelper.Logger.Info(string.Concat("ExcuteSetCardType : ", p_cardType));
                if (string.IsNullOrEmpty(p_cardType))
                {
                    return;
                }
                try
                {
                    string[] strArrays = p_cardType.Split(new char[] { '&' });
                    _cardInfo.CardType = strArrays[0];
                    double num = 0;
                    double.TryParse(strArrays[1], out num);
                    _cardInfo.FeeFixed = num;
                    num = 0;
                    double.TryParse(strArrays[2], out num);
                    _cardInfo.FeePercent = num;
                }
                catch
                {
                    Console.WriteLine(string.Concat("******* ExecuteSetNetworkType : ", p_cardType));
                }


                if (mainWindow.m_naviService != null)
                {
                    mainWindow.m_naviService.Navigate(new pageBuyGiftCard02());
                }
            }
        }

        private async void InitGiftCards()
        {
            this._metroStackPanel.Children.Clear();
            RTNetworkArray rTNetworkArray = (RTNetworkArray)await Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>().showDProgressBoxAsync("Request API..", new MainWindow.ProcessWithWaitDlg(this.GetGiftCardAsync), null);
            if (rTNetworkArray != null)
            {
                string[] strArrays = rTNetworkArray.network;
                if (strArrays != null)
                {
                    for (int i = 0; i < (int)strArrays.Length; i++)
                    {
                        string str = strArrays[i];
                        ucGiftCard _ucGiftCard = new ucGiftCard()
                        {
                            Tag = rTNetworkArray.fee[0]
                        };
                        _ucGiftCard._cardName.Text = str;
                        _ucGiftCard._cardName.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
                        _ucGiftCard._cardName.FontSize = 15;
                        string str1 = "pack://application:,,,/MicroPOS;component/Images/ico_gift_uber.png";
                        string upper = str.ToUpper();
                        if (upper != null)
                        {
                            if (upper == "NETFLIX")
                            {
                                str1 = "pack://application:,,,/MicroPOS;component/Images/ico_gift_netflix.png";
                            }
                            else if (upper == "SPOTIFY")
                            {
                                str1 = "pack://application:,,,/MicroPOS;component/Images/ico_gift_spotify.png";
                            }
                            else if (upper == "STEAM")
                            {
                                str1 = "pack://application:,,,/MicroPOS;component/Images/ico_gift_steam.png";
                            }
                            else if (upper == "UBER")
                            {
                                str1 = "pack://application:,,,/MicroPOS;component/Images/ico_gift_uber.png";
                            }
                            else if (upper == "XBOX")
                            {
                                str1 = "pack://application:,,,/MicroPOS;component/Images/ico_gift_xbox.png";
                            }
                            //RV - Add new gift card GG CREDITS FREE FIRE
                            else if (upper == "GG CREDITS FREE FIRE")
                            {
                                str1 = "pack://application:,,,/MicroPOS;component/Images/ico_gg_credits_free_fire.png";
                            }
                        }
                        _ucGiftCard._cardImage.Source = new BitmapImage(new Uri(str1));
                        _ucGiftCard.MouseUp += new MouseButtonEventHandler(this.GiftCard_MouseUp);
                        this._metroStackPanel.Children.Add(_ucGiftCard);
                    }
                }
            }
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            this.InitGiftCards();
            this._txtPageSubject.Text = "";
            this._txtPageSubject.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}