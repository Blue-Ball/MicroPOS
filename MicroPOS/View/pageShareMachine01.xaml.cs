using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using MicroPOS.Control;
using MicroPOS.Helper;

namespace MicroPOS.View
{
	// Token: 0x0200002C RID: 44
	public partial class pageShareMachine01 : Page
	{
		// Token: 0x060000D7 RID: 215 RVA: 0x000072A8 File Offset: 0x000054A8
		public pageShareMachine01()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000072C8 File Offset: 0x000054C8
		private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this.ss_init_controls();
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x000072D0 File Offset: 0x000054D0
		private void ss_init_controls()
		{
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
			this._txtCPF.Focus();
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000072FA File Offset: 0x000054FA
		private void _btnInput_Click(object sender, RoutedEventArgs e)
		{
			this._txtCPF.Text = "123.456.789-00";
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000730C File Offset: 0x0000550C
		private void _btnCancel_Click(object sender, RoutedEventArgs e)
		{
			this._txtCPF.Text = "";
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				mainWindow.GotoHome(true);
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00007348 File Offset: 0x00005548
		private void _btnNext_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				string numberFromMaskText = PosHelper.GetNumberFromMaskText(this._txtCPF.Text);
				if (string.IsNullOrEmpty(numberFromMaskText) || numberFromMaskText.Length != 11)
				{
					mainWindow.showDMessageBox("digite o CPF do destinatário.", "Máquina Compartilhada", DMessageBoxButtons.OK);
					return;
				}
				mainWindow.m_naviService.Navigate(new pageShareMachine02());
			}
		}
	}
}
