using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MicroPOS.ViewModel
{
	public static class NotifyPropertyChangedExtension
	{
		public static void MutateVerbose<TField>(this INotifyPropertyChanged instance, ref TField field, TField newValue, Action<PropertyChangedEventArgs> raise, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<TField>.Default.Equals(field, newValue))
			{
				return;
			}
			field = newValue;
			if (raise != null)
			{
				raise(new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}