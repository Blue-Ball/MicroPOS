using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Media;
using NLog;

namespace MicroPOS.Helper
{
	// Token: 0x02000077 RID: 119
	public class PosHelper
	{
		// Token: 0x06000327 RID: 807 RVA: 0x0000F7A3 File Offset: 0x0000D9A3
		public static string GetCurrencyString(double p_value)
		{
			return string.Format(new CultureInfo("pt-BR"), "{0:C2}", p_value);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000F7C0 File Offset: 0x0000D9C0
		public static string GetPayMethodName(int p_mehtod)
		{
			string result = "";
			if (p_mehtod == 1)
			{
				result = "Crédito";
			}
			else if (p_mehtod == 2)
			{
				result = "Débito";
			}
			return result;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000F7EC File Offset: 0x0000D9EC
		public static string GetNetworkName(int p_networkType)
		{
			string result = "";
			switch (p_networkType)
			{
			case 1:
				result = "OI";
				break;
			case 2:
				result = "TIM";
				break;
			case 3:
				result = "VIVO";
				break;
			case 4:
				result = "CLARO";
				break;
			}
			return result;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000F838 File Offset: 0x0000DA38
		public static List<double> GetRechargeValues(int p_networkType)
		{
			List<double> list = new List<double>();
			switch (p_networkType)
			{
			case 1:
				list.Add(10.0);
				list.Add(15.0);
				list.Add(20.0);
				list.Add(25.0);
				list.Add(30.0);
				break;
			case 2:
				list.Add(10.0);
				list.Add(20.0);
				list.Add(40.0);
				list.Add(80.0);
				break;
			case 3:
				list.Add(10.0);
				list.Add(100.0);
				list.Add(200.0);
				list.Add(300.0);
				list.Add(400.0);
				list.Add(500.0);
				break;
			case 4:
				list.Add(10.0);
				list.Add(20.0);
				list.Add(30.0);
				list.Add(40.0);
				list.Add(50.0);
				break;
			}
			return list;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000F9A1 File Offset: 0x0000DBA1
		public static string GetNameFromCPF(string cpfNumber)
		{
			return "Claudio Henrique";
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000F9A8 File Offset: 0x0000DBA8
		public static List<double> GetGiftPriceList(string p_cardType)
		{
			List<double> list = new List<double>();
			string text = p_cardType.ToUpper();
			if (text != null)
			{
				if (!(text == "UBER"))
				{
					if (!(text == "XBOX"))
					{
						if (text == "STEAM")
						{
							list.Add(5.0);
							list.Add(10.0);
							list.Add(25.0);
							list.Add(50.0);
							list.Add(100.0);
						}
					}
					else
					{
						list.Add(1.0);
						list.Add(2.0);
						list.Add(3.0);
						list.Add(5.0);
						list.Add(10.0);
						list.Add(20.0);
						list.Add(50.0);
						list.Add(100.0);
					}
				}
				else
				{
					list.Add(10.0);
					list.Add(15.0);
					list.Add(20.0);
					list.Add(25.0);
					list.Add(30.0);
					list.Add(40.0);
					list.Add(50.0);
					list.Add(60.0);
					list.Add(70.0);
					list.Add(80.0);
					list.Add(90.0);
					list.Add(100.0);
				}
			}
			return list;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000FB7C File Offset: 0x0000DD7C
		public static List<string> GetIPSList()
		{
			return new List<string>
			{
				"Company A",
				"Company B",
				"Company C",
				"Company D",
				"Company E",
				"Company F",
				"Company G",
				"Company H",
				"Company I",
				"Company J"
			};
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000FBFC File Offset: 0x0000DDFC
		public static string GetNumberFromMaskText(string p_maskText)
		{
			if (p_maskText == null)
			{
				return null;
			}
			return p_maskText.Replace("(", string.Empty).Replace(")", string.Empty).Replace("_", string.Empty).Replace("-", string.Empty).Replace(".", string.Empty).Replace(" ", string.Empty);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000FC6C File Offset: 0x0000DE6C
		public static string GetAreaCodeFromPhoneNumber(string p_formatedNumber)
		{
			string result = "";
			string[] array = p_formatedNumber.Replace(" ", string.Empty).Split(new char[]
			{
				')'
			});
			if (array != null && array.Length >= 1)
			{
				result = array[0].Trim(new char[]
				{
					'('
				}).Trim();
			}
			return result;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000FCC4 File Offset: 0x0000DEC4
		public static string GetNumberFromPhoneNumber(string p_formatedNumber)
		{
			string result = "";
			string[] array = p_formatedNumber.Replace(" ", string.Empty).Split(new char[]
			{
				')'
			});
			if (array != null && array.Length >= 2)
			{
				result = array[1].Replace("-", string.Empty).Trim();
			}
			return result;
		}

		// Token: 0x040002BB RID: 699
		public static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040002BC RID: 700
		public static Color ThemeBackColor = Color.FromArgb(byte.MaxValue, 237, 227, 228);

		// Token: 0x040002BD RID: 701
		public static Color ThemeTextColor = Color.FromArgb(byte.MaxValue, 48, 48, 48);

		// Token: 0x040002BE RID: 702
		public static Color ThemeTextSelColor = Color.FromArgb(byte.MaxValue, 48, 160, 48);
	}
}
