using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using MicroPOS.Control;

namespace MicroPOS.View
{
	// Token: 0x02000046 RID: 70
	public partial class pageBilling01 : Page
	{
		// Token: 0x06000172 RID: 370 RVA: 0x0000AFF2 File Offset: 0x000091F2
		public pageBilling01()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
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
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000B044 File Offset: 0x00009244
		private void _btnInput_Click(object sender, RoutedEventArgs e)
		{
			this._txtBarcode.Text = "0023847393272348723";
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
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				if (string.IsNullOrEmpty(this._txtBarcode.Text))
				{
					mainWindow.showDMessageBox("por favor insira o código de barras", "Informação do cartão requerida", DMessageBoxButtons.OK);
					return;
				}
				mainWindow.m_naviService.Navigate(new pageBilling02());
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000070F4 File Offset: 0x000052F4
		private void _txtBarcode_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}
	}
}
