using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using MicroPOS.Control;
using MicroPOS.Helper;
using WindowsInput;

namespace MicroPOS.View
{
	// Token: 0x02000042 RID: 66
	public partial class pagePhoneRecharge01 : Page
	{
		public delegate void numvaluefunctioncall(string Numvalue);
		private event numvaluefunctioncall numformFunctionPointer;

		// Token: 0x06000154 RID: 340 RVA: 0x0000A4F9 File Offset: 0x000086F9
		public pagePhoneRecharge01()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000A519 File Offset: 0x00008719
		private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this.ss_init_controls();
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000A521 File Offset: 0x00008721
		private void ss_init_controls()
		{
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
			this._txtPhoneNumber.Focus();

			numformFunctionPointer += new numvaluefunctioncall(NumaricKeypad);
			numPad.NumaricKeypad = numformFunctionPointer;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000A54C File Offset: 0x0000874C
		private void _btnCancel_Click(object sender, RoutedEventArgs e)
		{
			this._txtPhoneNumber.Text = "";
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				mainWindow.GotoHome(true);
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000A588 File Offset: 0x00008788
		private void _btnNext_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				string numberFromMaskText = PosHelper.GetNumberFromMaskText(this._txtPhoneNumber.Text);
				if (string.IsNullOrEmpty(numberFromMaskText) || numberFromMaskText.Length != 11)
				{
					mainWindow.showDMessageBox("Por favor, digite seu número de telefone", "recarga de celular", DMessageBoxButtons.OK);
					return;
				}
				mainWindow.m_naviService.Navigate(new pagePhoneRecharge02());
			}
		}

		private void NumaricKeypad(string numValue)
		{
			_txtPhoneNumber.Focus();
			if (numValue == "Backspace")
			{
				InputSimulator sim = new InputSimulator();
				sim.Keyboard.Sleep(2).KeyPress(WindowsInput.Native.VirtualKeyCode.BACK).Sleep(2);
			}
			else
			{
				InputSimulator sim = new InputSimulator();
				sim.Keyboard.Sleep(2).TextEntry(numValue).Sleep(2);
				// TextCompositionManager.StartComposition(new TextComposition(InputManager.Current, _txtPhoneNumber, numValue));
			}

			_txtPhoneNumber.Focus();
		}
	}
}
