using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace MicroPOS.ViewModel
{
	public class ViewModelLocator
	{
		public MainViewModel Main
		{
			get
			{
				return ServiceLocator.Current.GetInstance<MainViewModel>();
			}
		}

		public PageBillingViewModel PageBilling
		{
			get
			{
				return ServiceLocator.Current.GetInstance<PageBillingViewModel>();
			}
		}

		public PageGiftViewModel PageGift
		{
			get
			{
				return ServiceLocator.Current.GetInstance<PageGiftViewModel>();
			}
		}

		public PageIpsViewModel PageIps
		{
			get
			{
				return ServiceLocator.Current.GetInstance<PageIpsViewModel>();
			}
		}

		public PageRechargeViewModel PageRecharge
		{
			get
			{
				return ServiceLocator.Current.GetInstance<PageRechargeViewModel>();
			}
		}

		public PageShareViewModel PageShare
		{
			get
			{
				return ServiceLocator.Current.GetInstance<PageShareViewModel>();
			}
		}

		public ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
			SimpleIoc.Default.Register<MainViewModel>();
			SimpleIoc.Default.Register<PageBillingViewModel>();
			SimpleIoc.Default.Register<PageRechargeViewModel>();
			SimpleIoc.Default.Register<PageShareViewModel>();
			SimpleIoc.Default.Register<PageGiftViewModel>();
			SimpleIoc.Default.Register<PageIpsViewModel>();
			Messenger.Default.Register<NotificationMessage>(this, new Action<NotificationMessage>(this.NotifyUserMethod), false);
		}

		public static void Cleanup()
		{
		}

		private void NotifyUserMethod(NotificationMessage message)
		{
			MessageBox.Show(message.Notification);
		}
	}
}