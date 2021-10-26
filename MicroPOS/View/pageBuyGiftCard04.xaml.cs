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
	// Token: 0x02000026 RID: 38
	public partial class pageBuyGiftCard04 : Page
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x00006073 File Offset: 0x00004273
		public pageBuyGiftCard04()
		{
			this.InitializeComponent();
			base.Loaded += this.OnPageLoaded;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00006093 File Offset: 0x00004293
		private void OnPageLoaded(object sender, RoutedEventArgs e)
		{
			this._btnCancel.IsEnabled = true;
			this._txtPageSubject.Text = "";
			this._txtPageSubject.Visibility = Visibility.Hidden;
			this.ss_initMetroPanel();
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000060C4 File Offset: 0x000042C4
		protected void ss_initMetroPanel()
		{
			GiftCardInfo giftCardInfo = (GiftCardInfo)this._mainGrid.Tag;
			this._valSrevcName.Text = giftCardInfo.CardType;
			this._valPayAmount.Text = PosHelper.GetCurrencyString(giftCardInfo.Amount);
			this._valTransfFee.Text = PosHelper.GetCurrencyString(giftCardInfo.Tax);
			this._valAllAmount.Text = PosHelper.GetCurrencyString(giftCardInfo.TotalAmount);
			this._valPayMethod.Text = PosHelper.GetPayMethodName(giftCardInfo.PayMethod);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000614C File Offset: 0x0000434C
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
				mainWindow.GotoHome(true);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00006188 File Offset: 0x00004388
		private void _btnContinue_Click(object sender, RoutedEventArgs e)
		{
			try
			{

			MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
			if (mainWindow != null)
			{
				GiftCardInfo giftCardInfo = (GiftCardInfo)this._mainGrid.Tag;
				if (mainWindow.ExecuteRechargeGiftCard(this, giftCardInfo))
				{
					mainWindow.SetPaymentPendingText(giftCardInfo.TotalAmount, giftCardInfo.PayMethod, 1);
					mainWindow.showAlertMessage("Rede Totem", "INSIRA SEU CARTÃO", AlertType.info, -1);
					mainWindow.GotoMainForm(false);
					return;
				}
				mainWindow.showAlertMessage("Rede Totem", "ERROR in Communication with Pinpad", AlertType.warning, -1);
			}
			}
			catch (Exception ex)
			{

			}
		}

        private void _btnBack_Click(object sender, RoutedEventArgs e)
        {

            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault<MainWindow>();
            if (mainWindow != null && mainWindow.m_naviService != null && mainWindow.m_naviService.CanGoBack)
            {
                mainWindow.m_naviService.GoBack();
            }
        }
    }
}
