using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using MicroPOS.Helper;
using MicroPOS.Model;

namespace MicroPOS.View
{
	// Token: 0x02000028 RID: 40
	public partial class pagePhoneRecharge06 : Page
	{
		// Token: 0x060000BB RID: 187 RVA: 0x000068C0 File Offset: 0x00004AC0
		public pagePhoneRecharge06()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000068E0 File Offset: 0x00004AE0
		private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this._btnCancel.IsEnabled = true;
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
			this.ss_initMetroPanel();
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00006910 File Offset: 0x00004B10
		protected void ss_initMetroPanel()
		{
			RechargeInfo rechargeInfo = (RechargeInfo)this._gridConfirms.Tag;
			this._valPhoneNumb.Text = rechargeInfo.PhoneNumber;
			this._valMobileOpt.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(rechargeInfo.NetworkType.ToLower());
			this._valPayAmount.Text = PosHelper.GetCurrencyString(rechargeInfo.RcgAmount);
			this._valTransfFee.Text = PosHelper.GetCurrencyString(rechargeInfo.Tax);
			this._valAllAmount.Text = PosHelper.GetCurrencyString(rechargeInfo.TotalAmount);
			this._valPayMethod.Text = PosHelper.GetPayMethodName(rechargeInfo.PayMethod);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000069BC File Offset: 0x00004BBC
		private void _btnCancel_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				if (mainWindow.IsPendingState())
				{
					mainWindow.AbortPayment();
					return;
				}
				mainWindow.GotoHome(true);
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000069F8 File Offset: 0x00004BF8
		private void _btnContinue_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				RechargeInfo rechargeInfo = (RechargeInfo)this._gridConfirms.Tag;
				if (mainWindow.ExecuteRechargeMobile(this, rechargeInfo))
				{
					mainWindow.SetPaymentPendingText(rechargeInfo.TotalAmount, rechargeInfo.PayMethod, 1);
					mainWindow.showAlertMessage("Rede Totem", "INSIRA SEU CARTÃO", AlertType.info, -1);
					mainWindow.GotoMainForm(false);
					return;
				}
				mainWindow.showAlertMessage("Rede Totem", "ERROR in Communication with Pinpad", AlertType.warning, -1);
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
