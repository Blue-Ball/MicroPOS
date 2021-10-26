using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using MaterialDesignThemes.Wpf;
using MicroPOS.Control;
using MicroPOS.Helper;
using MicroPOS.Model;

namespace MicroPOS.View
{
	// Token: 0x02000025 RID: 37
	public partial class pageBilling05 : Page
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00005CA4 File Offset: 0x00003EA4
		public Model.boleto esteboleto { get; set; }
		public string recibo { get; set; }
		public pageBilling05()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005CC4 File Offset: 0x00003EC4
		private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this._btnCancel.IsEnabled = true;
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
			this.ss_initMetroPanel();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005CF4 File Offset: 0x00003EF4
		protected void ss_initMetroPanel()
		{
			BillingInfo billingInfo = (BillingInfo)this._mainGrid.Tag;
			if (billingInfo.PayMethod == 2)
			{
				this._valInstments.Visibility = Visibility.Hidden;
				this._infoInstments.Visibility = Visibility.Hidden;
			}
			else
			{
				this._valInstments.Visibility = Visibility.Visible;
				this._infoInstments.Visibility = Visibility.Visible;
			}
			this._valBarcode.Text = billingInfo.Barcode;
			this._valReceiver.Text = billingInfo.Receiver;
			this._valPayAmount.Text = PosHelper.GetCurrencyString(billingInfo.Amount);
			this._valTransFee.Text = PosHelper.GetCurrencyString(billingInfo.Tax);
			this._valAllAmount.Text = PosHelper.GetCurrencyString(billingInfo.TotalAmount);
			this._valPayMethod.Text = PosHelper.GetPayMethodName(billingInfo.PayMethod);
			this._valInstments.Text = billingInfo.Parcelas.ToString();
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005DE0 File Offset: 0x00003FE0
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

				//RV - Invoking PostCancel API call to cancel the transaction
				DialogOpenedEventHandler dialogOpenedEventHandler1 = null;

				TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
				TaskManagerForm.InvokeControlAction<UIElement>(this, async (UIElement arg) => {
					ucProcessBox _ucProcessBox = new ucProcessBox();
					_ucProcessBox._progressMsg.Text = "Post boleto Cancel";
					DialogOpenedEventHandler u003cu003e9_1 = dialogOpenedEventHandler1;
					if (u003cu003e9_1 == null)
					{
						DialogOpenedEventHandler retObj = async (object s, DialogOpenedEventArgs args) => {
							string objBoleto = await MainWindow.RestProvider.PostBoletoCancel(esteboleto);
							if (objBoleto.Contains("ERRO 400"))
							{
								string msg = "Http Error Retry";

								try
								{
									msg = MicroPOS.Helper.subs.getBetween(objBoleto, "message\":\"", "\"");
								}
								catch (Exception)
								{
								}

								Dispatcher.BeginInvoke(new Action(delegate
								{
									TaskManagerForm.InvokeControlAction<UIElement>(this, async (UIElement arg1) => {
										object obj = await DialogHost.Show(new ucDMessageBox()
										{
											Title = "Boleto Card",
											Message = msg,
											Buttons = 0
										}, "RootDialog", new DialogClosingEventHandler(this.closingEventHandler));
										bool flag = (bool)obj;
										Console.WriteLine(string.Concat("Dialog was closed, the CommandParameter used to close it was: ", obj.ToString()));
									});
								}));

							}

							args.Session.Close(false);
							taskCompletionSource.SetResult(true);
						};
						DialogOpenedEventHandler dialogOpenedEventHandler = retObj;
						dialogOpenedEventHandler1 = retObj;
						u003cu003e9_1 = dialogOpenedEventHandler;
					}
					DialogHost.Show(_ucProcessBox, "RootDialog", u003cu003e9_1);
				});

				mainWindow.GotoHome(true);
			}
		}

		private void closingEventHandler(object sender, DialogClosingEventArgs eventArgs)
		{
			Console.WriteLine("You can intercept the closing event, and cancel here.");
		}

		private void _btnContinue_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				BillingInfo billingInfo = (BillingInfo)this._mainGrid.Tag;
				if (mainWindow.ExecutePayBilling(this, billingInfo))
			    {
					mainWindow.SetPaymentPendingText(billingInfo.TotalAmount, billingInfo.PayMethod, 1, billingInfo.Parcelas);
					mainWindow.showAlertMessage("Rede Totem", "INSIRA SEU CARTÃO", AlertType.info, -1);
					mainWindow.GotoMainForm(false);
					return;
				}
				mainWindow.showAlertMessage("Rede Totem", "ERROR in Communication with Pinpad", AlertType.warning, -1);
			}
		}
	}
}
