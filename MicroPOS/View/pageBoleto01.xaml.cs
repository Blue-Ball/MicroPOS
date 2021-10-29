using MaterialDesignThemes.Wpf;
using MicroPOS.Control;
using MicroPOS.Helper;
using MicroPOS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsInput;

namespace MicroPOS.View
{
    /// <summary>
    /// Interaction logic for pageBoleto01.xaml
    /// </summary>
    public partial class pageBoleto01 : Page
    {
		public delegate void numvaluefunctioncall(string Numvalue);
		private event numvaluefunctioncall numformFunctionPointer;

		string bankCodes = @"001,341,237,033,104,422,399,000,003,004,012,014,018,019,021,024,025,029,031,036,037,039,
							 040,041,044,045,047,062,063,064,065,066,069,070,072,073,074,075,076,077,078,079,081,082,
						     083,084,085,086,087,088,090,091,092,093,094,096,097,098,099,107,114,119,121,122,125,136,
							 168,184,204,208,212,213,214,215,217,222,224,229,230,233,241,243,246,248,249,250,254,260,
							 263,265,266,300,318,320,356,370,376,389,394,409,412,453,456,464,473,477,479,487,488,492,
							 494,495,505,600,604,610,611,612,613,623,626,630,633,634,637,638,641,643,652,653,654,655,
							 707,712,719,721,724,734,735,738,739,740,741,743,744,745,746,747,748,749,751,752,753,755,
							 756,757";

        public pageBoleto01()
        {
            InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

        private void _txtBarcode_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

			//RV: Dynamic mask based on bank codes
			string bankCode = PosHelper.GetNumberFromMaskText(_txtBarcode.Text) + e.Text;
			if (!string.IsNullOrEmpty(bankCode) && bankCode.Length == 3 && e.Text != " " && bankCodes.Contains(bankCode))
			{
				Dispatcher.BeginInvoke(new Action(() =>
				{
					BehaviorCollection behaviors = Interaction.GetBehaviors(_txtBarcode);
					var textBoxInputMaskBehavior = (TextBoxInputMaskBehavior)behaviors[0];
					textBoxInputMaskBehavior.InputMask = "00000.00000 00000.000000 00000.000000 0 000000000000000000000000";
					textBoxInputMaskBehavior.RefreshMask(bankCode);
					textBoxInputMaskBehavior.Attach(this._txtBarcode);
				}));
			}
			else if (!string.IsNullOrEmpty(bankCode) && bankCode.Length == 3 && e.Text != " " && !bankCodes.Contains(bankCode))
			{
				Dispatcher.BeginInvoke(new Action(() =>
				{
					BehaviorCollection behaviors = Interaction.GetBehaviors(_txtBarcode);
					var textBoxInputMaskBehavior = (TextBoxInputMaskBehavior)behaviors[0];
					textBoxInputMaskBehavior.InputMask = "00000000000-0 00000000000-0 00000000000-0 00000000000-0";
					textBoxInputMaskBehavior.RefreshMask(bankCode);
					textBoxInputMaskBehavior.Attach(this._txtBarcode);
				}));				
			}

		}


        // Token: 0x06000173 RID: 371 RVA: 0x0000B012 File Offset: 0x00009212
        private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this.ss_init_controls();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000B01A File Offset: 0x0000921A
		private void ss_init_controls()
		{
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
			this._txtBarcode.Focus();

			numformFunctionPointer += new numvaluefunctioncall(NumaricKeypad);
			numPad.NumaricKeypad = numformFunctionPointer;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000B044 File Offset: 0x00009244
		private void _btnInput_Click(object sender, RoutedEventArgs e)
		{
			//this._txtBarcode.Text = "0023847393272348723";
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000B058 File Offset: 0x00009258
		private void _btnCancel_Click(object sender, RoutedEventArgs e)
		{
			this._txtBarcode.Text = "";
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				mainWindow.GotoHome(true);
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000B094 File Offset: 0x00009294
		private void _btnNext_Click(object sender, RoutedEventArgs e)
		{

			string barCode = _txtBarcode.Text;
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				//RV: Remove masked chareacters
				barCode = PosHelper.GetNumberFromMaskText(_txtBarcode.Text);
				if (string.IsNullOrEmpty(barCode))
				{
					mainWindow.showDMessageBox("por favor insira o código de barras", "Informação do cartão requerida", DMessageBoxButtons.OK);
					return;
				}

				DialogOpenedEventHandler dialogOpenedEventHandler1 = null;
			
				TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
				TaskManagerForm.InvokeControlAction<UIElement>(this, async (UIElement arg) => {
				ucProcessBox _ucProcessBox = new ucProcessBox();
				_ucProcessBox._progressMsg.Text = "Geting boleto";
				DialogOpenedEventHandler u003cu003e9_1 = dialogOpenedEventHandler1;
				if (u003cu003e9_1 == null)
				{
					DialogOpenedEventHandler retObj = async (object s, DialogOpenedEventArgs args) => {
					var objBoleto = await MainWindow.RestProvider.GetBoleto(barCode);
					if (objBoleto == null)
					{
                        await Dispatcher.BeginInvoke(new Action(delegate
						{
							TaskManagerForm.InvokeControlAction<UIElement>(this, async (UIElement arg1) => {
								object obj = await DialogHost.Show(new ucDMessageBox()
								{
									Title = "Boleto",
									Message = "Boleto não encontrado",
									Buttons = 0
								}, "RootDialog", new DialogClosingEventHandler(this.closingEventHandler));
								bool flag = (bool)obj;
								Console.WriteLine(string.Concat("Dialog was closed, the CommandParameter used to close it was: ", obj.ToString()));
							});
						}));
					}
					if (!objBoleto.failed)
						mainWindow.m_naviService.Navigate(new pageBoleto02 { esteBoleto = objBoleto });
					else
					{
							
							string msg = MicroPOS.Helper.subs.getBetween(objBoleto.failedMsg, "message\":\"", "\"");

                            await Dispatcher.BeginInvoke(new Action(delegate
								{    
									TaskManagerForm.InvokeControlAction<UIElement>(this, async (UIElement arg1) => {
										object obj = await DialogHost.Show(new ucDMessageBox()
										{
											Title = "Boleto",
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
                    await DialogHost.Show(_ucProcessBox, "RootDialog", u003cu003e9_1);
				});





			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000070F4 File Offset: 0x000052F4


		private void closingEventHandler(object sender, DialogClosingEventArgs eventArgs)
		{
			Console.WriteLine("You can intercept the closing event, and cancel here.");
		}

		private void NumaricKeypad(string numValue)
		{
			_txtBarcode.Focus();
			if (numValue == "Backspace")
			{
				InputSimulator sim = new InputSimulator();
				sim.Keyboard.Sleep(2).KeyPress(WindowsInput.Native.VirtualKeyCode.BACK).Sleep(2);
			}
			else
			{
				InputSimulator sim = new InputSimulator();
				sim.Keyboard.Sleep(2).TextEntry(numValue).Sleep(2);
				// TextCompositionManager.StartComposition(new TextComposition(InputManager.Current, _txtBarcode, numValue));
			}
			_txtBarcode.Focus();
		}
	}


}
