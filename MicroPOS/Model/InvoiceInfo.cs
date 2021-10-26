using System;

namespace MicroPOS.Model
{
	internal class InvoiceInfo
	{
		private string _Seller;

		private decimal _Price;

		private decimal _Tax;

		private decimal _TotalAmount;

		public decimal Price
		{
			get
			{
				return this._Price;
			}
			set
			{
				this._Price = value;
			}
		}

		public string SellerName
		{
			get
			{
				return this._Seller;
			}
			set
			{
				this._Seller = value;
			}
		}

		public decimal Tax
		{
			get
			{
				return this._Tax;
			}
			set
			{
				this._Tax = value;
			}
		}

		public decimal TotalAmount
		{
			get
			{
				return this._TotalAmount;
			}
			set
			{
				this._TotalAmount = value;
			}
		}

		public InvoiceInfo()
		{
		}

		public void GetInvoiceFromBarcode(string barcode)
		{
			this._Seller = "Neymar";
			this._Price = new decimal(10000, 0, 0, false, 2);
			this._Tax = new decimal(58, 0, 0, false, 1);
			this._TotalAmount = this._Price + this._Tax;
		}
	}
}