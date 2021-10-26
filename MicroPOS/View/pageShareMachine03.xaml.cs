using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using MicroPOS.Control;
using MicroPOS.Model;

namespace MicroPOS.View
{
	// Token: 0x0200002B RID: 43
	public partial class pageShareMachine03 : Page
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x000070B0 File Offset: 0x000052B0
		public pageShareMachine03()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000070D0 File Offset: 0x000052D0
		private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
			this.ss_initMetroPanel();
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000070F4 File Offset: 0x000052F4
		protected void ss_initMetroPanel()
		{
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000070F8 File Offset: 0x000052F8
		private void _btnNext_Click(object sender, RoutedEventArgs e)
		{
			ShareMachineInfo shareMachineInfo = (ShareMachineInfo)this._metroStackPanel.Tag;
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				if (shareMachineInfo.PayMethod != 1 && shareMachineInfo.PayMethod != 2)
				{
					mainWindow.showDMessageBox("Escolha a forma de pagamento.", "recarga de celular", DMessageBoxButtons.OK);
					return;
				}
				mainWindow.m_naviService.Navigate(new pageShareMachine04());
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00007164 File Offset: 0x00005364
		private void _btnCancel_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				mainWindow.GotoHome(true);
			}
		}
	}
}
