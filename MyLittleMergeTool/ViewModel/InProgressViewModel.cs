using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyLittleMergeTool.Entities;

namespace MyLittleMergeTool.ViewModel
{
    public class InProgressViewModel
    {
        public static ScriptViewModel scriptViewModel { get; set; }

        public Int32 CompletingRatio
        {
            get { return scriptViewModel.GetCheckedItems()/scriptViewModel.GetProcessedItems(); }
        }

    }
}
