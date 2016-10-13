using System.IO;
using System.ServiceModel.Activation;
using System.Web;
using System.Xml; 

namespace Ch08_RichMedia.Recipe8_2
{
  [AspNetCompatibilityRequirements(
    RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
  
  public class MediaLocationProvider : IMediaLocationProvider
  {

    #region IMediaLocationProvider Members

    public XmlDocument GetLocationList()
    {
      //open the local data file
      StreamReader stmrdr =
        new StreamReader(
          new FileStream(
            HttpContext.Current.Request.MapPath("App_Data/Locations.xml"),
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
