using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using musicDBManagementSystem.project.controller;
using musicDBManagementSystem.project.graphic;

namespace musicDBManagementSystem.project {
    public class Graphic {
        private Controller controller;
        
        private Form mainForm;
        private TableLayoutPanel mainPanel;
        
        private BasicLabel formationLabel;
        private BasicDataGridView formationTable;

        private BasicLabel genreLabel;
        private BasicDataGridView genreTable;

        private Label artistLabel;
        private BasicDataGridView artistTable;

        private BasicLabel artistDataLabel;
        private BasicLabel artistNameLabel;
        private BasicTextBox artistNameEdit;
        private BasicButton addArtistButton;
        private BasicButton removeArtistButton;
        private BasicButton updateArtistButton;

        public Graphic(Controller ctrl, string title) {
            this.controller = ctrl;
            this.initializeMain(title);
            this.initializeTable();
            this.initializeControls();
            this.refreshTables();
        }

        private void initializeMain(string title) {
            this.mainPanel = new TableLayoutPanel {Anchor = 
                    AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 4
            };
            this.mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            this.mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            this.mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            this.mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            this.mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            this.mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            this.mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            
            this.mainForm = new Form() {Text = title ?? "WinForms App"};
            this.mainForm.Controls.Add(this.mainPanel);
            this.mainForm.FormClosing += this.onClose;
            this.mainForm.Size = new Size(1000, 500);
            this.mainForm.MinimumSize = new Size(800, 400);
        }
        
        private void initializeTable() {
            this.formationLabel = new BasicLabel("Formations");
            this.formationTable = new BasicDataGridView();
            this.formationTable.SelectionChanged += this.formationOrGenreSelected;
            
            this.genreLabel = new BasicLabel("Genres");
            this.genreTable = new BasicDataGridView();
            this.genreTable.SelectionChanged += this.formationOrGenreSelected;
            
            this.artistLabel = new BasicLabel("Artists");
            this.artistTable = new BasicDataGridView();

            this.mainPanel.Controls.Add(this.formationLabel, 0, 0);
            this.mainPanel.Controls.Add(this.genreLabel, 1, 0);
            this.mainPanel.Controls.Add(this.artistLabel, 2, 0);
            
            this.mainPanel.Controls.Add(this.formationTable, 0, 1);
            this.mainPanel.Controls.Add(this.genreTable, 1, 1);
            this.mainPanel.Controls.Add(this.artistTable, 2, 1);
        }

        private void initializeControls() {
            this.artistDataLabel = new BasicLabel("Artist Data");
            
            this.artistNameLabel = new BasicLabel("Name");
            this.artistNameEdit = new BasicTextBox("Type artist name... ");

            this.addArtistButton = new BasicButton("Add");
            this.addArtistButton.Click += this.addArtist;
            new ToolTip().SetToolTip(this.addArtistButton, "Add new Artist with given Name for selected Formation and Genre.");
            this.updateArtistButton = new BasicButton("Update");
            this.updateArtistButton.Click += this.updateArtist;
            new ToolTip().SetToolTip(this.updateArtistButton, "Update Name of the selected Artist with given Name.");
            this.removeArtistButton = new BasicButton("Remove");
            this.removeArtistButton.Click += this.removeArtist;
            new ToolTip().SetToolTip(this.removeArtistButton, "Remove selected Artist.");
            
            this.mainPanel.Controls.Add(this.artistDataLabel, 0, 2);
            
            this.mainPanel.Controls.Add(this.artistNameLabel, 1, 2);
            this.mainPanel.Controls.Add(this.artistNameEdit, 2, 2);

            this.mainPanel.Controls.Add(this.addArtistButton, 0, 3);
            this.mainPanel.Controls.Add(this.updateArtistButton, 1, 3);
            this.mainPanel.Controls.Add(this.removeArtistButton, 2, 3);
        }

        private void refreshTables() {
            this.formationTable.Columns.Add("title", "Formation");
            this.formationTable.Columns.Add("about", "Description");
            this.refreshFormation();
            
            this.genreTable.Columns.Add("title", "Genre");
            this.genreTable.Columns.Add("about", "Description");
            this.refreshGenre();
            
            this.artistTable.Columns.Add("title", "Artist");
            this.refreshArtist();
        }

