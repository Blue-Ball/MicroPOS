using GalaSoft.MvvmLight;
using MicroPOS.Model;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace MicroPOS.ViewModel
{
	public class PageBillingViewModel : ViewModelBase
	{
		private BillingInfo _billInfo;

		private Color _pageBackColor = (Color)ColorConverter.ConvertFromString("White");

		private Color _pageForeColor = Color.FromRgb(0, 0, 0);

		private Color _pageTitleColor = (Color)ColorConverter.ConvertFromString("Blue");

		private Color _txtHintColor = (Color)ColorConverter.ConvertFromString("Silver");

		private Color _txtForeColor = (Color)ColorConverter.ConvertFromString("Black");

		private SolidColorBrush _txtCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));

		private SolidColorBrush _txtDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));

		public BillingInfo BillInfo
		{
			get
			{
				return this._billInfo;
			}
			set
			{
				base.Set<BillingInfo>(Expression.Lambda<Func<BillingInfo>>(Expression.Property(Expression.Constant(this, typeof(PageBillingViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(PageBillingViewModel).GetMethod("get_BillInfo").MethodHandle)), Array.Empty<ParameterExpression>()), ref this._billInfo, value);
			}
		}

		public RelayCommand<string> CmdSetBarcode
		{
			get;
			private set;
		}

		public RelayCommand<int> CmdSetInstallment
		{
			get;
			private set;
		}

		public RelayCommand<int> CmdSetPayMethod
		{
			get;
			private set;
		}

		public SolidColorBrush mPageBackColor
		{
			get
			{
				return new SolidColorBrush(this._pageBackColor);
			}
		}

		public SolidColorBrush mPageForeColor
		{
			get
			{
				return new SolidColorBrush(this._pageForeColor);
			}
		}

		public SolidColorBrush mPageTitleColor
		{
			get
			{
				return new SolidColorBrush(this._pageTitleColor);
			}
		}

		public SolidColorBrush mTextCreditColor
		{
			get
			{
				return this._txtCreditColor;
			}
			set
			{
				base.Set<SolidColorBrush>(Expression.Lambda<Func<SolidColorBrush>>(Expression.Property(Expression.Constant(this, typeof(PageBillingViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(PageBillingViewModel).GetMethod("get_mTextCreditColor").MethodHandle)), Array.Empty<ParameterExpression>()), ref this._txtCreditColor, value);
			}
		}

		public SolidColorBrush mTextDebitColor
		{
			get
			{
				return this._txtDebitColor;
			}
			set
			{
				base.Set<SolidColorBrush>(Expression.Lambda<Func<SolidColorBrush>>(Expression.Property(Expression.Constant(this, typeof(PageBillingViewModel)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(PageBillingViewModel).GetMethod("get_mTextDebitColor").MethodHandle)), Array.Empty<ParameterExpression>()), ref this._txtDebitColor, value);
			}
		}

		public SolidColorBrush mTextForeColor
		{
			get
			{
				return new SolidColorBrush(this._txtForeColor);
			}
		}

		public SolidColorBrush mTextHintColor
		{
			get
			{
				return new SolidColorBrush(this._txtHintColor);
			}
		}

		public PageBillingViewModel()
		{
			this._billInfo = new BillingInfo();
			this.CmdSetBarcode = new RelayCommand<string>(new Action<string>(this.ExecuteSetBarcode));
			this.CmdSetPayMethod = new RelayCommand<int>(new Action<int>(this.ExecuteSetPayMethod));
			this.CmdSetInstallment = new RelayCommand<int>(new Action<int>(this.ExecuteSetInstallment));
		}

		private void ExecuteSetBarcode(string p_barcode)
		{
			this._billInfo.PayMethod = 0;
			this.mTextCreditColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
			this.mTextDebitColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
			this.BillInfo.Barcode = p_barcode;
			this.ss_checkBarcode(p_barcode);
		}

		private void ExecuteSetInstallment(int p_instNumber)
		{
			this.BillInfo.Parcelas = p_instNumber;
		}

		private void ExecuteSetPayMethod(int p_payMethod)
		{
			this.BillInfo.PayMethod = p_payMethod;
		}

		private bool ss_checkBarcode(string barCode)
		{
			this.BillInfo.Receiver = "Claudio Henrique";
			this.BillInfo.Amount = 250;
			this.BillInfo.Tax = this.BillInfo.Amount * 0.04;
			this.BillInfo.TotalAmount = this.BillInfo.Amount + this.BillInfo.Tax;
			return true;
		}
	}
}