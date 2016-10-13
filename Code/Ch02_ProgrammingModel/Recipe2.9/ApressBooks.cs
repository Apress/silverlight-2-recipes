using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Collections;

namespace Ch02_ProgrammingModel.Recipe2_9
{
  public class ApressBooks
  {
    private List<ApressBook> _apressBookList;
    public List<ApressBook> ApressBookList
    {
      get
      {
        if (null == _apressBookList)
          RetrieveData();
        return _apressBookList;
      }
    }

    private void RetrieveData()
    {
      XmlReaderSettings XmlRdrSettings = new XmlReaderSettings();
      XmlRdrSettings.XmlResolver = new XmlXapResolver();
      XmlReader reader = XmlReader.Create("ApressBooks.xml", XmlRdrSettings);

      XDocument xDoc = XDocument.Load(reader);

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
