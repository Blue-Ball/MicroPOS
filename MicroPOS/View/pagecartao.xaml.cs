using MaterialDesignThemes.Wpf;
using MicroPOS.Control;
using MicroPOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
namespace MicroPOS.View
{
    /// <summary>
    /// Interaction logic for pagecartao.xaml
    /// </summary>
    /// 



    public partial class pagecartao : Page
    {
        public Model.boleto esteboleto { get; set; }

		public string recibo { get; set; }

		public pagecartao()
        {
            InitializeComponent();
        }

        private void _btnVisa_Click(object sender, RoutedEventArgs e)
        {
			APICall("visa");

		}

        private void _btnMaster_Click(object sender, RoutedEventArgs e)
        {
			APICall("master");

		}

        private void _btnElo_Click(object sender, RoutedEventArgs e)
        {
			APICall("elo");

		}

        private void _btnOutros_Click(object sender, RoutedEventArgs e)
        {
			APICall("outro");

		}

		public double RoundUp(double input, int places)
		{
			double multiplier = Math.Pow(10, Convert.ToDouble(places));
			return Math.Ceiling(input * multiplier) / multiplier;
		}
		void APICall(string nCard )
        {

			DialogOpenedEventHandler dialogOpenedEventHandler1 = null;

			TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
			TaskManagerForm.InvokeControlAction<UIElement>(this, async (UIElement arg) => {
				ucProcessBox _ucProcessBox = new ucProcessBox();
				_ucProcessBox._progressMsg.Text = "Geting boleto";
				DialogOpenedEventHandler u003cu003e9_1 = dialogOpenedEventHandler1;
				if (u003cu003e9_1 == null)
				{
					MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
					DialogOpenedEventHandler retObj = async (object s, DialogOpenedEventArgs args) => {

						BillingInfo billingInfo = (BillingInfo)this._mainGrid.Tag;

						string api = nCard + "/credit/" + billingInfo.Parcelas;
						if (billingInfo.PayMethod == 2)
							api = nCard + "/debit";

						Model.tarifa tarifa = await MainWindow.RestProvider.GetTarifa(api);
						if (tarifa != null) {
							billingInfo.Tax = RoundUp(tarifa.fixed_amount + (tarifa.percentual_amount / 100) * billingInfo.Amount, 2); 
							billingInfo.TotalAmount = billingInfo.Tax + billingInfo.Amount;
							mainWindow.m_naviService.Navigate(new pageBilling05 { esteboleto = esteboleto, recibo=recibo });
						}
					
						else
						{
							Dispatcher.BeginInvoke(new Action(delegate
							{
								TaskManagerForm.InvokeControlAction<UIElement>(this, async (UIElement arg1) => {
									object obj = await DialogHost.Show(new ucDMessageBox()
									{
										Title = "Boleto Card",
										Message = "Erro a ir buscar tarifa",
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


		}

        private void closingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
			Console.WriteLine("You can intercept the closing event, and cancel here.");
		}

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


		

	}
}
