using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using MicroPOS.Helper;
using MicroPOS.Model;

namespace MicroPOS.View
{
	// Token: 0x02000045 RID: 69
	public partial class pageBilling02 : Page
	{
		// Token: 0x0600016C RID: 364 RVA: 0x0000AE0A File Offset: 0x0000900A
		public pageBilling02()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000AE2C File Offset: 0x0000902C
		private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
			BillingInfo billingInfo = (BillingInfo)this._mainGrid.Tag;
			this._txtReciever.Text = billingInfo.Receiver;
			this._txtAmount.Text = PosHelper.GetCurrencyString(billingInfo.Amount);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000AE90 File Offset: 0x00009090
		private void _btnNo_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null && mainWindow.m_naviService != null && mainWindow.m_naviService.CanGoBack)
			{
				mainWindow.m_naviService.GoBack();
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000AED8 File Offset: 0x000090D8
		private void _btnYes_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				mainWindow.m_naviService.Navigate(new pageBilling03());
			}
		}
	}
}
