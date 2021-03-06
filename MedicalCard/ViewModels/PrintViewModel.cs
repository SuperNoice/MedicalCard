using MedicalCard.Commands;
using MedicalCard.Models;
using Spire.Doc;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MedicalCard.ViewModels
{
    public class PrintViewModel : INotifyPropertyChanged
    {
        private Document _document;
        private PrintDialog _printDialog;
        private PrintDocument _printDocument;
        private string[] _templates;
        private Card _selectedCard;

        public bool IsPrinting
        {
            get;
            private set;
        }

        public PrintViewModel(Card card)
        {
            IsPrinting = false;
            ProgressBarVisability = Visibility.Collapsed;
            ButtonVisability = Visibility.Visible;
            _progressBarValue = 0;
            _selectedCard = card;
            _document = new Document();
            _printDialog = new PrintDialog();
            _printDialog.AllowPrintToFile = true;
            _printDialog.AllowCurrentPage = true;
            _printDialog.AllowSomePages = true;
            _printDialog.UseEXDialog = true;
            _document.PrintDialog = _printDialog;
            _printDocument = _document.PrintDocument;

            _templateNames = new ObservableCollection<DocTemplate>();

            Directory.CreateDirectory("./templates");
            _templates = Directory.GetFiles("./templates", "*.doc");
            foreach (string filePath in _templates)
            {
                if (!Regex.IsMatch(filePath, @"\[\d\]") || Regex.IsMatch(filePath, @"\[1\]"))
                {
                    _templateNames.Add(new DocTemplate(filePath));
                }
            }

            var sorted = from name in _templateNames
                         orderby name.FileName
                         select name;
            _templateNames = new ObservableCollection<DocTemplate>(sorted);
        }

        private ObservableCollection<DocTemplate> _templateNames;
        public ObservableCollection<DocTemplate> TemplateNames
        {
            get => _templateNames;
            set
            {
                _templateNames = value;
                OnPropetryChanged(nameof(TemplateNames));
            }
        }

        private Visibility _buttonVisability = Visibility.Visible;
        public Visibility ButtonVisability
        {
            get => _buttonVisability;
            set
            {
                _buttonVisability = value;
                OnPropetryChanged(nameof(ButtonVisability));
            }
        }

        private Visibility _progressBarVisability = Visibility.Collapsed;
        public Visibility ProgressBarVisability
        {
            get => _progressBarVisability;
            set
            {
                _progressBarVisability = value;
                OnPropetryChanged(nameof(ProgressBarVisability));
            }
        }

        private double _progressBarValue;
        public double ProgressBarValue
        {
            get => _progressBarValue;
            set
            {
                _progressBarValue = value;
                OnPropetryChanged(nameof(ProgressBarValue));
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
                        var documentsCount = _templateNames.Count(x => x.IsActive);
                        if (documentsCount == 0)
                        {
                            System.Windows.MessageBox.Show("Нет выбранных файлов!");
                            return;
                        }

                        if (_printDialog.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        var progressBarStep = 100 / (double)documentsCount;
                        IsPrinting = true;
                        _progressBarValue = 0;
                        ButtonVisability = Visibility.Collapsed;
                        ProgressBarVisability = Visibility.Visible;
                        Task.Factory.StartNew(() =>
                        {
                            foreach (DocTemplate template in _templateNames)
                            {
                                if (!IsPrinting)
                                {
                                    break;
                                }

                                if (template.IsActive)
                                {
                                    var docs = Directory.GetFiles("./templates", $"{Path.GetFileNameWithoutExtension(template.FilePath)}*");

                                    var sorted = (from name in docs
                                                  orderby name
                                                  select name).ToArray();

                                    foreach (var doc in sorted)
                                    {
                                        try
                                        {
                                            _document = new Document();
                                            _document.PrintDialog = _printDialog;
                                            _printDocument = _document.PrintDocument;
                                            _document.LoadFromFile(doc, FileFormat.Docx);
                                            ReplaceInDoc();
                                            // TODO: Здесь падает NullReferenceExeption
                                            _printDocument.Print();
                                            _document.Close();
                                            ProgressBarValue += progressBarStep;
                                        }
                                        catch (NullReferenceException)
                                        {
                                            System.Windows.MessageBox.Show($"Не удалось прочитать файл: {template.FileName}\n\nУдалите все закладки! (Word -> Вставка -> Ссылки -> Закладка)", "Ошибка чтения файла!");
                                        }
                                        catch (Exception e)
                                        {
                                            System.Windows.MessageBox.Show($"Не удалось прочитать файл: {template.FileName}\n\n{e.Message}", "Ошибка чтения файла!");
                                        }
                                    }

                                }
                            }
                            IsPrinting = false;
                            ProgressBarValue = 0;
                            ButtonVisability = Visibility.Visible;
                            ProgressBarVisability = Visibility.Collapsed;
                        });


                    }));
            }
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ??
                    (_cancelCommand = new RelayCommand(obj =>
                    {
                        IsPrinting = false;
                    }));
            }
        }

        private void ReplaceInDoc()
        {
            DateTime birthDay;
            DateTime.TryParseExact(_selectedCard.BirthDay, new string[] { "dd.MM.yyyy", "d.M.yyyy" }, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out birthDay);
            DateTime regDate;
            DateTime.TryParseExact(_selectedCard.DateReg, new string[] { "dd.MM.yyyy", "d.M.yyyy" }, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out regDate);

            _document.Replace(new Regex(@"{fio}"), _selectedCard.Fio);
            _document.Replace(new Regex(@"{shortfio}"), _selectedCard.GetShortFIO());
            _document.Replace(new Regex(@"{sex}"), _selectedCard.Sex.ToString());
            _document.Replace(new Regex(@"{birthDay}"), _selectedCard.BirthDay);
            _document.Replace(new Regex(@"{address}"), _selectedCard.Address);
            _document.Replace(new Regex(@"{phone}"), _selectedCard.Phone);
            _document.Replace(new Regex(@"{countryType}"), _selectedCard.CountryType.ToString());
            _document.Replace(new Regex(@"{passport}"), _selectedCard.Passport);
            _document.Replace(new Regex(@"{nowd}"), DateTime.Now.Day.ToString());
            _document.Replace(new Regex(@"{nowM}"), DateTime.Now.Month.ToString());
            _document.Replace(new Regex(@"{nowMMMM}"), DateTime.Now.ToString("MMMM"));
            _document.Replace(new Regex(@"{nowyear}"), DateTime.Now.ToString("yyyy"));
            _document.Replace(new Regex(@"{Bd}"), birthDay.Day.ToString());
            _document.Replace(new Regex(@"{BM}"), birthDay.Month.ToString());
            _document.Replace(new Regex(@"{BMMMM}"), birthDay.ToString("MMMM"));
            _document.Replace(new Regex(@"{Byear}"), birthDay.ToString("yyyy"));

        }



        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropetryChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
