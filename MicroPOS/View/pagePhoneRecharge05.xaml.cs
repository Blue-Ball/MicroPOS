using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using MicroPOS.Control;
using MicroPOS.Model;

namespace MicroPOS.View
{
	// Token: 0x0200003A RID: 58
	public partial class pagePhoneRecharge05 : Page
	{
		// Token: 0x0600012C RID: 300 RVA: 0x00009452 File Offset: 0x00007652
		public pagePhoneRecharge05()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00009472 File Offset: 0x00007672
		private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00009490 File Offset: 0x00007690
		private void _btnNext_Click(object sender, RoutedEventArgs e)
		{
			RechargeInfo rechargeInfo = (RechargeInfo)this._metroStackPanel.Tag;
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				if (rechargeInfo.PayMethod != 1 && rechargeInfo.PayMethod != 2)
				{
					mainWindow.showDMessageBox("Escolha a forma de pagamento.", "recarga de celular", DMessageBoxButtons.OK);
					return;
				}
				mainWindow.m_naviService.Navigate(new pagePhoneRecharge06());
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000094FC File Offset: 0x000076FC
		private void _btnCancel_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				mainWindow.GotoHome(true);
			}
		}

        private void _btnCredit_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            RechargeInfo rechargeInfo = (RechargeInfo)this._metroStackPanel.Tag;
            rechargeInfo.PayMethod = 1;
            //this._metroStackPanel.Tag = 1;
            //this._txtCredit.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0));
            //this._txtDebit.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null)
            {
                if (rechargeInfo.PayMethod != 1 && rechargeInfo.PayMethod != 2)
                {
                    mainWindow.showDMessageBox("Escolha a forma de pagamento.", "recarga de celular", DMessageBoxButtons.OK);
                    return;
                }
                mainWindow.m_naviService.Navigate(new pagePhoneRecharge06());
            }
        }

        private void _btnDebit_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RechargeInfo rechargeInfo = (RechargeInfo)this._metroStackPanel.Tag;
            rechargeInfo.PayMethod = 2;

            //this._metroStackPanel.Tag = 2;
            //this._txtCredit.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
            //this._txtDebit.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0));
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null)
            {
                if (rechargeInfo.PayMethod != 1 && rechargeInfo.PayMethod != 2)
                {
                    mainWindow.showDMessageBox("Escolha a forma de pagamento.", "recarga de celular", DMessageBoxButtons.OK);
                    return;
                }
                mainWindow.m_naviService.Navigate(new pagePhoneRecharge06());
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
