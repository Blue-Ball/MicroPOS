using GalaSoft.MvvmLight;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MicroPOS.Model
{
	internal class Payment : ObservableObject
	{
		private string _strBarCode;

		private MicroPOS.Model.InvoiceInfo _invoiceInfo;

		private decimal _payAmount;

		private int _installment;

		private Payment.EN_PyamentMethod _payMethod;

		public string Barcode
		{
			get
			{
				return this._strBarCode;
			}
			set
			{
				base.Set<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(Payment)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(Payment).GetMethod("get_Barcode").MethodHandle)), Array.Empty<ParameterExpression>()), ref this._strBarCode, value);
			}
		}

		public int Installment
		{
			get
			{
				return this._installment;
			}
			set
			{
				base.Set<int>(Expression.Lambda<Func<int>>(Expression.Property(Expression.Constant(this, typeof(Payment)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(Payment).GetMethod("get_Installment").MethodHandle)), Array.Empty<ParameterExpression>()), ref this._installment, value);
			}
		}

		public MicroPOS.Model.InvoiceInfo InvoiceInfo
		{
			get
			{
				return this._invoiceInfo;
			}
			set
			{
				base.Set<MicroPOS.Model.InvoiceInfo>(Expression.Lambda<Func<MicroPOS.Model.InvoiceInfo>>(Expression.Property(Expression.Constant(this, typeof(Payment)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(Payment).GetMethod("get_InvoiceInfo").MethodHandle)), Array.Empty<ParameterExpression>()), ref this._invoiceInfo, value);
			}
		}

		public decimal PayAmount
		{
			get
			{
				return this._payAmount;
			}
			set
			{
				base.Set<decimal>(Expression.Lambda<Func<decimal>>(Expression.Property(Expression.Constant(this, typeof(Payment)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(Payment).GetMethod("get_PayAmount").MethodHandle)), Array.Empty<ParameterExpression>()), ref this._payAmount, value);
			}
		}

		public Payment.EN_PyamentMethod PayMethod
		{
			get
			{
				return this._payMethod;
			}
			set
			{
				base.Set<Payment.EN_PyamentMethod>(Expression.Lambda<Func<Payment.EN_PyamentMethod>>(Expression.Property(Expression.Constant(this, typeof(Payment)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(Payment).GetMethod("get_PayMethod").MethodHandle)), Array.Empty<ParameterExpression>()), ref this._payMethod, value);
			}
		}

		public Payment()
		{
		}

		public enum EN_PyamentMethod
		{
			PAY_METHOD_UNKNOWN = -1,
			PAY_METHOD_CREDIT = 0,
			PAY_METHOD_DEBIT = 1
		}
	}
}