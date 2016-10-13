using System.IO;
using System.ServiceModel.Activation;
using System.Web;
using System.Linq;
using System.Xml;
using System.Xml.Linq; 

namespace Ch08_RichMedia.Recipe8_5
{
  [AspNetCompatibilityRequirements(
    RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
  
  public class MediaLocationProvider : IMediaLocationProvider
  {

    #region IMediaLocationProvider Members

    public XmlDocument GetDownloadsList()
    {
      //open the local data file
      StreamReader stmrdr =
        new StreamReader(
          new FileStream(
            HttpContext.Current.Request.MapPath("App_Data/Downloads.xml"),
            FileMode.Open));
      //creat and load an XmlDocument
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(stmrdr.ReadToEnd());
      stmrdr.Close();

      //return the document
      HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
      return xDoc;
    }

    #endregion

    #region IMediaLocationProvider Members


    public XmlDocument GetOnDemandStreamsList()
    { 
      //open the local data file
      StreamReader stmrdr =
        new StreamReader(
          new FileStream(
            HttpContext.Current.Request.MapPath("App_Data/OnDemandStreams.xml"),
            FileMode.Open));
      //creat and load an XmlDocument
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(stmrdr.ReadToEnd());
      stmrdr.Close();

      //return the document
      HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
      return xDoc;
     
    }

    #endregion

    #region IMediaLocationProvider Members


    public XmlDocument GetBroadcastStreamsList()
    {
      //open the local data file
      StreamReader stmrdr =
        new StreamReader(
          new FileStream(
            HttpContext.Current.Request.MapPath("App_Data/BroadcastStreams.xml"),
            FileMode.Open));
      //creat and load an XmlDocument
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(stmrdr.ReadToEnd());
      stmrdr.Close();

      //return the document
      HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
      return xDoc;
    }

    #endregion

    #region IMediaLocationProvider Members


    public XmlDocument GetCaptionsForMedia(string Id)
    {
      XmlDocument retVal = new XmlDocument();
      XDocument xDoc = XDocument.Load(HttpContext.Current.Request.MapPath("App_Data/Captions.xml"));
      var medialist = from med in xDoc.Root.Elements()
                       where med.Attribute("Id").Value.Equals(Id)
                       select med;
      if (medialist.Count() > 0)
      {
        XElement media = medialist.ToList()[0];        
        retVal.LoadXml(media.ToString());
        return retVal;
      }
      else
        return null;
    }

    #endregion

    #region IMediaLocationProvider Members


    public XmlDocument GetCommercial(string MarkerType)
    {
      XmlDocument retVal = new XmlDocument();
      XDocument xDoc = XDocument.Load(HttpContext.Current.Request.MapPath("App_Data/Commercials.xml"));
      var medialist = from med in xDoc.Root.Elements()
                      where med.Attribute("Type").Value.Equals(MarkerType)
                      select med;
      if (medialist.Count() > 0)
      {
        XElement media = medialist.ToList()[0];
        retVal.LoadXml(media.ToString());
        return retVal;
      }
      else
      {
        return null;
      }
      
    }

    #endregion
  }
}
