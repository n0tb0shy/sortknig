using System;
using System.Drawing;
using System.Windows.Forms;
using BookCollection.Models;

namespace BookCollection.Forms
{
    public partial class BookDetailsForm : Form
    {
        public Book Book { get; private set; }
        private readonly bool _isEditing;

        private TextBox txtTitle;
        private TextBox txtAuthor;
        private NumericUpDown numYear;
        private TextBox txtGenre;
        private TextBox txtDescription;
        private CheckBox chkIsRead;
        private NumericUpDown numRating;
        private Button btnSave;
        private Button btnCancel;

        public BookDetailsForm(Book book, bool isEditing)
        {
            Book = book;
            _isEditing = isEditing;
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Настройка формы
            Text = _isEditing ? "Редактирование книги" : "Просмотр книги";
            ClientSize = new Size(450, 400);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            // Элементы управления
            var lblTitle = new Label
            {
                Text = "Название:",
                Location = new Point(20, 20),
                AutoSize = true
            };

            txtTitle = new TextBox
            {
                Location = new Point(20, 40),
                Size = new Size(400, 20),
                ReadOnly = !_isEditing,
                Text = Book.Title
            };

            var lblAuthor = new Label
            {
                Text = "Автор:",
                Location = new Point(20, 70),
                AutoSize = true
            };

            txtAuthor = new TextBox
            {
                Location = new Point(20, 90),
                Size = new Size(400, 20),
                ReadOnly = !_isEditing,
                Text = Book.Author
            };

            var lblYear = new Label
            {
                Text = "Год издания:",
                Location = new Point(20, 120),
                AutoSize = true
            };

            numYear = new NumericUpDown
            {
                Location = new Point(20, 140),
                Size = new Size(100, 20),
                Minimum = 0,
                Maximum = DateTime.Now.Year,
                Value = Book.Year,
                ReadOnly = !_isEditing
            };

            var lblGenre = new Label
            {
                Text = "Жанр:",
                Location = new Point(140, 120),
                AutoSize = true
            };

            txtGenre = new TextBox
            {
                Location = new Point(140, 140),
                Size = new Size(280, 20),
                ReadOnly = !_isEditing,
                Text = Book.Genre
            };

            var lblDescription = new Label
            {
                Text = "Описание:",
                Location = new Point(20, 170),
                AutoSize = true
            };

            txtDescription = new TextBox
            {
                Location = new Point(20, 190),
                Size = new Size(400, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = !_isEditing,
                Text = Book.Description
            };

            chkIsRead = new CheckBox
            {
                Text = "Прочитана",
                Location = new Point(20, 300),
                AutoSize = true,
                Checked = Book.IsRead,
                Enabled = _isEditing
            };

            var lblRating = new Label
            {
                Text = "Рейтинг:",
                Location = new Point(20, 330),
                AutoSize = true
            };

            numRating = new NumericUpDown
            {
                Location = new Point(80, 330),
                Size = new Size(50, 20),
                Minimum = 0,
                Maximum = 10,
                Value = Book.Rating,
                ReadOnly = !_isEditing
            };

            btnSave = new Button
            {
                Text = _isEditing ? "Сохранить" : "Закрыть",
                Location = new Point(250, 330),
                Size = new Size(80, 30)
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(340, 330),
                Size = new Size(80, 30)
            };
            btnCancel.Click += BtnCancel_Click;

            // Добавление элементов на форму
            Controls.Add(lblTitle);
            Controls.Add(txtTitle);
            Controls.Add(lblAuthor);
            Controls.Add(txtAuthor);
            Controls.Add(lblYear);
            Controls.Add(numYear);
            Controls.Add(lblGenre);
            Controls.Add(txtGenre);
            Controls.Add(lblDescription);
            Controls.Add(txtDescription);
            Controls.Add(chkIsRead);
            Controls.Add(lblRating);
            Controls.Add(numRating);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (_isEditing)
            {
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Введите название книги", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Book.Title = txtTitle.Text.Trim();
                Book.Author = txtAuthor.Text.Trim();
                Book.Year = (int)numYear.Value;
                Book.Genre = txtGenre.Text.Trim();
                Book.Description = txtDescription.Text.Trim();
                Book.IsRead = chkIsRead.Checked;
                Book.Rating = (int)numRating.Value;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}