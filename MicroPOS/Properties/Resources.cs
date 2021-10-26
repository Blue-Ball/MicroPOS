using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace MicroPOS.Properties
{
	// Token: 0x02000023 RID: 35
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x06000099 RID: 153 RVA: 0x00003DB6 File Offset: 0x00001FB6
		internal Resources()
		{
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00005C44 File Offset: 0x00003E44
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new ResourceManager("MicroPOS.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00005C70 File Offset: 0x00003E70
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00005C77 File Offset: 0x00003E77
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x0400009E RID: 158
		private static ResourceManager resourceMan;

		// Token: 0x0400009F RID: 159
		private static CultureInfo resourceCulture;
	}
}
