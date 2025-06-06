using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BookCollection.Data;
using BookCollection.Models;

namespace BookCollection.Forms
{
    public partial class LoginForm : Form
    {
        private readonly Database _database;
        private List<User> _users;

        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;
        private Label lblUsername;
        private Label lblPassword;

        public LoginForm()
        {
            InitializeForm();
            _database = new Database();
            _users = _database.LoadUsers();
        }

        private void InitializeForm()
        {
            // Настройка формы
            Text = "Вход в систему";
            ClientSize = new Size(350, 220);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            FormClosing += (s, e) => Application.Exit();

            // Элементы управления
            lblUsername = new Label
            {
                Text = "Имя пользователя:",
                Location = new Point(30, 30),
                AutoSize = true
            };

            txtUsername = new TextBox
            {
                Location = new Point(30, 50),
                Size = new Size(290, 20)
            };

            lblPassword = new Label
            {
                Text = "Пароль:",
                Location = new Point(30, 80),
                AutoSize = true
            };

            txtPassword = new TextBox
            {
                Location = new Point(30, 100),
                Size = new Size(290, 20),
                PasswordChar = '*'
            };

            btnLogin = new Button
            {
                Text = "Войти",
                Location = new Point(30, 140),
                Size = new Size(130, 30)
            };
            btnLogin.Click += BtnLogin_Click;

            btnRegister = new Button
            {
                Text = "Регистрация",
                Location = new Point(190, 140),
                Size = new Size(130, 30)
            };
            btnRegister.Click += BtnRegister_Click;

            // Добавление элементов на форму
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
            Controls.Add(btnRegister);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите имя пользователя и пароль", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User user = _users.Find(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                Hide();
                new MainForm(username, _database).Show();
            }
            else
            {
                MessageBox.Show("Неверные учетные данные", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите имя пользователя и пароль", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_users.Exists(u => u.Username == username))
            {
                MessageBox.Show("Пользователь уже существует", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _users.Add(new User { Username = username, Password = password });
            _database.SaveUsers(_users);
            MessageBox.Show("Регистрация успешна", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}