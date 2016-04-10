using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Windows;
using UCS.Core.Interfaces;
using System.Threading;
using UCS.Helpers;

namespace UCS.Sys
{
    public class PluginManager
    {
        public List<PluginWrapperIGP> LoadedPluginsIGP = new List<PluginWrapperIGP>();
        public List<PluginWrapperICP> LoadedPluginsICP = new List<PluginWrapperICP>();

        public void PreloadPlugins()
        {
            //System plugin

            ICommandPlugin SysCommands = new CoreCommand();
            LoadedPluginsICP.Add(new PluginWrapperICP(SysCommands, "UCS.exe"));

            //End of system plugins

            string[] files = Directory.GetFiles("Plugins", "*.dll");

            foreach (var file in files)
            {
                try
                {
                    Assembly assemblies = Assembly.LoadFrom(file);
                    Type[] types = assemblies.GetTypes();

                    for (int i = 0; i < types.Length; i++)
                    {
                        if (types[i].GetInterface("IGeneralPlugin") != null)
                        {
                            object plug = Activator.CreateInstance(types[i]);
                            IGeneralPlugin IGP = plug as IGeneralPlugin;
                            var FinalPlugin = new PluginWrapperIGP(IGP, file);
                            LoadedPluginsIGP.Add(FinalPlugin);
                        }

                        if (types[i].GetInterface("ICommandPlugin") != null)
                        {
                            object plug = Activator.CreateInstance(types[i]);
                            ICommandPlugin ICP = plug as ICommandPlugin;
                            var FinalPlugin = new PluginWrapperICP(ICP, file);
                            LoadedPluginsICP.Add(FinalPlugin);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }

    public class PluginWrapperIGP //IGeneralPlugin Wrapper
    {
        public IGeneralPlugin plugin;
        public string DLLName;

        public PluginWrapperIGP(IGeneralPlugin pluginUI, string dllName)
        {
            plugin = pluginUI;
            DLLName = dllName;
        }

        public void LaunchUI() {
            //MessageBox.Show(string.Format("{0} {1} {2} {3} {4} {5}",Title,AuthorName, ImageURL,Information, URL, Version));
            var T = new Thread(() => { plugin.RunUI(); });
            T.SetApartmentState(ApartmentState.STA);
            T.Start();
        }
    }

    public class PluginWrapperICP //ICommandPlugin Wrapper
    {
        public ICommandPlugin plugin;
        public string DLLName;

        public List<CommandActionList> CAL = new List<CommandActionList>();

        public PluginWrapperICP(ICommandPlugin pluginUI, string dllName)
        {
            plugin = pluginUI;
            DLLName = dllName;
        }
    }
}
