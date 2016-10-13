using System.IO;
using System.ServiceModel.Activation;
using System.Web;
using System.Xml; 

namespace Ch08_RichMedia.Recipe8_4
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
  }
}
