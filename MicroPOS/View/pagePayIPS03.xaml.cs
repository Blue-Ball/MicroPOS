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
	// Token: 0x02000035 RID: 53
	public partial class pagePayIPS03 : Page
	{
		// Token: 0x0600010A RID: 266 RVA: 0x000087A1 File Offset: 0x000069A1
		public pagePayIPS03()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000087C1 File Offset: 0x000069C1
		private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
			this.ss_initMetroPanel();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000070F4 File Offset: 0x000052F4
		protected void ss_initMetroPanel()
		{
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000087E8 File Offset: 0x000069E8
		private void _btnDebit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IpsInfo ipsInfo = (IpsInfo)this._mainGrid.Tag;
            ipsInfo.PayMethod = 2;
            this._metroStackPanel.Tag = 2;
			//this._txtCredit.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
			//this._txtDebit.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0));
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null)
            {
                if (ipsInfo.PayMethod != 1 && ipsInfo.PayMethod != 2)
                {
                    mainWindow.showDMessageBox("Escolha a forma de pagamento.", "recarga de celular", DMessageBoxButtons.OK);
                    return;
                }
                mainWindow.m_naviService.Navigate(new pagePayIPS04());
            }
        }

		// Token: 0x0600010E RID: 270 RVA: 0x00008854 File Offset: 0x00006A54
		private void _btnCredit_MouseUp(object sender, MouseButtonEventArgs e)
		{
            IpsInfo ipsInfo = (IpsInfo)this._mainGrid.Tag;
            ipsInfo.PayMethod = 1;
            this._metroStackPanel.Tag = 1;
			//this._txtCredit.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0));
			//this._txtDebit.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
            
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null)
            {
                if (ipsInfo.PayMethod != 1 && ipsInfo.PayMethod != 2)
                {
                    mainWindow.showDMessageBox("Escolha a forma de pagamento.", "recarga de celular", DMessageBoxButtons.OK);
                    return;
                }
                mainWindow.m_naviService.Navigate(new pagePayIPS04());
            }
        }

		// Token: 0x0600010F RID: 271 RVA: 0x000088C0 File Offset: 0x00006AC0
		private void _btnNext_Click(object sender, RoutedEventArgs e)
		{
			IpsInfo ipsInfo = (IpsInfo)this._mainGrid.Tag;
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				if (ipsInfo.PayMethod != 1 && ipsInfo.PayMethod != 2)
				{
					mainWindow.showDMessageBox("Escolha a forma de pagamento.", "recarga de celular", DMessageBoxButtons.OK);
					return;
				}
				mainWindow.m_naviService.Navigate(new pagePayIPS04());
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000892C File Offset: 0x00006B2C
		private void _btnCancel_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				mainWindow.GotoHome(true);
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
