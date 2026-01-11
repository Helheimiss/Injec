using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace Injec
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OpenFileDialog OpenDllDialog = new OpenFileDialog();
        public MainWindow()
        {
            InitializeComponent();
            OpenDllDialog.Filter = "Dll files (*.dll)|*.dll";
            OpenDllDialog.FilterIndex = 1;
            OpenDllDialog.RestoreDirectory = true;
            OpenDllDialog.Multiselect = false;

            LoadProcessDataGrid();
        }

        void LoadProcessDataGrid()
        {
            var process = Process.GetProcesses();
            MainWindowInjec.Title = $"Injec process count {process.Length}";

            List<InjecProcess> result = new List<InjecProcess>(process.Length);
            foreach (var item in process)
                result.Add(new InjecProcess(item.ProcessName, item.Id));

            ProcessDataGrid.ItemsSource = result;
        }

        private void RefreshProcessButton_Click(object sender, RoutedEventArgs e)
        {
            LoadProcessDataGrid();
        }

        private void SelectProcessButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentModuleInfoTextBox.Text = string.Empty;

            if (ProcessDataGrid.SelectedItem != null)
            {
                if (OpenDllDialog.ShowDialog() == true && OpenDllDialog.FileNames.Length > 0)
                {
                    InjecProcess process = ProcessDataGrid.SelectedItem as InjecProcess;
                    CurrentModuleInfoTextBox.Text += $"[+] process {process.ProcessName} {process.PID}\n";

                    bool otvet = InjecProcess.InjectModuleToProcess(process.PID, OpenDllDialog.FileNames[0]);
                    if (otvet)
                        CurrentModuleInfoTextBox.Text += $"[+] loading {OpenDllDialog.FileNames[0]}\n";
                    else
                        CurrentModuleInfoTextBox.Text += $"[-] loading error\n";
                }
            }
            else
            {
                CurrentModuleInfoTextBox.Text += $"[-] select process\n";
            }
        }

        private void ClearCurrentInfoButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentModuleInfoTextBox.Text = string.Empty;
        }

        private void SearchProcessTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (ProcessDataGrid.Items.Count > 0)
            {
                for (int i = 0; i < ProcessDataGrid.Items.Count; ++i)
                {
                    InjecProcess proc = ProcessDataGrid.Items[i] as InjecProcess;

                    if (proc.ProcessName.StartsWith(SearchProcessTextBox.Text) ||
                        proc.ProcessName.Contains(SearchProcessTextBox.Text) ||
                        proc.ProcessName.ToUpper().StartsWith(SearchProcessTextBox.Text.ToUpper()) ||
                        proc.ProcessName.ToUpper().Contains(SearchProcessTextBox.Text.ToUpper()) ||
                        proc.PID.ToString().StartsWith(SearchProcessTextBox.Text))
                    {
                        ProcessDataGrid.SelectedIndex = i;
                        ProcessDataGrid.ScrollIntoView(ProcessDataGrid.SelectedItem);
                        break;
                    }
                }
            }
        }

        private void ListModulesForCurrentProcessButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessDataGrid.SelectedItem != null)
            {
                try
                {
                    InjecProcess pr = ProcessDataGrid.SelectedItem as InjecProcess;
                    ModulesProcessInfo mpi = new ModulesProcessInfo(pr.PID);
                    mpi.ShowDialog();
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                CurrentModuleInfoTextBox.Text = string.Empty;
                CurrentModuleInfoTextBox.Text += $"[-] select process\n";
            }
        }
    }
}
