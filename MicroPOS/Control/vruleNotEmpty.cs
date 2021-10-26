using System;
using System.Globalization;
using System.Windows.Controls;

namespace MicroPOS.Control
{
	// Token: 0x02000088 RID: 136
	public class vruleNotEmpty : ValidationRule
	{
		// Token: 0x06000377 RID: 887 RVA: 0x000113D7 File Offset: 0x0000F5D7
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (!string.IsNullOrWhiteSpace((value ?? "").ToString()))
			{
				return ValidationResult.ValidResult;
			}
			return new ValidationResult(false, "Field is required.");
		}
	}
}
