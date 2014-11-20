using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using MyLittleMergeTool.Helpers;
using System.ComponentModel;

namespace MyLittleMergeTool.Entities
{
    public class ScriptViewModel : INotifyPropertyChanged
    {
        public String Name { get; set; }

        public String AbsolutePath { get; set; }

        public List<ScriptViewModel> SubElements { get; private set; }

        ScriptViewModel _parent;

        public Boolean IsDirectory { get; set; }

        private bool? _isChecked = false;

        public Boolean IsProcessed { get; set; }

        public ScriptViewModel()
        {
            IsDirectory = true;
        }

        public ScriptViewModel(String path, Boolean isDirectory)
        {

            SubElements = new List<ScriptViewModel>();
            IsDirectory = isDirectory;
            IsProcessed = false;
            if (isDirectory)
            {
                DirectoryInfo dInfo = new DirectoryInfo(path);
                Name = dInfo.Name;
                AbsolutePath = dInfo.FullName;
            }
            else
            {
                FileInfo fInfo = new FileInfo(path);
                Name = fInfo.Name;
                AbsolutePath = fInfo.FullName;
            }
            if (isDirectory)
            {
                String[] directories = Directory.GetDirectories(path);
                Array.Sort(directories, new MyComparer());
                foreach (var d in directories)
                {
                    SubElements.Add(new ScriptViewModel(d, true));
                }
                String[] files = Directory.GetFiles(path);
                foreach (var f in files)
                {

                    FileInfo fInfo = new FileInfo(f);
                    if (fInfo.Extension == ".sql")
                    {
                        SubElements.Add(new ScriptViewModel(f, false));
                    }
                }
            }
            this.Initialize();
        }

        public String Merge()
        {
            String s = "";
            if (!IsDirectory && _isChecked == true)
            {
                using (StreamReader sr = new StreamReader(AbsolutePath))
                {
                    String stringScript = sr.ReadToEnd();
                    IsProcessed = true;
                    return String.Format("\n/*-------------{2}---------------*/\n{0}\n{1}\n/*-------------{2}---------------*/\n", stringScript, Settings.Break, Name);

                }
            }
            else
            {
                foreach (var sub in SubElements)
                {
                    s = String.Format("{0}{1}", s, sub.Merge());
                }
                IsProcessed = true;
                return s;
            }

        }

        public Int32 GetCheckedItems()
        {
            if (!IsDirectory)
            {
                return IsChecked == true ? 1 : 0;
            }
            else
            {
                Int32 nSubElements = 0;
                SubElements.ForEach(s=> { nSubElements += s.GetCheckedItems(); });
                return nSubElements;
            }
        }

        public Int32 GetProcessedItems()
        {
            if (!IsDirectory)
            {
                return IsChecked == true ? 1 : 0;
            }
            else
            {
                Int32 nProcessedSubItems = 0;
                SubElements.ForEach(s => { nProcessedSubItems += s.GetProcessedItems(); });
                return nProcessedSubItems;
            }
        }

        public void ResetTree()
        {
            IsProcessed = false;
            foreach (var s in SubElements)
            {
                s.ResetTree();
            }
        }

        void Initialize()
        {
            foreach (ScriptViewModel child in SubElements)
            {
                child._parent = this;
                child.Initialize();
            }
        }

        #region Properties

        public bool IsInitiallySelected { get; private set; }

        #region IsChecked

        /// <summary>
        /// Gets/sets the state of the associated UI toggle (ex. CheckBox).
        /// The return value is calculated based on the check state of all
        /// child FooViewModels.  Setting this property to true or false
        /// will set all children to the same check state, and setting it 
        /// to any value will cause the parent to verify its check state.
        /// </summary>
        public bool? IsChecked
        {
            get { return _isChecked; }
            set { SetIsChecked(value, true, true); }
        }

        void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isChecked)
                return;

            _isChecked = value;

            if (updateChildren && _isChecked.HasValue)
                this.SubElements.ForEach(c => c.SetIsChecked(_isChecked, true, false));

            if (_parent != null && updateParent && _isChecked != null)
                _parent.VerifyCheckState();

            OnPropertyChanged("IsChecked");
        }

        void VerifyCheckState()
        {
            bool? state = null;
            for (int i = 0; i < SubElements.Count; ++i)
            {
                bool? current = SubElements[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }
            this.SetIsChecked(state, false, true);
        }

        #endregion // IsChecked

        #endregion // Properties


        void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;




    }
}
