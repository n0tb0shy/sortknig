using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BookCollection.Data;
using BookCollection.Models;

namespace BookCollection.Forms
{
    public partial class MainForm : Form
    {
        private readonly string _currentUser;
        private readonly Database _database;
        private List<Book> _books;

        private Label lblUsername;
        private ListBox lstBooks;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnView;
        private TextBox txtSearch;
        private Button btnSearch;

        public MainForm(string username, Database database)
        {
            _currentUser = username;
            _database = database;
            InitializeForm();
            LoadBooks();
        }

        private void InitializeForm()
        {
            // Настройка формы
            Text = $"Книжная коллекция - {_currentUser}";
            ClientSize = new Size(700, 500);
            StartPosition = FormStartPosition.CenterScreen;
            FormClosing += (s, e) => Application.Exit();

            // Элементы управления
            lblUsername = new Label
            {
                Text = $"Пользователь: {_currentUser}",
                Location = new Point(20, 20),
                AutoSize = true
            };

            txtSearch = new TextBox
            {
                Location = new Point(20, 50),
                Size = new Size(450, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            btnSearch = new Button
            {
                Text = "Поиск",
                Location = new Point(480, 50),
                Size = new Size(80, 23),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnSearch.Click += BtnSearch_Click;

            lstBooks = new ListBox
            {
                Location = new Point(20, 80),
                Size = new Size(540, 350),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            btnAdd = new Button
            {
                Text = "Добавить",
                Location = new Point(580, 80),
                Size = new Size(100, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnAdd.Click += BtnAdd_Click;

            btnEdit = new Button
            {
                Text = "Редактировать",
                Location = new Point(580, 120),
                Size = new Size(100, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnEdit.Click += BtnEdit_Click;

            btnDelete = new Button
            {
                Text = "Удалить",
                Location = new Point(580, 160),
                Size = new Size(100, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnDelete.Click += BtnDelete_Click;

            btnView = new Button
            {
                Text = "Просмотр",
                Location = new Point(580, 200),
                Size = new Size(100, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnView.Click += BtnView_Click;

            // Добавление элементов на форму
            Controls.Add(lblUsername);
            Controls.Add(txtSearch);
            Controls.Add(btnSearch);
            Controls.Add(lstBooks);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(btnView);
        }

        private void LoadBooks()
        {
            try
            {
                _books = _database.LoadBooks();
                UpdateBookList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки книг: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateBookList()
        {
            lstBooks.Items.Clear();
            foreach (var book in _books)
            {
                lstBooks.Items.Add($"{book.Title} - {book.Author} ({book.Year})");
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new BookDetailsForm(new Book(), true);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _books.Add(form.Book);
                    _database.SaveBooks(_books);
                    UpdateBookList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления книги: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstBooks.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите книгу для редактирования", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedBook = _books[lstBooks.SelectedIndex];
                var form = new BookDetailsForm(selectedBook, true);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _database.SaveBooks(_books);
                    UpdateBookList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка редактирования книги: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstBooks.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите книгу для удаления", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Удалить выбранную книгу?", "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _books.RemoveAt(lstBooks.SelectedIndex);
                    _database.SaveBooks(_books);
                    UpdateBookList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления книги: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstBooks.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите книгу для просмотра", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedBook = _books[lstBooks.SelectedIndex];
                new BookDetailsForm(selectedBook, false).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка просмотра книги: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                UpdateBookList();
                return;
            }

            lstBooks.Items.Clear();
            foreach (var book in _books)
            {
                if (book.Title.ToLower().Contains(searchText) ||
                    book.Author.ToLower().Contains(searchText) ||
                    (book.Genre?.ToLower().Contains(searchText) ?? false))
                {
                    lstBooks.Items.Add($"{book.Title} - {book.Author} ({book.Year})");
                }
            }
        }
    }
}