using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using MicroPOS.Control;
using MicroPOS.Model;

namespace MicroPOS.View
{
	// Token: 0x0200002D RID: 45
	public partial class pageBuyGiftCard03 : Page
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x000074AB File Offset: 0x000056AB
		public pageBuyGiftCard03()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000074CB File Offset: 0x000056CB
		private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
			this.ss_initMetroPanel();
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000070F4 File Offset: 0x000052F4
		protected void ss_initMetroPanel()
		{
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000074F0 File Offset: 0x000056F0
		private void _btnNext_Click(object sender, RoutedEventArgs e)
		{
			GiftCardInfo giftCardInfo = (GiftCardInfo)this._mainGrid.Tag;
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				if (giftCardInfo.PayMethod != 1 && giftCardInfo.PayMethod != 2)
				{
					mainWindow.showDMessageBox("Escolha a forma de pagamento.", "recarga de celular", DMessageBoxButtons.OK);
					return;
				}
				mainWindow.m_naviService.Navigate(new pageBuyGiftCard04());
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000755C File Offset: 0x0000575C
		private void _btnCancel_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				mainWindow.GotoHome(true);
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00007588 File Offset: 0x00005788
		private void _btnDebit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            GiftCardInfo giftCardInfo = (GiftCardInfo)this._mainGrid.Tag;
            giftCardInfo.PayMethod = 2;
            //this._metroStackPanel.Tag = 2;
			//this._txtCredit.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			//this._txtDebit.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0));

            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null)
            {
                if (giftCardInfo.PayMethod != 1 && giftCardInfo.PayMethod != 2)
                {
                    mainWindow.showDMessageBox("Escolha a forma de pagamento.", "recarga de celular", DMessageBoxButtons.OK);
                    return;
                }
                mainWindow.m_naviService.Navigate(new pageBuyGiftCard04());
            }
        }

		// Token: 0x060000E6 RID: 230 RVA: 0x000075F4 File Offset: 0x000057F4
		private void _btnCredit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            GiftCardInfo giftCardInfo = (GiftCardInfo)this._mainGrid.Tag;
            giftCardInfo.PayMethod = 1;
            //this._metroStackPanel.Tag = 1;
			//this._txtCredit.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0));
			//this._txtDebit.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null)
            {
                if (giftCardInfo.PayMethod != 1 && giftCardInfo.PayMethod != 2)
                {
                    mainWindow.showDMessageBox("Escolha a forma de pagamento.", "recarga de celular", DMessageBoxButtons.OK);
                    return;
                }
                mainWindow.m_naviService.Navigate(new pageBuyGiftCard04());
            }
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
