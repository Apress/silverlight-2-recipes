using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Ch02_ProgrammingModel.Recipe2_12
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();
    }

    private void RetrieveXMLandLoad_Click(object sender, RoutedEventArgs e)
    {
      Uri location =
         new Uri("http://localhost:9090/xml/ApressBooks.xml", UriKind.Absolute);
      WebRequest request = HttpWebRequest.Create(location);
      request.BeginGetResponse(
          new AsyncCallback(this.RetrieveXmlCompleted), request);
    }

    void RetrieveXmlCompleted(IAsyncResult ar)
    {
      List<ApressBook> _apressBookList;
      HttpWebRequest request = ar.AsyncState as HttpWebRequest;
      WebResponse response = request.EndGetResponse(ar);
      Stream responseStream = response.GetResponseStream();
      using (StreamReader streamreader = new StreamReader(responseStream))
      {
        XDocument xDoc = XDocument.Load(streamreader);
        _apressBookList =
        (from b in xDoc.Descendants("ApressBook")
         select new ApressBook()
         {
           Author = b.Element("Author").Value,
           Title = b.Element("Title").Value,
           ISBN = b.Element("ISBN").Value,
           Description = b.Element("Description").Value,
           PublishedDate = Convert.ToDateTime(b.Element("DatePublished").Value),
           NumberOfPages = b.Element("NumPages").Value,
           Price = b.Element("Price").Value,
           ID = b.Element("ID").Value
         }).ToList();
      }
      //Could use Anonymous delegate (does same as below line of code)
      //Dispatcher.BeginInvoke(
      //  delegate()
      //  {
      //    DataBindListBox(_apressBookList);
      //  }
      // );
      //Use C# 3.0 Lambda
      Dispatcher.BeginInvoke(() => DataBindListBox(_apressBookList));
    }

    void DataBindListBox(List<ApressBook> list)
    {
      BooksListBox.ItemsSource = list;
    }
  }

  public class ApressBook
  {
    public string Author { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }
    public string Description { get; set; }
    public DateTime PublishedDate { get; set; }
    public string NumberOfPages { get; set; }
    public string Price { get; set; }
    public string ID { get; set; }
  }
}
