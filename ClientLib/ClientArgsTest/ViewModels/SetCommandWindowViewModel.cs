using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ClientArgsTest.ViewModels
{


    public class SetCommandWindowViewModel : BindableBase
    {
        private string _templatePath;
        public string TemplatePath
        {
            get { return _templatePath; }
            set { SetProperty(ref _templatePath, value); }
        }
        private string _findPageNum;
        public string FindPageNum
        {
            get { return _findPageNum; }
            set { SetProperty(ref _findPageNum, value); }
        }

        public ICommand ShowTemplateWindow { get; set; }
        public ICommand SetTemplatePath { get; set; }
        public ICommand SetPageNum { get; set; }
        public ICommand SetPlayCmd { get; set; }
        public ICommand SetStopCmd { get; set; }

        public SetCommandWindowViewModel()
        {
            ShowTemplateWindow = new DelegateCommand(ShowTemplateWindowCmd);
            SetTemplatePath = new DelegateCommand(SetTemplatePathCmd);
            SetPageNum = new DelegateCommand(SetPageNumCmd);
            SetPlayCmd = new DelegateCommand(SetPlay);
            SetStopCmd = new DelegateCommand(SetStop);
        }

 

        private void ShowTemplateWindowCmd()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == false)
            {
                return;
            }

            TemplatePath = openFileDialog.FileName;

            
        }
        private void SetTemplatePathCmd()
        {
            // main window 에 TemplatePath 값 보내주기
            // Event? public ?

        }

        private void SetPageNumCmd()
        {
            //FindPageNum
        }
        private void SetStop()
        {
            throw new NotImplementedException();
        }

        private void SetPlay()
        {
            throw new NotImplementedException();
        }
    }
}
