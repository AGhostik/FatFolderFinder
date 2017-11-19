using System;
using System.Linq;
using System.Reflection;

namespace FatFolderFinder
{
    public partial class App
    {
        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // not my code

            var dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");
            dllName = dllName.Replace(".", "_");
            if (dllName.EndsWith("_resources")) return null;
            var rm = new System.Resources.ResourceManager(GetType().Namespace + ".Properties.Resources", Assembly.GetExecutingAssembly());
            var bytes = (byte[])rm.GetObject(dllName);
            return Assembly.Load(bytes);
        }
    }
}
