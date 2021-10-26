using GalaSoft.MvvmLight;
using MaterialDesignThemes.Wpf;
using MicroPOS.Control;
using MicroPOS.Model;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MicroPOS.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		private MessageBoxInfo _msgInfo;

		private Color _pageBackColor = (Color)ColorConverter.ConvertFromString("White");

		private Color _pageForeColor = Color.FromRgb(0, 0, 0);

		private Color _pageTitleColor = (Color)ColorConverter.ConvertFromString("Blue");

		private Color _txtHintColor = (Color)ColorConverter.ConvertFromString("Silver");

		private Color _txtForeColor = (Color)ColorConverter.ConvertFromString("Black");

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

		public MessageBoxInfo MsgInfo
		{
			get
			{
				return this._msgInfo;
			}
			set
			{
				this._msgInfo = value;
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

		public RelayCommand<MessageBoxInfo> RunDialogCommand
		{
			get;
			private set;
		}

		public MainViewModel()
		{
			this._msgInfo = new MessageBoxInfo();
			this.RunDialogCommand = new RelayCommand<MessageBoxInfo>(new Action<MessageBoxInfo>(this.ExecuteRunDialog));
		}

		private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
		{
			Console.WriteLine("You can intercept the closing event, and cancel here.");
		}

		private async void ExecuteRunDialog(MessageBoxInfo p_msgInfo)
		{
			ucInputMsgBox _ucInputMsgBox = new ucInputMsgBox()
			{
				DataContext = new viewmodelInputMsgBox()
			};
			_ucInputMsgBox._msgTitle.Text = p_msgInfo.Title;
			_ucInputMsgBox._msgMessage.Text = p_msgInfo.Message;
			object obj = await DialogHost.Show(_ucInputMsgBox, "RootDialog", new DialogClosingEventHandler(this.ClosingEventHandler));
			Console.WriteLine(string.Concat("Dialog was closed, the CommandParameter used to close it was: ", obj.ToString()));
		}
	}
}