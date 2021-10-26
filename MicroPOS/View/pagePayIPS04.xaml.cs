using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using MicroPOS.Helper;
using MicroPOS.Model;

namespace MicroPOS.View
{
	// Token: 0x02000034 RID: 52
	public partial class pagePayIPS04 : Page
	{
		// Token: 0x06000103 RID: 259 RVA: 0x0000843E File Offset: 0x0000663E
		public pagePayIPS04()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000845E File Offset: 0x0000665E
		private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this._btnCancel.IsEnabled = true;
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
			this.ss_initMetroPanel();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00008490 File Offset: 0x00006690
		protected void ss_initMetroPanel()
		{
			IpsInfo ipsInfo = (IpsInfo)this._mainGrid.Tag;
			this._valName.Text = ipsInfo.Invoice.nome_razao;
			this._valCPF.Text = ipsInfo.Invoice.doc;
			this._valPayAmount.Text = PosHelper.GetCurrencyString(ipsInfo.Amount);
			this._valTax.Text = PosHelper.GetCurrencyString(ipsInfo.Tax);
			this._valTotalAmount.Text = PosHelper.GetCurrencyString(ipsInfo.TotalAmount);
			this._valPayMethod.Text = PosHelper.GetPayMethodName(ipsInfo.PayMethod);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00008534 File Offset: 0x00006734
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

		// Token: 0x06000107 RID: 263 RVA: 0x00008570 File Offset: 0x00006770
		private void _btnContinue_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				IpsInfo ipsInfo = (IpsInfo)this._mainGrid.Tag;
				if (mainWindow.ExecutePayInternet(this, ipsInfo))
				{
					mainWindow.SetPaymentPendingText(ipsInfo.TotalAmount, ipsInfo.PayMethod, 1);
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
