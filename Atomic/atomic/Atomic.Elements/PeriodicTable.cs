using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Reflection;

namespace Chemistry.Elements
{
    [XmlRoot("Table")]
    public class PeriodicTable : INotifyPropertyChanged
    {
        public PeriodicTable()
        {
        }

        public static async Task<PeriodicTable> LoadTable()
        {
            PeriodicTable aTable = null;
            try
            {
                var assembly = typeof(PeriodicTable).GetTypeInfo().Assembly;
                var elementStream = assembly.GetManifestResourceStream("Chemistry.Elements.XML.Elements.xml");
                //StreamReader ingredientReader = new StreamReader(ingredientStream);

                StorageFile sf = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"Chemistry.Elements\XML\Elements.xml");
                //Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("PeriodicTable.test.xml");
                //XmlDocument doc = new XmlDocument();
                //doc.Load(s);

                //  FileStream fs = new FileStream(@"C:\test.xml", FileMode.Open, FileAccess.Read);
                Type t = typeof(PeriodicTable);
                XmlSerializer ser = new XmlSerializer(typeof(PeriodicTable));
                /*
                FileRandomAccessStream stream = (FileRandomAccessStream)await sf.OpenAsync(FileAccessMode.Read);
                System.IO.Stream st = stream.AsStreamForRead();
                */
                aTable = (PeriodicTable)ser.Deserialize(elementStream);
            }
            catch (Exception e)
            {
                string s = e.Message;
                aTable = new PeriodicTable();
            }

            foreach (ChemElement e in aTable.Elements)
            {
                e.CreatePropertyList();
            }

            return aTable;
        }

        private ChemElement selElement;
        public ChemElement SelectedElement
        {
            get
            {
                return selElement;
            }
            set
            {
                selElement = value;
                NotifyPropertyChanged("SelectedElement");
            }
        }

        [XmlArray("Elements"), XmlArrayItem(ElementName = "Element", Type = typeof(ChemElement))]
        public ChemElement[] Elements
        {
            get;
            set;
        }

        /*
        public static async Task<PeriodicTable> Deserialize()
        {
            PeriodicTable el = null;
            try
            {
                StorageFile sf = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("Test.xml");
                //Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("PeriodicTable.test.xml");
                //XmlDocument doc = new XmlDocument();
                //doc.Load(s);

                //  FileStream fs = new FileStream(@"C:\test.xml", FileMode.Open, FileAccess.Read);
                Type t = typeof(PeriodicTable);
                XmlSerializer ser = new XmlSerializer(typeof(PeriodicTable));

                el = (PeriodicTable)ser.Deserialize(await sf.OpenStreamForReadAsync());


            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            //fs.Close();



            return el;
        }*/

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

    }
}
