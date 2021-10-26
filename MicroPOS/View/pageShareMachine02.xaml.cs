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
	// Token: 0x02000029 RID: 41
	public partial class pageShareMachine02 : Page
	{
		// Token: 0x060000C2 RID: 194 RVA: 0x00006C18 File Offset: 0x00004E18
		public pageShareMachine02()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00006C38 File Offset: 0x00004E38
		private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this.ss_init_controls();
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006C40 File Offset: 0x00004E40
		private void ss_init_controls()
		{
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
			this._txtPayAmonut.Focus();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006C6C File Offset: 0x00004E6C
		private void _btnNext_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				if (string.IsNullOrEmpty(this._txtPayAmonut.Text))
				{
					mainWindow.showDMessageBox("Digite um valor a pagar.", "Máquina Compartilhada", DMessageBoxButtons.OK);
					return;
				}
				double num = 0.0;
				if (!double.TryParse(this._txtPayAmonut.Text, out num))
				{
					mainWindow.showDMessageBox("Digite um valor a pagar.", "Máquina Compartilhada", DMessageBoxButtons.OK);
					return;
				}
				mainWindow.m_naviService.Navigate(new pageShareMachine03());
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00006CF8 File Offset: 0x00004EF8
		private void _txtPayAmonut_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9.,]+");
			e.Handled = regex.IsMatch(e.Text);
		}
	}
}
