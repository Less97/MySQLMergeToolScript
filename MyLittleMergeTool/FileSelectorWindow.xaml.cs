using System.IO;
using System.Windows.Controls;
using MyLittleMergeTool.Entities;
using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Ookii.Dialogs.Wpf;

namespace MyLittleMergeTool
{
    public partial class Window1 : Window
    {

        public List<ScriptViewModel> Items;

        public Window1()
        {
            InitializeComponent();

           // ScriptViewModel root = new ScriptViewModel(directoryPath,true);
          

            base.CommandBindings.Add(
                new CommandBinding(
                    ApplicationCommands.Undo,
                    (sender, e) => // Execute
                    {
                        this.tree.Items.Clear();
                        VistaFolderBrowserDialog dlg = new VistaFolderBrowserDialog();

                        if (dlg.ShowDialog() == true)
                        {
                            String fPath = dlg.SelectedPath;
                            this.tree.Items.Add(new ScriptViewModel(fPath,true));
                        }
                    },
                    (sender, e) => // CanExecute
                    {
                        e.CanExecute = true;
                    }));

            base.CommandBindings.Add(
                new CommandBinding(
                    ApplicationCommands.SaveAs,
                    (sender, e) => // Execute
                    {
                        /*ScriptViewModel s = this.tree.Items[0] as ScriptViewModel;
                        SaveUpgradeScript(s.Merge());*/
                        var dlg = new VistaSaveFileDialog();
                        dlg.FileName = "Script"; // Default file name
                        dlg.DefaultExt = ".sql"; // Default file extension
                        dlg.Filter = "SQL Script (.sql)|*.sql"; // Filter files by extension

                        // Show save file dialog box
                        Nullable<bool> result = dlg.ShowDialog();

                        // Process save file dialog box results
                        if (result == true)
                        {
                            // Save document
                            string filename = dlg.FileName;
                            ScriptViewModel s = this.tree.Items[0] as ScriptViewModel;
                            SaveUpgradeScript(s.Merge(), filename);
                        }

                    },
                    (sender, e) => // CanExecute
                    {
                        e.CanExecute = true;
                    }));
        
        }


        private void SaveUpgradeScript(String scriptResult,String filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(scriptResult);
            }
        }
    }
}