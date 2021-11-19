using MedicalCard.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Spire.Doc;
using System.Windows.Forms;


namespace MedicalCard.ViewModels
{
    public class PrintViewModel : INotifyPropertyChanged
    {
        private Document _document;
        private PrintDialog _printDialog;
        private PrintDocument _printDocument;

        public PrintViewModel()
        {
            Document doc = new Document();
            PrintDialog dialog = new PrintDialog();
            dialog.AllowPrintToFile = true;
            dialog.AllowCurrentPage = true;
            dialog.AllowSomePages = true;
            dialog.UseEXDialog = true;
            doc.PrintDialog = dialog;
            PrintDocument printDoc = doc.PrintDocument;
        }

        private bool _showPrintDialog;
        public bool ShowPrintDialog
        {
            get => _showPrintDialog;
            set
            {
                _showPrintDialog = value;
                OnPropetryChanged(nameof(ShowPrintDialog));
            }
        }

        private RelayCommand _printCommand;
        public RelayCommand PrintCommand
        {
            get
            {
                return _printCommand ??
                    (_printCommand = new RelayCommand(obj =>
                    {
                        if (_showPrintDialog)
                        {
                            if (_printDialog.ShowDialog() == DialogResult.OK)
                            {
                                
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {

                        }
                    }));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropetryChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
