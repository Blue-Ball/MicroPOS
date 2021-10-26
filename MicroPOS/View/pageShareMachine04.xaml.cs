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
	// Token: 0x0200002A RID: 42
	public partial class pageShareMachine04 : Page
	{
		// Token: 0x060000C9 RID: 201 RVA: 0x00006DD3 File Offset: 0x00004FD3
		public pageShareMachine04()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00006DF3 File Offset: 0x00004FF3
		private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this._btnCancel.IsEnabled = true;
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
			this.ss_initMetroPanel();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00006E24 File Offset: 0x00005024
		protected void ss_initMetroPanel()
		{
			ShareMachineInfo shareMachineInfo = (ShareMachineInfo)this._gridConfirms.Tag;
			this._valName.Text = PosHelper.GetNameFromCPF(shareMachineInfo.CpfNumber);
			this._valCPF.Text = shareMachineInfo.CpfNumber;
			this._valPayAmount.Text = PosHelper.GetCurrencyString(shareMachineInfo.Amount);
			this._valPayMethod.Text = PosHelper.GetPayMethodName(shareMachineInfo.PayMethod);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00006E98 File Offset: 0x00005098
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

		// Token: 0x060000CD RID: 205 RVA: 0x00006ED4 File Offset: 0x000050D4
		private void _btnContinue_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				ShareMachineInfo shareMachineInfo = (ShareMachineInfo)this._gridConfirms.Tag;
				if (mainWindow.SendPayRequest())
				{
					mainWindow.SetPaymentPendingText(shareMachineInfo.Amount, shareMachineInfo.PayMethod, 1);
					mainWindow.showAlertMessage("Rede Totem", "INSIRA SEU CARTÃO", AlertType.info, -1);
					mainWindow.GotoMainForm(false);
					return;
				}
				mainWindow.showAlertMessage("Rede Totem", "ERROR in Communication with Pinpad", AlertType.warning, -1);
			}
		}
	}
}
