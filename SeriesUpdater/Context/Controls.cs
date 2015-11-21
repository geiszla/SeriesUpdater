using System.Drawing;
using System.Windows.Forms;

namespace SeriesUpdater.Context
{
    class Controls
    {
        public static Label CreateLabel(string Name, string Text, bool IsHeader)
        {
            Label newLabel = new Label();
            newLabel.Anchor = AnchorStyles.Top;
            newLabel.AutoSize = true;
            newLabel.AutoEllipsis = true;
            newLabel.Font = new Font("Microsoft Sans Serif", 8.25F,
                (IsHeader ? FontStyle.Bold : FontStyle.Regular) | (IsHeader ? FontStyle.Underline : FontStyle.Regular),
                GraphicsUnit.Point, 238);
            newLabel.Margin = new Padding(3, 0, 3, 10);
            newLabel.Name = Name;
            newLabel.Text = Text;
            newLabel.MaximumSize = new Size(150, 15);

            return newLabel;
        }

        public static TextBox CreateTextBox(string Name, string Text)
        {
            TextBox newTextBox = new TextBox();
            newTextBox.Anchor = AnchorStyles.Top;
            newTextBox.Margin = new Padding(3, 0, 3, 10);
            newTextBox.Name = Name;
            newTextBox.Text = Text;
            newTextBox.Size = newTextBox.PreferredSize;

            return newTextBox;
        }

        public static TableLayoutPanel CreateTableLayoutPanel(params Label[] Headers)
        {
            TableLayoutPanel newTable = new TableLayoutPanel();
            newTable.AutoSize = true;
            newTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            for (int i = 0; i < Headers.Length; i++)
            {
                newTable.Controls.Add(Headers[i], i, 0);
            }

            newTable.Location = new Point(15, 47);
            newTable.Name = "seriesTable";

            return newTable;
        }

        public static PictureBox CreatePictureBox(string Name, Bitmap Image, int Left, int Top, int Size, bool IsClickable)
        {
            PictureBox newImage = new PictureBox();
            newImage.Cursor = IsClickable ? Cursors.Hand : Cursors.Arrow;
            newImage.Height = Size;
            newImage.Width = Size;
            newImage.Image = Image;
            newImage.SizeMode = PictureBoxSizeMode.Zoom;
            newImage.Left = Left;
            newImage.Top = Top;
            newImage.Name = Name;

            return newImage;
        }
    }
}
