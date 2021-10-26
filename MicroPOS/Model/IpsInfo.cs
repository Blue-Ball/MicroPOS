using MicroPOS.RestAPI;
using System;

namespace MicroPOS.Model
{
	public class IpsInfo
	{
		private string _company;

		private string _cpfNumber;

		private int _payMethod;

		private RTIpsInvoice _invoice;

		public double Amount
		{
			get
			{
				double valorFatura = 0;
				if (this._invoice != null && this._invoice.faturas != null)
				{
					RTFatura[] rTFaturaArray = this._invoice.faturas;
					for (int i = 0; i < (int)rTFaturaArray.Length; i++)
					{
						RTFatura rTFatura = rTFaturaArray[i];
						if (rTFatura.@select)
						{
							valorFatura += rTFatura.valor_fatura;
						}
					}
				}
				return valorFatura;
			}
		}

		public string Company
		{
			get
			{
				return this._company;
			}
			set
			{
				this._company = value;
			}
		}

		public string CpfNumber
		{
			get
			{
				return this._cpfNumber;
			}
			set
			{
				this._cpfNumber = value;
			}
		}

		public RTIpsInvoice Invoice
		{
			get
			{
				return this._invoice;
			}
			set
			{
				this._invoice = value;
			}
		}

		public int PayMethod
		{
			get
			{
				return this._payMethod;
			}
			set
			{
				this._payMethod = value;
			}
		}

		public double Tax
		{
			get
			{
				double fixedAmount = 0;
				if (this._invoice != null && this._invoice.fee != null)
				{
					fixedAmount = (double)this._invoice.fee[0].fixed_amount + this.Amount * (double)this._invoice.fee[0].percentual_amount / 100;
				}
				return fixedAmount;
			}
		}

		public double TotalAmount
		{
			get
			{
				return this.Amount + this.Tax;
			}
		}

		public IpsInfo()
		{
		}
	}
}