        private void refreshFormation() {
            this.formationTable.Rows.Clear();
            foreach (DataRow row in this.controller.getFormationRows()) {
                this.formationTable.Rows.Add(row[0], row[1]);
            }
        }

        private void refreshGenre() {
            this.genreTable.Rows.Clear();
            foreach (DataRow row in this.controller.getGenreRows()) {
                this.genreTable.Rows.Add(row[0], row[1]);
            }
        }
        
        private void refreshArtist() {
            this.artistTable.Rows.Clear();
            foreach (DataRow row in this.controller.getArtistRows()) {
                this.artistTable.Rows.Add(row[0]);
            }
        }
        
        private void addArtist(object sender, EventArgs e) {
            string title = this.artistNameEdit.Text;
            int formationId;
            int genreId;
            try {
                formationId = this.controller.getFormationId(
                    this.formationTable.SelectedRows[0].Cells[0].Value.ToString().Trim());
                genreId = this.controller.getGenreId(
                    this.genreTable.SelectedRows[0].Cells[0].Value.ToString().Trim());
            } catch (ArgumentOutOfRangeException) {
                this.showAlert("Select Formation and Genre of Artist!");
                return;
            }
            this.controller.addArtist(title, formationId, genreId);
            this.refreshArtist();
        }


        private void updateArtist(object sender, EventArgs e) {
            string title = this.artistNameEdit.Text;
            int artistId;
            int formationId;
            int genreId;
            try {
                artistId = this.controller.getArtistId(
                    this.artistTable.SelectedRows[0].Cells[0].Value.ToString().Trim());
                formationId = this.controller.getFormationId(
                    this.formationTable.SelectedRows[0].Cells[0].Value.ToString().Trim());
                genreId = this.controller.getGenreId(
                    this.genreTable.SelectedRows[0].Cells[0].Value.ToString().Trim());
            } catch (ArgumentOutOfRangeException) {
                this.showAlert("Select Formation and Genre and Artist to Update!");
                return;
            }
            this.controller.updateArtist(artistId, title, formationId, genreId);
            this.refreshArtist();
        }
        
        private void removeArtist(object sender, EventArgs e) {
            int artistId;
            try {
                artistId = this.controller.getArtistId(
                    this.artistTable.SelectedRows[0].Cells[0].Value.ToString().Trim());
            } catch (ArgumentOutOfRangeException) {
                this.showAlert("Select Artist to Delete!");
                return;
            }
            this.controller.removeArtist(artistId);
            this.refreshArtist();
        }
        
        private void formationOrGenreSelected(object sender, EventArgs e) {
            try {
                List<int> formationIds = new List<int>();
                try {
                    foreach (DataGridViewRow row in this.formationTable.SelectedRows) {
                        formationIds.Add(this.controller.getFormationId(
                            row.Cells[0].Value.ToString().Trim()));
                    }
                } catch (ArgumentOutOfRangeException) {}

                List<int> genreIds = new List<int>();
                try {
                    foreach (DataGridViewRow row in this.genreTable.SelectedRows) {
                        genreIds.Add(this.controller.getGenreId(
                            row.Cells[0].Value.ToString().Trim()));
                    }
                } catch(ArgumentOutOfRangeException) {}

                if (formationIds.Count > 0 || genreIds.Count > 0) {
                    this.artistTable.Rows.Clear();
                    foreach (DataRow row in this.controller.getArtistsOfFormationAndGenre(formationIds, genreIds)) {
                        this.artistTable.Rows.Add(row[0]);
                    }
                }
                else {
                    this.refreshArtist();
                }
            } catch (Exception exception) {
                Console.WriteLine(exception);
            }
            
        }
        
        private void showAlert(string message) {
            MessageBox.Show(message);
        }
        
        public Form getMain() {
            return this.mainForm;
        }

        private void onClose(object sender, System.ComponentModel.CancelEventArgs e) {
            this.controller.close();
        }

    }
}