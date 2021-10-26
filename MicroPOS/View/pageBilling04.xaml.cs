using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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
using MicroPOS.Helper;
using MicroPOS.Model;

namespace MicroPOS.View
{
	// Token: 0x02000043 RID: 67
	public partial class pageBilling04 : Page
	{
		// Token: 0x0600015C RID: 348 RVA: 0x0000A6B4 File Offset: 0x000088B4

		public Model.boleto esteBoleto { get; set; }
		public string recibo { get; set; }
		public pageBilling04()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000A6D4 File Offset: 0x000088D4
		private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
			BillingInfo billingInfo = (BillingInfo)this._mainGrid.Tag;
			List<string> list = new List<string>();

            if (esteBoleto != null)
            {

				billingInfo.Barcode = esteBoleto.digitable;
				billingInfo.Receiver = esteBoleto.receiver;
				billingInfo.Amount = esteBoleto.total_updated;
				billingInfo.isBoleto = true;
				billingInfo.transactionID = esteBoleto.transaction_id;
				billingInfo.printoData = recibo;



				if (billingInfo.PayMethod == 1)
				{

					for (int i = 0; i < 6; i++)
					{
						string item = string.Format("{0} X {1}", i + 1, PosHelper.GetCurrencyString((double)((float)(esteBoleto.total_updated / (double)(i + 1)))));
						list.Add(item);
						if (esteBoleto.total_updated < 50)
							break;
					}
				}
				else if (billingInfo.PayMethod == 2)
				{
					string currencyString = PosHelper.GetCurrencyString(esteBoleto.total_updated);
					list.Add(currencyString);
				}
			}
            else
            {
				if (billingInfo.PayMethod == 1)
				{
					for (int i = 0; i < 6; i++)
					{
						string item = string.Format("{0} X {1}", i + 1, PosHelper.GetCurrencyString((double)((float)(billingInfo.Amount / (double)(i + 1)))));
						list.Add(item);
						if (billingInfo.Amount < 50)
							break;
					}
				}
				else if (billingInfo.PayMethod == 2)
				{
					string currencyString = PosHelper.GetCurrencyString(billingInfo.Amount);
					list.Add(currencyString);
				}
			}



			
			this.ss_initMetroPanel(list);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000A780 File Offset: 0x00008980
		protected void ss_initMetroPanel(List<string> w_listValue)
		{
			double num = 80.0;
			double num2 = 200.0;
			double num3 = 900.0;
			double num4 = 475.0;
			double num5 = 20.0;
			double num6 = 30.0;
			int num7 = w_listValue.Count<string>();
			int num8;
			int num9;
			if (num7 <= 3)
			{
				num8 = num7;
				num9 = 1;
			}
			else if (num7 < 9)
			{
				num8 = 2;
				num9 = (int)Math.Ceiling((double)num7 / 2.0);
			}
			else
			{
				num8 = 3;
				num9 = (int)Math.Ceiling((double)num7 / 3.0);
			}
			double num10 = num4 / (double)num9 - num6;
			double width = num3 / (double)num8 - num5;
			if (num10 < num)
			{
				num10 = num;
				num6 = num4 / (double)num9 - num;
			}
			if (num10 > num2)
			{
				num10 = num2;
			}
			WrapPanel wrapPanel = new WrapPanel();
			wrapPanel.Orientation = Orientation.Vertical;
			wrapPanel.VerticalAlignment = VerticalAlignment.Center;
			wrapPanel.Height = (num10 + num6) * (double)num9;
			for (int i = 0; i < w_listValue.Count; i++)
			{
				ucPriceButton ucPriceButton = new ucPriceButton();
				ucPriceButton._tileBorder.Background = new SolidColorBrush(PosHelper.ThemeBackColor);
				ucPriceButton._tileText.Text = w_listValue[i];
				ucPriceButton._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
				ucPriceButton.Tag = i + 1;
				ucPriceButton.Width = width;
				ucPriceButton.Height = num10;
				ucPriceButton.Margin = new Thickness(0.0, 0.0, num5, num6);
				ucPriceButton.MouseUp += new MouseButtonEventHandler(this.OnPriceTile_Clicked);
				wrapPanel.Children.Add(ucPriceButton);
			}
			this._metroStackPanel.Children.Add(wrapPanel);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000A94C File Offset: 0x00008B4C
		private void OnPriceTile_Clicked(object sender, RoutedEventArgs e)
		{
			ucPriceButton ucPriceButton = (ucPriceButton)sender;
			if (this.m_selButton != null)
			{
				this.m_selButton._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextColor);
			}
			ucPriceButton._tileText.Foreground = new SolidColorBrush(PosHelper.ThemeTextSelColor);
			this.m_selButton = ucPriceButton;
			this._metroStackPanel.Tag = this.m_selButton.Tag;


			BillingInfo billingInfo = (BillingInfo)this._mainGrid.Tag;

			billingInfo.Parcelas = Convert.ToInt32( this.m_selButton.Tag);

			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				if (this.m_selButton == null)
				{
					mainWindow.showDMessageBox("selecione preço de pagamento.", "Rode Totem", DMessageBoxButtons.OK);
					return;
				}
				mainWindow.m_naviService.Navigate(new pagecartao { esteboleto = esteBoleto, recibo = recibo });

			}


		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000A9B4 File Offset: 0x00008BB4
		private void _btnNext_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				if (this.m_selButton == null)
				{
					mainWindow.showDMessageBox("selecione preço de pagamento.", "Rode Totem", DMessageBoxButtons.OK);
					return;
				}
				mainWindow.m_naviService.Navigate(new pageBilling05());
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000AA08 File Offset: 0x00008C08
		private int GetBuySplitCount()
		{
			int result = 0;
			if (Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>() != null)
			{
				BillingInfo billingInfo = (BillingInfo)this._mainGrid.Tag;
				if (billingInfo.PayMethod == 1)
				{
					result = int.Parse(this.m_selButton._tileText.Text.Substring(0, 2).Trim());
				}
				else if (billingInfo.PayMethod == 2)
				{
					result = 1;
				}
			}
			return result;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000AA78 File Offset: 0x00008C78
		private int GetBuySplitPrice()
		{
			string text = this.m_selButton._tileText.Text;
			int num = text.IndexOf("R$");
			return (int)float.Parse(text.Substring(num + 2).Replace(',', '.'));
		}

		// Token: 0x040001A3 RID: 419
		private ucPriceButton m_selButton;

        private void _btnCancel_Click(object sender, RoutedEventArgs e)
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
