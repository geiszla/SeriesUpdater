using System.Drawing;
using System.Windows.Forms;

namespace SeriesUpdater.Context
{
    class Controls
    {
        public static Label createLabel(string name, string text, bool isHeader)
        {
            Label newLabel = new Label();
            newLabel.Anchor = AnchorStyles.Top;
            newLabel.AutoSize = true;
            newLabel.AutoEllipsis = true;
            newLabel.Font = new Font("Microsoft Sans Serif", 8.25F, (isHeader ? FontStyle.Bold : FontStyle.Regular) | (isHeader ? FontStyle.Underline : FontStyle.Regular), GraphicsUnit.Point, ((byte)(238)));
            newLabel.Margin = new Padding(3, 0, 3, 10);
            newLabel.Name = name;
            newLabel.Text = text;
            newLabel.MaximumSize = new System.Drawing.Size(150, 50);

            return newLabel;
        }

        public static TableLayoutPanel createTableLayoutPanel(params Label[] headers)
        {
            TableLayoutPanel newTable = new TableLayoutPanel();
            newTable.AutoSize = true;
            newTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            for (int i = 0; i < headers.Length; i++)
            {
                newTable.Controls.Add(headers[i], i, 0);
            }

            newTable.Location = new Point(15, 47);
            newTable.Name = "createSeriesTable";

            return newTable;
        }

        public static PictureBox createPictureBox(string name, Bitmap image, int left, int top, int size, bool clickable)
        {
            PictureBox newImage = new PictureBox();
            newImage.Cursor = clickable ? Cursors.Hand : Cursors.Arrow;
            newImage.Height = size;
            newImage.Width = size;
            newImage.Image = image;
            newImage.Left = left;
            newImage.Top = top;
            newImage.Name = name;

            return newImage;
        }
    }
}
