using System.Drawing;
using System.Windows.Forms;

namespace SeriesUpdater.Context
{
    class Forms
    {
        public static Label createLabel(string name, string text, bool isHeader)
        {
            Label newLabel = new Label();
            newLabel.Anchor = AnchorStyles.Top;
            newLabel.AutoSize = true;
            newLabel.AutoEllipsis = true;
            newLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, (isHeader ? FontStyle.Bold : FontStyle.Regular) | (isHeader ? FontStyle.Underline : FontStyle.Regular), GraphicsUnit.Point, ((byte)(238)));
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

            if (clickable)
            {
                newImage.Click += Form1.deleteImage_Click;
            }

            return newImage;
        }

        public static void placeForm(bool isFormClosed)
        {
            Form mainForm = Application.OpenForms[0];

            if (Cursor.Position.X + (mainForm.Width / 2) <= Screen.PrimaryScreen.WorkingArea.Width - 10 && !isFormClosed)
            {
                mainForm.Left = Cursor.Position.X - (mainForm.Width / 2);
            }

            else
            {
                mainForm.Left = Screen.PrimaryScreen.WorkingArea.Width - mainForm.Width - 10;
            }

            mainForm.Top = Screen.PrimaryScreen.WorkingArea.Height - mainForm.Height - 10;
        }

        public static void applyData(bool isAdd)
        {
            Form mainForm = Application.OpenForms[0];

            if (MainProgram.Variables.seriesList.Count > 0)
            {
                if (mainForm.Controls.Find("createSeriesTable", true).Length == 0)
                {
                    Label nameHeaderLabel = Context.Forms.createLabel("nameHeaderLabel", "Név", true);
                    Label lastViewedHeaderLabel = Context.Forms.createLabel("lastViewedHeaderLabel", "Legutóbb megtekintett", true);
                    Label lastEpisodeHeaderLabel = Context.Forms.createLabel("lastEpisodeHeaderLabel", "Legújabb", true);

                    TableLayoutPanel seriesTable = Context.Forms.createTableLayoutPanel(nameHeaderLabel, lastViewedHeaderLabel, lastEpisodeHeaderLabel);
                    Application.OpenForms[0].Controls.Add(seriesTable);
                }
            }

            int forStart = 0;

            if (isAdd)
            {
                forStart = MainProgram.Variables.seriesList.Count - 1;
            }

            else
            {
                forStart = 0;
            }

            for (int i = forStart; i < MainProgram.Variables.seriesList.Count; i++)
            {
                Label nameLabel = Context.Forms.createLabel("name_" + MainProgram.Variables.seriesList[i].id, MainProgram.Variables.seriesList[i].name, false);
                Label lastViewedLabel = Context.Forms.createLabel("lastViewed_" + MainProgram.Variables.seriesList[i].id, "S" + MainProgram.Variables.seriesList[i].lastViewed[0] + "E" + MainProgram.Variables.seriesList[i].lastViewed[1], false);

                Label lastEpLabel = new Label();
                if (MainProgram.Variables.seriesList[i].lastEpisode != default(int[]))
                {
                    lastEpLabel = Context.Forms.createLabel("lastEp_" + MainProgram.Variables.seriesList[i].id, "S" + MainProgram.Variables.seriesList[i].lastEpisode[0] + "E" + MainProgram.Variables.seriesList[i].lastEpisode[1], false);
                }

                else
                {
                    lastEpLabel = Context.Forms.createLabel("lastEp_" + MainProgram.Variables.seriesList[i].id, "", false);
                }

                TableLayoutPanel seriesTable = (TableLayoutPanel)mainForm.Controls.Find("createSeriesTable", true)[0];
                seriesTable.Controls.Add(nameLabel, 0, i + 1);
                seriesTable.Controls.Add(lastViewedLabel, 1, i + 1);
                seriesTable.Controls.Add(lastEpLabel, 2, i + 1);

                if (MainProgram.Variables.seriesList[i].lastEpisode[0] > MainProgram.Variables.seriesList[i].lastViewed[0] || (MainProgram.Variables.seriesList[i].lastEpisode[0] == MainProgram.Variables.seriesList[i].lastViewed[0] && MainProgram.Variables.seriesList[i].lastEpisode[1] > MainProgram.Variables.seriesList[i].lastViewed[1]))
                {
                    lastEpLabel.Font = new Font(lastEpLabel.Font, FontStyle.Bold | FontStyle.Underline);
                    lastEpLabel.Width = lastEpLabel.PreferredWidth;

                    /*
                    PictureBox newPicture = ControlManagement.NewControls.createPictureBox("new_" + Variables.PublicVariables.seriesList[i].id, MainProgram.Properties.Resources.uj_másolat, createSeriesTable.Left + lastEpLabel.Left + lastEpLabel.Width + 2, createSeriesTable.Top + lastEpLabel.Top + 1, 10);
                    newPicture.BringToFront();
                     */
                }

                Control currRowLabel = mainForm.Controls.Find("name_" + MainProgram.Variables.seriesList[i].id, true)[0];
                PictureBox deleteImage = Context.Forms.createPictureBox("delete_" + MainProgram.Variables.seriesList[i].id, SeriesUpdater.Properties.Resources.delete1, 2, currRowLabel.Top + seriesTable.Top, 15, true);
                mainForm.Controls.Add(deleteImage);
            }
        }
    }
}
