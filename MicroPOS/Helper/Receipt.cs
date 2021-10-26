using System;
using System.Collections.Generic;
using System.Drawing;

namespace MicroPOS.Helper
{
	// Token: 0x0200007E RID: 126
	public class Receipt
	{
		// Token: 0x06000343 RID: 835 RVA: 0x0000FE8C File Offset: 0x0000E08C
		public Receipt(string stroe_name, string stroe_sub_name, DateTime receipt_date, string cashier, List<ReceiptItem> items, List<ReceiptItem> sub_total_items, List<ReceiptItem> total_items)
		{
			this.store_name = stroe_name;
			this.stroe_sub_name = stroe_sub_name;
			this.receipt_date = receipt_date;
			this.cashier = cashier;
			this.items = items;
			this.sub_total_items = sub_total_items;
			this.total_items = total_items;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000FEF5 File Offset: 0x0000E0F5
		public void print(int _print_page_width, Graphics g)
		{
			this.print_page_width = _print_page_width;
			this.calc_margin(g);
			this.print_header(g);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000FF0C File Offset: 0x0000E10C
		private void calc_margin(Graphics g)
		{
			this.default_font = new Font(this.fontFamily, 10f);
			this.store_font = new Font(this.fontFamily, 14f, FontStyle.Bold);
			this.receipt_title_font = new Font(this.fontFamily, 18f, FontStyle.Bold);
			this.total_font = new Font(this.fontFamily, 10f, FontStyle.Bold);
			this.thanks_font = new Font(this.fontFamily, 12f, FontStyle.Bold);
			this.subitem_font = new Font(this.fontFamily, 8f);
			this.margin_left = (this.margin_right = (float)this.print_page_width / 40f);
			this.margin_top = (this.margin_bottom = this.margin_left * 2f);
			this.item_start_pos = this.margin_left * 2f;
			this.subitem_start_pos = (float)((int)((double)this.margin_left * 2.5));
			this.price_end_pos = (float)this.print_page_width - this.margin_right * 2f;
			this.one_char_size = g.MeasureString("-", this.default_font);
			float num = ((float)this.print_page_width - this.margin_left * 2f) / this.one_char_size.Width;
			this.line_text = "-";
			while (g.MeasureString(this.line_text, this.default_font).Width < (float)this.print_page_width - this.margin_left * 2f)
			{
				this.line_text += "-";
			}
			this.line_text += "-";
		}

		// Token: 0x06000346 RID: 838 RVA: 0x000100C0 File Offset: 0x0000E2C0
		private void print_header(Graphics g)
		{
			float x = this.margin_left;
			float num = this.margin_top;
			g.DrawString(this.line_text, this.default_font, new SolidBrush(Color.Black), new PointF(x, num));
			num += this.one_char_size.Height;
			SizeF sizeF = g.MeasureString(this.store_name, this.store_font);
			x = ((float)this.print_page_width - this.margin_left * 2f - sizeF.Width) / 2f + this.margin_left;
			num += 5f;
			g.DrawString(this.store_name, this.store_font, new SolidBrush(Color.Black), new PointF(x, num));
			num += sizeF.Height;
			sizeF = g.MeasureString(this.stroe_sub_name, this.default_font);
			x = ((float)this.print_page_width - this.margin_left * 2f - sizeF.Width) / 2f + this.margin_left;
			g.DrawString(this.stroe_sub_name, this.default_font, new SolidBrush(Color.Black), new PointF(x, num));
			num += sizeF.Height;
			x = this.margin_left;
			num += 5f;
			g.DrawString(this.line_text, this.default_font, new SolidBrush(Color.Black), new PointF(x, num));
			num += this.one_char_size.Height;
			sizeF = g.MeasureString(this.receipt_title, this.receipt_title_font);
			x = ((float)this.print_page_width - this.margin_left * 2f - sizeF.Width) / 2f + this.margin_left;
			num += 5f;
			g.DrawString(this.receipt_title, this.receipt_title_font, new SolidBrush(Color.Black), new PointF(x, num));
			num += sizeF.Height;
			sizeF = g.MeasureString(this.cashier, this.default_font);
			x = this.item_start_pos;
			g.DrawString(this.cashier, this.default_font, new SolidBrush(Color.Black), new PointF(x, num));
			string text = this.receipt_date.ToString("MM/dd/yyyy hh:mm tt");
			sizeF = g.MeasureString(text, this.default_font);
			x = this.price_end_pos - sizeF.Width;
			g.DrawString(text, this.default_font, new SolidBrush(Color.Black), new PointF(x, num));
			num += sizeF.Height;
			x = this.margin_left;
			num += 5f;
			g.DrawString(this.line_text, this.default_font, new SolidBrush(Color.Black), new PointF(x, num));
			num += this.one_char_size.Height;
			if (this.items != null && this.items.Count > 0)
			{
				foreach (ReceiptItem receiptItem in this.items)
				{
					sizeF = g.MeasureString(receiptItem.name, this.default_font);
					x = this.item_start_pos;
					g.DrawString(receiptItem.name, this.default_font, new SolidBrush(Color.Black), new PointF(x, num));
					text = receiptItem.value;
					sizeF = g.MeasureString(text, this.default_font);
					x = this.price_end_pos - sizeF.Width;
					if (receiptItem.isValRect)
					{
						g.DrawRectangle(new Pen(new SolidBrush(Color.Black)), x, num, sizeF.Width, sizeF.Height);
					}
					g.DrawString(text, this.default_font, new SolidBrush(Color.Black), new PointF(x, num));
					num += sizeF.Height;
					if (receiptItem.sub_desc != "")
					{
						sizeF = g.MeasureString(receiptItem.sub_desc, this.subitem_font);
						x = this.subitem_start_pos;
						g.DrawString(receiptItem.sub_desc, this.subitem_font, new SolidBrush(Color.Black), new PointF(x, num));
						num += sizeF.Height;
					}
				}
			}
			x = this.margin_left;
			num += 5f;
			g.DrawString(this.line_text, this.default_font, new SolidBrush(Color.Black), new PointF(x, num));
			num += this.one_char_size.Height;
			if (this.sub_total_items != null && this.sub_total_items.Count > 0)
			{
				foreach (ReceiptItem receiptItem2 in this.sub_total_items)
				{
					sizeF = g.MeasureString(receiptItem2.name, this.default_font);
					x = this.item_start_pos;
					g.DrawString(receiptItem2.name, this.default_font, new SolidBrush(Color.Black), new PointF(x, num));
					text = receiptItem2.value;
					sizeF = g.MeasureString(text, this.default_font);
					x = this.price_end_pos - sizeF.Width;
					g.DrawString(text, this.default_font, new SolidBrush(Color.Black), new PointF(x, num));
					num += sizeF.Height;
				}
				x = this.margin_left;
				num += 5f;
				g.DrawString(this.line_text, this.default_font, new SolidBrush(Color.Black), new PointF(x, num));
				num += this.one_char_size.Height;
			}
			if (this.total_items != null && this.total_items.Count > 0)
			{
				int num2 = 0;
				foreach (ReceiptItem receiptItem3 in this.total_items)
				{
					sizeF = g.MeasureString(receiptItem3.name, (num2 == 0) ? this.total_font : this.default_font);
					x = this.item_start_pos;
					g.DrawString(receiptItem3.name, (num2 == 0) ? this.total_font : this.default_font, new SolidBrush(Color.Black), new PointF(x, num));
					text = receiptItem3.value;
					sizeF = g.MeasureString(text, this.default_font);
					x = this.price_end_pos - sizeF.Width;
					g.DrawString(text, this.default_font, new SolidBrush(Color.Black), new PointF(x, num));
					num += sizeF.Height;
					num2++;
				}
				x = this.margin_left;
				num += 5f;
				g.DrawString(this.line_text, this.default_font, new SolidBrush(Color.Black), new PointF(x, num));
				num += this.one_char_size.Height;
			}
			sizeF = g.MeasureString(this.thanks_title, this.thanks_font);
			x = ((float)this.print_page_width - this.margin_left * 2f - sizeF.Width) / 2f + this.margin_left;
			g.DrawString(this.thanks_title, this.thanks_font, new SolidBrush(Color.Black), new PointF(x, num));
			num += sizeF.Height;
			x = this.margin_left;
			num += 5f;
			g.DrawString(this.line_text, this.default_font, new SolidBrush(Color.Black), new PointF(x, num));
			num += this.one_char_size.Height;
		}

		// Token: 0x040002CC RID: 716
		protected int print_page_width;

		// Token: 0x040002CD RID: 717
		protected float margin_top;

		// Token: 0x040002CE RID: 718
		protected float margin_bottom;

		// Token: 0x040002CF RID: 719
		protected float margin_left;

		// Token: 0x040002D0 RID: 720
		protected float margin_right;

		// Token: 0x040002D1 RID: 721
		protected float item_start_pos;

		// Token: 0x040002D2 RID: 722
		protected float subitem_start_pos;

		// Token: 0x040002D3 RID: 723
		protected float price_end_pos;

		// Token: 0x040002D4 RID: 724
		protected string fontFamily = "Times New Roman";

		// Token: 0x040002D5 RID: 725
		protected Font default_font;

		// Token: 0x040002D6 RID: 726
		protected Font store_font;

		// Token: 0x040002D7 RID: 727
		protected Font receipt_title_font;

		// Token: 0x040002D8 RID: 728
		protected Font total_font;

		// Token: 0x040002D9 RID: 729
		protected Font thanks_font;

		// Token: 0x040002DA RID: 730
		protected Font subitem_font;

		// Token: 0x040002DB RID: 731
		protected string line_text;

		// Token: 0x040002DC RID: 732
		private SizeF one_char_size;

		// Token: 0x040002DD RID: 733
		protected string store_name;

		// Token: 0x040002DE RID: 734
		protected string stroe_sub_name;

		// Token: 0x040002DF RID: 735
		protected DateTime receipt_date;

		// Token: 0x040002E0 RID: 736
		protected string cashier;

		// Token: 0x040002E1 RID: 737
		protected List<ReceiptItem> items;

		// Token: 0x040002E2 RID: 738
		protected List<ReceiptItem> sub_total_items;

		// Token: 0x040002E3 RID: 739
		protected List<ReceiptItem> total_items;

		// Token: 0x040002E4 RID: 740
		protected readonly string receipt_title = "*** RECIBO ***";

		// Token: 0x040002E5 RID: 741
		protected readonly string thanks_title = "Obrigado por utiliza a Rede Totem!";
	}
}
