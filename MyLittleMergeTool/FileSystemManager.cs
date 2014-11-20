using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MyLittleMergeTool.Entities;
using TreeViewWithCheckBoxes;

namespace MyLittleMergeTool
{
    public class FileSystemManager
    {
        
        public Int32 idx;
        private String[] fileNames;
        private TextReader tr;
        private TextWriter tw;
        private String upgradePathDirectory;

        public ScriptViewModel scriptDirectory;

        public FileSystemManager(String fileDirectoryPath)
        {
            idx = 0;
            fileNames = Directory.GetFiles(fileDirectoryPath);
            upgradePathDirectory = fileDirectoryPath;
            GetUpdateStructure();
        }

        public void GetUpdateStructure()
        {
            scriptDirectory = new ScriptViewModel(upgradePathDirectory, true);
        }

        
    }
}
