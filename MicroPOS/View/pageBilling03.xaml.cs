using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using MicroPOS.Control;
using MicroPOS.Model;
using Newtonsoft.Json.Linq;

namespace MicroPOS.View
{
	// Token: 0x02000044 RID: 68
	public partial class pageBilling03 : Page
	{
		// Token: 0x06000165 RID: 357 RVA: 0x0000AB83 File Offset: 0x00008D83
		public Model.boleto esteBoleto { get; set; }
		public string recibo { get; set; }
		public pageBilling03()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000ABA3 File Offset: 0x00008DA3
		private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000ABC4 File Offset: 0x00008DC4
		private void _btnDebit_MouseUp(object sender, RoutedEventArgs e)
		{
			this._metroStackPanel.Tag = 2;
			this._txtCredit.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
			this._txtDebit.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
			Next();
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000AC20 File Offset: 0x00008E20
		private void _btnCredit_MouseUp(object sender, RoutedEventArgs e)
		{
			this._metroStackPanel.Tag = 1;
			this._txtDebit.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
			this._txtCredit.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Black"));
			Next();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000AC7C File Offset: 0x00008E7C
		private void Next()
		{

			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				if ((int)this._metroStackPanel.Tag != 1 && (int)this._metroStackPanel.Tag != 2)
				{			
					mainWindow.showDMessageBox("Escolha a forma de pagamento.", "Rode Totem", DMessageBoxButtons.OK);
					return;
				}

				BillingInfo billingInfo = (BillingInfo)this._mainGrid.Tag;
				billingInfo.PayMethod = (int)this._metroStackPanel.Tag;
				mainWindow.m_naviService.Navigate(new pageBilling04 { esteBoleto = esteBoleto, recibo = recibo });
			}
		}

        private void __btnCancel_Click(object sender, RoutedEventArgs e)
        {
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
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
							string objBoleto = await MainWindow.RestProvider.PostBoletoCancel(esteBoleto);
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


	}
}
