using System;
using System.Windows;
using System.Windows.Controls;

namespace Ch05_Controls.Recipe5_9
{
  public class WrapPanel : Panel
  {
    //Orientation dependency property
    DependencyProperty OrientationProperty =
      DependencyProperty.Register("Orientation", typeof(Orientation),
      typeof(WrapPanel),
      new PropertyMetadata(
        new PropertyChangedCallback(OrientationPropertyChangedCallback)));

    public Orientation Orientation
    {
      get
      {
        return (Orientation)GetValue(OrientationProperty);
      }
      set
      {
        SetValue(OrientationProperty, value);
      }
    }
    private static void OrientationPropertyChangedCallback(
      DependencyObject target, DependencyPropertyChangedEventArgs e)
    {
      //cause the layout to be redone on change of Orientation
      if (e.OldValue != e.NewValue)
        (target as WrapPanel).InvalidateMeasure();
    }

    public WrapPanel()
    {
      //initialize the orientation
      Orientation = Orientation.Horizontal;
    }
    protected override Size MeasureOverride(Size availableSize)
    {
      double DesiredWidth = 0;
      double DesiredHeight = 0;
      double RowHeight = 0;
      double RowWidth = 0;
      double ColHeight = 0;
      double ColWidth = 0;

      //call Measure() on each child - this is mandatory. 
      //get the true measure of things by passing in infinite sizing
      foreach (UIElement uie in this.Children)
        uie.Measure(availableSize);

      //for horizontal orientation - children laid out in rows
      if (Orientation == Orientation.Horizontal)
      {
        //iterate over children
        for (int idx = 0; idx < this.Children.Count; idx++)
        {

          //if we are at a point where adding the next child would 
          //put us at greater than the available width
          if (RowWidth + Children[idx].DesiredSize.Width
            >= availableSize.Width)
          {
            //set the desired width to the max of row width so far
            DesiredWidth = Math.Max(RowWidth, DesiredWidth);
            //accumulate the row height in preparation to move on to the next row
            DesiredHeight += RowHeight;
            //initialize the row height and row width for the next row iteration
            RowWidth = 0;
            RowHeight = 0;
          }
          //if on the other hand we are within width bounds
          if (RowWidth + Children[idx].DesiredSize.Width
            < availableSize.Width)
          {
            //increment the width of the current row by the child's width
            RowWidth += Children[idx].DesiredSize.Width;
            //set the row height if this child is taller 
            //than the others in the row so far
            RowHeight = Math.Max(RowHeight,
              Children[idx].DesiredSize.Height);


          }
          //this means we ran out of children in the middle or exactly at the end 
          //of a row
          if (RowWidth != 0 && RowHeight != 0)
          {
            //account for the last row
            DesiredWidth = Math.Max(RowWidth, DesiredWidth);
            DesiredHeight += RowHeight;
          }

        }
      }
      else //vertical orientation - children laid out in columns
      {
        //iterate over children
        for (int idx = 0; idx < this.Children.Count; idx++)
        {
          //if we are at a point where adding the next child would 
          //put us at greater than the available height
          if (ColHeight + Children[idx].DesiredSize.Height
            >= availableSize.Height)
          {
            //set the desired height to max of column height so far           
            DesiredHeight = Math.Max(ColHeight, DesiredHeight);
            //accumulate the column width in preparation to 
            //move on to the next column
            DesiredWidth += ColWidth;
            //initialize the column height and column width for the 
            //next column iteration
            ColHeight = 0;
            ColWidth = 0;
          }
          //if on the other hand we are within height bounds
          if (ColHeight + Children[idx].DesiredSize.Height
            < availableSize.Height)
          {
            //increment the height of the current column by the child's height
            ColHeight += Children[idx].DesiredSize.Height;
            //set the column width if this child is wider 
            //than the others in the column so far
            ColWidth = Math.Max(ColWidth,
              Children[idx].DesiredSize.Width);
          }
        }
        //this means we ran out of children in the middle or exactly at the end 
        //of a column
        if (RowWidth != 0 && RowHeight != 0)
        {
          //account for the last row
          DesiredHeight = Math.Max(ColHeight, DesiredHeight);
          DesiredWidth += ColWidth;
        }
      }

      //return the desired size
      return new Size(DesiredWidth, DesiredHeight);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      double ChildX = 0;
      double ChildY = 0;
      double FinalHeight = 0;
      double FinalWidth = 0;
      //horizontal orientation - children in rows
      if (Orientation == Orientation.Horizontal)
      {
        double RowHeight = 0;
        //iterate over children
        for (int idx = 0; idx < this.Children.Count; idx++)
        {
          //if we are about to go beyond width bounds with the next child
          if (ChildX + Children[idx].DesiredSize.Width
            >= finalSize.Width)
          {
            //move to next row 
            ChildY += RowHeight;
            FinalHeight += RowHeight;
            FinalWidth = Math.Max(FinalWidth, ChildX);
            //shift to the left edge to start next row
            ChildX = 0;
          }
          //if we are within width bounds
          if (ChildX + Children[idx].DesiredSize.Width
            < finalSize.Width)
          {
            //lay out child at the current X,Y coords with 
            //the desired width and height
            Children[idx].Arrange(new Rect(ChildX, ChildY,
              Children[idx].DesiredSize.Width,
              Children[idx].DesiredSize.Height));
            //increment X value to position next child horizontally right after the 
            //currently laid out child
            ChildX += Children[idx].DesiredSize.Width;
            //set the row height if this child is taller 
            //than the others in the row so far
            RowHeight = Math.Max(RowHeight,
              Children[idx].DesiredSize.Height);
          }
        }
      }
      else //vertical orientation - children in columns
      {
        double ColWidth = 0;
        //iterate over children
        for (int idx = 0; idx < this.Children.Count; idx++)
        {
          //if we are about to go beyond height bounds with the next child
          if (ChildY + Children[idx].DesiredSize.Height
            >= finalSize.Height)
          {
            //move to next column 
            ChildX += ColWidth;
            FinalWidth += ColWidth;
            FinalHeight = Math.Max(FinalHeight, ChildY);
            //shift to the top edge to start next column
            ChildY = 0;
          }
          //if we are within height bounds
          if (ChildY + Children[idx].DesiredSize.Height
            < finalSize.Height)
          {
            //lay out child at the current X,Y coords with 
            //the desired width and height
            Children[idx].Arrange(new Rect(ChildX, ChildY,
              Children[idx].DesiredSize.Width,
              Children[idx].DesiredSize.Height));
            //increment Y value to position next child vertically right below the 
            //currently laid out child
            ChildY += Children[idx].DesiredSize.Height;
            //set the column width if this child is wider 
            //than the others in the column so far
            ColWidth = Math.Max(ColWidth,
              Children[idx].DesiredSize.Width);
          }
        }
      }
      //return the original final size
      return finalSize;
    }
  }
}
