using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MicroPOS.Popup
{
	// Token: 0x02000055 RID: 85
	public partial class KMessageBox : Window
	{
		// Token: 0x06000233 RID: 563 RVA: 0x0000CB58 File Offset: 0x0000AD58
		public KMessageBox()
		{
			this.InitializeComponent();
			this._txtMsgTitle.Text = "";
			this._txtMsgDesc.Text = "";
			this._txtMsgTitle.Visibility = Visibility.Hidden;
			this._buttons = KMessageBoxButtons.YesNo;
			this._btnMsgYes.Content = "SIM";
			this._btnMsgNo.Content = "NÃO";
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000CBCB File Offset: 0x0000ADCB
		// (set) Token: 0x06000235 RID: 565 RVA: 0x0000CBD8 File Offset: 0x0000ADD8
		public string Subject
		{
			get
			{
				return this._txtMsgTitle.Text;
			}
			set
			{
				this._txtMsgTitle.Text = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000CBE6 File Offset: 0x0000ADE6
		// (set) Token: 0x06000237 RID: 567 RVA: 0x0000CBF3 File Offset: 0x0000ADF3
		public string Message
		{
			get
			{
				return this._txtMsgDesc.Text;
			}
			set
			{
				this._txtMsgDesc.Text = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000CC01 File Offset: 0x0000AE01
		// (set) Token: 0x06000239 RID: 569 RVA: 0x0000CC0C File Offset: 0x0000AE0C
		public KMessageBoxButtons Buttons
		{
			get
			{
				return this._buttons;
			}
			set
			{
				switch (value)
				{
				case KMessageBoxButtons.OK:
					this._btnMsgYes.Visibility = Visibility.Visible;
					this._btnMsgYes.SetValue(Grid.ColumnProperty, 3);
					this._btnMsgYes.Content = "OK";
					this._btnMsgNo.Visibility = Visibility.Hidden;
					this._btnMsgNo.SetValue(Grid.ColumnProperty, 2);
					this._btnMsgNo.Content = "";
					return;
				case KMessageBoxButtons.OKCancel:
					this._btnMsgYes.Visibility = Visibility.Visible;
					this._btnMsgYes.SetValue(Grid.ColumnProperty, 2);
					this._btnMsgYes.Content = "OK";
					this._btnMsgNo.Visibility = Visibility.Visible;
					this._btnMsgNo.SetValue(Grid.ColumnProperty, 3);
					this._btnMsgNo.Content = "Cancelar";
					return;
				case (KMessageBoxButtons)2:
				case (KMessageBoxButtons)3:
					break;
				case KMessageBoxButtons.YesNo:
					this._btnMsgYes.Visibility = Visibility.Visible;
					this._btnMsgYes.SetValue(Grid.ColumnProperty, 2);
					this._btnMsgYes.Content = "SIM";
					this._btnMsgNo.Visibility = Visibility.Visible;
					this._btnMsgNo.SetValue(Grid.ColumnProperty, 3);
					this._btnMsgNo.Content = "NÃO";
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000CD62 File Offset: 0x0000AF62
		private void _btnMsgYes_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000CD70 File Offset: 0x0000AF70
		private void _btnMsgNo_Click(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x04000216 RID: 534
		private KMessageBoxButtons _buttons = KMessageBoxButtons.YesNo;
	}
}
