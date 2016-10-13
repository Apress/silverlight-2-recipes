using System;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Ch03_DesigningUX.Recipe3_12
{
  [Serializable]
  public class StrokeSerializer
  {
    public Color Color;
    public Color OutlineColor;
    public Double Witdh;
    public Double Height;

    private List<StylusPointsSerializer> _stylusPoints;
    public List<StylusPointsSerializer> StylusPoints
    {
      get
      {
        if (_stylusPoints != null)
          return _stylusPoints;
        else
        {
          _stylusPoints = new List<StylusPointsSerializer>();
          return _stylusPoints;
        }
      }
    }
  }

  public struct StylusPointsSerializer
  {
    public StylusPointsSerializer(Double x, Double y)
    {
      X = x;
      Y = y;
    }
    Double X;
    Double Y;
  }
}
