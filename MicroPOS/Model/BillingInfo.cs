using Newtonsoft.Json;
using System;

namespace MicroPOS.Model
{
	public class BillingInfo
	{
		public bool isBoleto { get; set; }
		public int  transactionID { get; set; }
		public string printoData { get; set; }
		private string _barcode;

		private string _receiver;

		private double _amount;

		private double _tax;

		private double _totlaAmount;

		private int _parcelas;

		private int _payMethod;

		public double Amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				this._amount = value;
			}
		}

		public string Barcode
		{
			get
			{
				return this._barcode;
			}
			set
			{
				this._barcode = value;
			}
		}

		public int Parcelas
		{
			get
			{
				return this._parcelas;
			}
			set
			{
				this._parcelas = value;
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

		public string Receiver
		{
			get
			{
				return this._receiver;
			}
			set
			{
				this._receiver = value;
			}
		}

		public double Tax
		{
			get
			{
				return this._tax;
			}
			set
			{
				this._tax = value;
			}
		}

		public double TotalAmount
		{
			get
			{
				return this._totlaAmount;
			}
			set
			{
				this._totlaAmount = value;
			}
		}

		public BillingInfo()
		{
		}

		public override string ToString()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}