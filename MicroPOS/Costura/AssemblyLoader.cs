using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Costura
{
	// Token: 0x02000089 RID: 137
	[CompilerGenerated]
	internal static class AssemblyLoader
	{
		// Token: 0x06000379 RID: 889 RVA: 0x00011408 File Offset: 0x0000F608
		private static string CultureToString(CultureInfo culture)
		{
			if (culture == null)
			{
				return "";
			}
			return culture.Name;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0001141C File Offset: 0x0000F61C
		private static Assembly ReadExistingAssembly(AssemblyName name)
		{
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				AssemblyName name2 = assembly.GetName();
				if (string.Equals(name2.Name, name.Name, StringComparison.InvariantCultureIgnoreCase) && string.Equals(AssemblyLoader.CultureToString(name2.CultureInfo), AssemblyLoader.CultureToString(name.CultureInfo), StringComparison.InvariantCultureIgnoreCase))
				{
					return assembly;
				}
			}
			return null;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00011484 File Offset: 0x0000F684
		private static void CopyTo(Stream source, Stream destination)
		{
			byte[] array = new byte[81920];
			int count;
			while ((count = source.Read(array, 0, array.Length)) != 0)
			{
				destination.Write(array, 0, count);
			}
		}

		// Token: 0x0600037C RID: 892 RVA: 0x000114B8 File Offset: 0x0000F6B8
		private static Stream LoadStream(string fullName)
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (fullName.EndsWith(".compressed"))
			{
				using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(fullName))
				{
					using (DeflateStream deflateStream = new DeflateStream(manifestResourceStream, CompressionMode.Decompress))
					{
						MemoryStream memoryStream = new MemoryStream();
						AssemblyLoader.CopyTo(deflateStream, memoryStream);
						memoryStream.Position = 0L;
						return memoryStream;
					}
				}
			}
			return executingAssembly.GetManifestResourceStream(fullName);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0001153C File Offset: 0x0000F73C
		private static Stream LoadStream(Dictionary<string, string> resourceNames, string name)
		{
			string fullName;
			if (resourceNames.TryGetValue(name, out fullName))
			{
				return AssemblyLoader.LoadStream(fullName);
			}
			return null;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0001155C File Offset: 0x0000F75C
		private static byte[] ReadStream(Stream stream)
		{
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			return array;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00011584 File Offset: 0x0000F784
		private static Assembly ReadFromEmbeddedResources(Dictionary<string, string> assemblyNames, Dictionary<string, string> symbolNames, AssemblyName requestedAssemblyName)
		{
			string text = requestedAssemblyName.Name.ToLowerInvariant();
			if (requestedAssemblyName.CultureInfo != null && !string.IsNullOrEmpty(requestedAssemblyName.CultureInfo.Name))
			{
				text = requestedAssemblyName.CultureInfo.Name + "." + text;
			}
			byte[] rawAssembly;
			using (Stream stream = AssemblyLoader.LoadStream(assemblyNames, text))
			{
				if (stream == null)
				{
					return null;
				}
				rawAssembly = AssemblyLoader.ReadStream(stream);
			}
			using (Stream stream2 = AssemblyLoader.LoadStream(symbolNames, text))
			{
				if (stream2 != null)
				{
					byte[] rawSymbolStore = AssemblyLoader.ReadStream(stream2);
					return Assembly.Load(rawAssembly, rawSymbolStore);
				}
			}
			return Assembly.Load(rawAssembly);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00011644 File Offset: 0x0000F844
		public static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
		{
			object obj = AssemblyLoader.nullCacheLock;
			lock (obj)
			{
				if (AssemblyLoader.nullCache.ContainsKey(e.Name))
				{
					return null;
				}
			}
			AssemblyName assemblyName = new AssemblyName(e.Name);
			Assembly assembly = AssemblyLoader.ReadExistingAssembly(assemblyName);
			if (assembly != null)
			{
				return assembly;
			}
			assembly = AssemblyLoader.ReadFromEmbeddedResources(AssemblyLoader.assemblyNames, AssemblyLoader.symbolNames, assemblyName);
			if (assembly == null)
			{
				obj = AssemblyLoader.nullCacheLock;
				lock (obj)
				{
					AssemblyLoader.nullCache[e.Name] = true;
				}
				if ((assemblyName.Flags & AssemblyNameFlags.Retargetable) != AssemblyNameFlags.None)
				{
					assembly = Assembly.Load(assemblyName);
				}
			}
			return assembly;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00011724 File Offset: 0x0000F924
		// Note: this type is marked as 'beforefieldinit'.
		static AssemblyLoader()
		{
			AssemblyLoader.assemblyNames.Add("bespokefusion", "costura.bespokefusion.dll.compressed");
			AssemblyLoader.assemblyNames.Add("cappta.gp.api.com", "costura.cappta.gp.api.com.dll.compressed");
			AssemblyLoader.assemblyNames.Add("commonservicelocator", "costura.commonservicelocator.dll.compressed");
			AssemblyLoader.assemblyNames.Add("galasoft.mvvmlight", "costura.galasoft.mvvmlight.dll.compressed");
			AssemblyLoader.assemblyNames.Add("galasoft.mvvmlight.extras", "costura.galasoft.mvvmlight.extras.dll.compressed");
			AssemblyLoader.symbolNames.Add("galasoft.mvvmlight.extras", "costura.galasoft.mvvmlight.extras.pdb.compressed");
			AssemblyLoader.symbolNames.Add("galasoft.mvvmlight", "costura.galasoft.mvvmlight.pdb.compressed");
			AssemblyLoader.assemblyNames.Add("galasoft.mvvmlight.platform", "costura.galasoft.mvvmlight.platform.dll.compressed");
			AssemblyLoader.symbolNames.Add("galasoft.mvvmlight.platform", "costura.galasoft.mvvmlight.platform.pdb.compressed");
			AssemblyLoader.assemblyNames.Add("mahapps.metro", "costura.mahapps.metro.dll.compressed");
			AssemblyLoader.symbolNames.Add("mahapps.metro", "costura.mahapps.metro.pdb.compressed");
			AssemblyLoader.assemblyNames.Add("materialdesigncolors", "costura.materialdesigncolors.dll.compressed");
			AssemblyLoader.symbolNames.Add("materialdesigncolors", "costura.materialdesigncolors.pdb.compressed");
			AssemblyLoader.assemblyNames.Add("materialdesignthemes.wpf", "costura.materialdesignthemes.wpf.dll.compressed");
			AssemblyLoader.symbolNames.Add("materialdesignthemes.wpf", "costura.materialdesignthemes.wpf.pdb.compressed");
			AssemblyLoader.assemblyNames.Add("newtonsoft.json", "costura.newtonsoft.json.dll.compressed");
			AssemblyLoader.assemblyNames.Add("nlog", "costura.nlog.dll.compressed");
			AssemblyLoader.assemblyNames.Add("system.windows.interactivity", "costura.system.windows.interactivity.dll.compressed");
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000118C1 File Offset: 0x0000FAC1
		public static void Attach()
		{
			if (Interlocked.Exchange(ref AssemblyLoader.isAttached, 1) == 1)
			{
				return;
			}
			AppDomain.CurrentDomain.AssemblyResolve += AssemblyLoader.ResolveAssembly;
		}

		// Token: 0x0400030F RID: 783
		private static object nullCacheLock = new object();

		// Token: 0x04000310 RID: 784
		private static Dictionary<string, bool> nullCache = new Dictionary<string, bool>();

		// Token: 0x04000311 RID: 785
		private static Dictionary<string, string> assemblyNames = new Dictionary<string, string>();

		// Token: 0x04000312 RID: 786
		private static Dictionary<string, string> symbolNames = new Dictionary<string, string>();

		// Token: 0x04000313 RID: 787
		private static int isAttached;
	}
}
