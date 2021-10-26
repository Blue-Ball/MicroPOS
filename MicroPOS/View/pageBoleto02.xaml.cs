using MaterialDesignThemes.Wpf;
using MicroPOS.Control;
using MicroPOS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;

namespace MicroPOS.View
{
    /// <summary>
    /// Interaction logic for pageBoleto02.xaml
    /// </summary>
    public partial class pageBoleto02 : Page
    {
        public Model.boleto esteBoleto {get;set;}
        public pageBoleto02()
        {
            InitializeComponent();
            base.Loaded += this.OnPageLoaded;
        }

        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            this._boleto_company.Text = esteBoleto.boleto_company;
            this._digitable.Text = esteBoleto.digitable;
            this._discount_value.Text = PosHelper.GetCurrencyString(esteBoleto.discount_value).Replace(" ", string.Empty);
            this._doc_payer.Text = esteBoleto.doc_payer.ToString();
			var dateTime = DateTime.Parse(esteBoleto.due_date);
			this._due_date.Text = dateTime.ToString("dd/MM/yyyy");
            this._original_value.Text = PosHelper.GetCurrencyString(esteBoleto.original_value).Replace(" ", string.Empty);
            this._payer.Text = esteBoleto.payer;
            this._receiver.Text = esteBoleto.receiver;
            this._total_additional.Text =  PosHelper.GetCurrencyString(esteBoleto.total_additional).Replace(" ", string.Empty);
			this._total_discount.Text = PosHelper.GetCurrencyString(esteBoleto.total_discount).Replace(" ", string.Empty); 
            this._total_updated.Text = PosHelper.GetCurrencyString(esteBoleto.total_updated).Replace(" ", string.Empty); 
   
        }

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

        private void _btnContinue_Click(object sender, RoutedEventArgs e)
        {

	
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
	
				DialogOpenedEventHandler dialogOpenedEventHandler1 = null;

				TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
				TaskManagerForm.InvokeControlAction<UIElement>(this, async (UIElement arg) => {
					ucProcessBox _ucProcessBox = new ucProcessBox();
					_ucProcessBox._progressMsg.Text = "Geting boleto";
					DialogOpenedEventHandler u003cu003e9_1 = dialogOpenedEventHandler1;
					if (u003cu003e9_1 == null)
					{
						DialogOpenedEventHandler retObj = async (object s, DialogOpenedEventArgs args) => {
							string objBoleto = await MainWindow.RestProvider.PostBoletoConfirm(esteBoleto);
							if(objBoleto.Contains("ERRO 400"))
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
                            else
                            {
								string msg = MicroPOS.Helper.subs.getBetween(objBoleto, "receiptformatted\":\"", "\"");
								//Fiz apenas essa alteração - deixei comentado para nao alterar nada
								JObject jObject = JObject.Parse(objBoleto);
								string transaction_id = (string)jObject.SelectToken("transactionId");
								esteBoleto.transaction_id = Int32.Parse(transaction_id);
								mainWindow.m_naviService.Navigate(new pageBilling03 { recibo = msg, esteBoleto= esteBoleto });
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





			}
		}


		private void closingEventHandler(object sender, DialogClosingEventArgs eventArgs)
		{
			Console.WriteLine("You can intercept the closing event, and cancel here.");
		}

	}
}
