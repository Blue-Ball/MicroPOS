using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MicroPOS.Control
{
	// Token: 0x02000080 RID: 128
	public partial class ucDMessageBox : UserControl
	{
		// Token: 0x0600035B RID: 859 RVA: 0x00010DB4 File Offset: 0x0000EFB4
		public ucDMessageBox()
		{
			this.InitializeComponent();
			this._msgTitle.Text = "";
			this._msgMessage.Text = "";
			this._buttons = DMessageBoxButtons.YesNo;
			this._btnMsgYes.Content = "SIM";
			this._btnMsgNo.Content = "NÃO";
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600035C RID: 860 RVA: 0x00010E1B File Offset: 0x0000F01B
		// (set) Token: 0x0600035D RID: 861 RVA: 0x00010E28 File Offset: 0x0000F028
		public string Title
		{
			get
			{
				return this._msgTitle.Text;  
			}
			set
			{
				this._msgTitle.Text = value;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600035E RID: 862 RVA: 0x00010E36 File Offset: 0x0000F036
		// (set) Token: 0x0600035F RID: 863 RVA: 0x00010E43 File Offset: 0x0000F043
		public string Message
		{
			get
			{
				return this._msgMessage.Text;
			}
			set
			{
				this._msgMessage.Text = value;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000360 RID: 864 RVA: 0x00010E51 File Offset: 0x0000F051
		// (set) Token: 0x06000361 RID: 865 RVA: 0x00010E5C File Offset: 0x0000F05C
		public DMessageBoxButtons Buttons
		{
			get
			{
				return this._buttons;
			}
			set
			{
				switch (value)
				{
				case DMessageBoxButtons.OK:
					this._btnMsgYes.Visibility = Visibility.Visible;
					this._btnMsgYes.SetValue(Grid.ColumnProperty, 3);
					this._btnMsgYes.Content = "OK";
					this._btnMsgNo.Visibility = Visibility.Hidden;
					this._btnMsgNo.SetValue(Grid.ColumnProperty, 2);
					this._btnMsgNo.Content = "";
					return;
				case DMessageBoxButtons.OKCancel:
					this._btnMsgYes.Visibility = Visibility.Visible;
					this._btnMsgYes.SetValue(Grid.ColumnProperty, 2);
					this._btnMsgYes.Content = "OK";
					this._btnMsgNo.Visibility = Visibility.Visible;
					this._btnMsgNo.SetValue(Grid.ColumnProperty, 3);
					this._btnMsgNo.Content = "Cancelar";
					return;
				case (DMessageBoxButtons)2:
				case (DMessageBoxButtons)3:
					break;
				case DMessageBoxButtons.YesNo:
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

		// Token: 0x040002E9 RID: 745
		private DMessageBoxButtons _buttons = DMessageBoxButtons.YesNo;
	}
}
