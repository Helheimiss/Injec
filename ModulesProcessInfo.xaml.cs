using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Injec
{
    /// <summary>
    /// Interaction logic for ModulesProcessInfo.xaml
    /// </summary>
    public partial class ModulesProcessInfo : Window
    {
        Process SelectedProcess;

        public ModulesProcessInfo(Process SelectedProcess)
        {
            this.SelectedProcess = SelectedProcess;
            InitializeComponent();

            LoadModulesDataGrid();
        }

        private void SearchProcessTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ModulesDataGrid.Items.Count > 0)
            {
                for (int i = 0; i < ModulesDataGrid.Items.Count; ++i)
                {
                    InjecModules proc = ModulesDataGrid.Items[i] as InjecModules;

                    if (proc.ModuleName.StartsWith(SearchModuleTextBox.Text) ||
                        proc.ModuleName.Contains(SearchModuleTextBox.Text) ||
                        proc.ModuleName.ToUpper().StartsWith(SearchModuleTextBox.Text.ToUpper()) ||
                        proc.ModuleName.ToUpper().Contains(SearchModuleTextBox.Text.ToUpper()))
                    {
                        ModulesDataGrid.SelectedIndex = i;
                        ModulesDataGrid.ScrollIntoView(ModulesDataGrid.SelectedItem);
                        break;
                    }
                }
            }
        }

        void LoadModulesDataGrid()
        {
            ProcessModuleCollection modules = SelectedProcess.Modules;
            Title = $"Modules count {modules.Count}";



            List<InjecModules> result = new List<InjecModules>(modules.Count);
            for (int i = 0; i < modules.Count; ++i)
            {
                result.Add(new InjecModules(modules[i].ModuleName, modules[i].FileName, modules[i].BaseAddress, modules[i].ModuleMemorySize, modules[i].EntryPointAddress, modules[i].FileVersionInfo));
            }

            ModulesDataGrid.ItemsSource = result;
        }

        private void RefreshModullesButton_Click(object sender, RoutedEventArgs e)
        {
            LoadModulesDataGrid();
        }
    }
}
