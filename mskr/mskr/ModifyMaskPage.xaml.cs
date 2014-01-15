using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Primitives;
using Microsoft.Phone.Tasks;

namespace mskr
{
    using mskr.DataSrc;

    public partial class ModifyMaskPage : PhoneApplicationPage
    {

        public ModifyMaskPage()
        {
            InitializeComponent();
            DataContext = new VM { Data = new DataSrc { SelectedItem = new DataItem { Selected = 0 } } };
            string[] masks = new string[] { "resources/crclmsk.png", "resources/crclmsk.png", "resources/crclmsk.png" };
            //this.loop.DataSource = new DataSrc<string>() { Items = masks, SelectedItem = "resources/crclmsk.png" };
        }
    }

    public class DataSrc : ILoopingSelectorDataSource, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private DataItem _selected;

        public DataItem Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    OnPropertyChanged("Selected");
                }
            }
        }

        public object GetNext(object relativeTo)
        {
            DataItem num = (DataItem)relativeTo;
            return new DataItem { Selected = num.Selected + 1 };
        }

        public object GetPrevious(object relativeTo)
        {
            DataItem num = (DataItem)relativeTo;
            return new DataItem { Selected = num.Selected - 1 };
        }

        public object SelectedItem { get { return Selected; } set { Selected = (DataItem)value; } }

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;
    }

    public class DataItem : INotifyPropertyChanged
    {
        private int _selected;

        public int Selected
        {
            get { return _selected; }
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    OnPropertyChanged("Selected");
                }
            }
        }

        public string TextItem
        {
            get { return "label for " + Selected; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class VM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private DataSrc _data;

        [XmlIgnore]
        public DataSrc Data
        {
            get { return _data; }
            set
            {
                if (_data != value)
                {
                    _data = value;
                    OnPropertyChanged("Data");
                }
            }
        }
    }
}