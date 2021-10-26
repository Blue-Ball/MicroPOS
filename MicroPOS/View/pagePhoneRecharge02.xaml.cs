using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using MicroPOS.Control;
using MicroPOS.Helper;
using MicroPOS.Model;

namespace MicroPOS.View
{
	// Token: 0x02000041 RID: 65
	public partial class pagePhoneRecharge02 : Page
	{
		// Token: 0x0600014C RID: 332 RVA: 0x0000A2EA File Offset: 0x000084EA
		public pagePhoneRecharge02()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000A30A File Offset: 0x0000850A
		private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this.ss_init_controls();
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000A312 File Offset: 0x00008512
		private void ss_init_controls()
		{
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
			this._txtPhoneNumber.Focus();
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000A33C File Offset: 0x0000853C
		private void _btnCancel_Click(object sender, RoutedEventArgs e)
		{
			this._txtPhoneNumber.Text = "";
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				mainWindow.GotoHome(true);
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000A378 File Offset: 0x00008578
		private void _btnNext_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				RechargeInfo rechargeInfo = (RechargeInfo)this._mainGrid.Tag;
				if (rechargeInfo != null)
				{
					string numberFromMaskText = PosHelper.GetNumberFromMaskText(this._txtPhoneNumber.Text);
					if (string.IsNullOrEmpty(numberFromMaskText) || numberFromMaskText.Length != 11)
					{
						mainWindow.showDMessageBox("Por favor, digite seu número de telefone", "recarga de celular", DMessageBoxButtons.OK);
						return;
					}
					if (!rechargeInfo.PhoneNumber.Equals(this._txtPhoneNumber.Text))
					{
						mainWindow.showDMessageBox("O número de telefone redigitado não corresponde", "recarga de celular", DMessageBoxButtons.OK);
						return;
					}
					mainWindow.m_naviService.Navigate(new pagePhoneRecharge03());
				}
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
