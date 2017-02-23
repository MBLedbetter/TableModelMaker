using System;
using Mvvm;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Windows.Input;
using TableModelMaker.Services;
using System.Collections.ObjectModel;

namespace TableModelMaker.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Commands 

        public ICommand HelpWindowCommand
        {
            get
            {
                if (_openHelpWindow == null)
                {
                    _openHelpWindow = new RelayCommand(param => OpenHelpWindow());
                }
                return _openHelpWindow;
            }
        }
        private RelayCommand _openHelpWindow;

        public ICommand SelectFileCommand
        {
            get
            {
                if (_selectFile == null)
                {
                    _selectFile = new RelayCommand(param => SelectFile());
                }
                return _selectFile;
            }
        }
        private RelayCommand _selectFile;


        #endregion // Commands

        #region Properties
        
        public TableRowService TableRowService = new TableRowService();

        public string Content
        {
            get
            {
                return _Content;
            }
            set
            {
                if (Content != value)
                {
                    _Content = value;
                    OnPropertyChanged("Content");
                }
            }
        }
        private string _Content = string.Empty;

        #endregion // Properties

        #region Public Methods

        public void SelectFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel files|*.xls; *.xlsx";
            if (dialog.ShowDialog() != true)
            {
                return;
            }
            else
            {
                Content = TableRowService.ProcessRows(dialog.FileName);
            }
        }

        public void OpenHelpWindow()
        {

        }

        #endregion // Public Methods

        #region Constructor

        public MainWindowViewModel()
        {

        }

        #endregion // Constructor
    }
}
