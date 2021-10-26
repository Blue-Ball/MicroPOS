using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace MicroPOS.Properties
{
	// Token: 0x02000024 RID: 36
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00005C7F File Offset: 0x00003E7F
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x040000A0 RID: 160
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
