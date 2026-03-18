using Polirubro.Datos;
using Polirubro.Entidades;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PolirubroWPF
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            Loaded += (s, e) => txtEmail.Focus();
        }


        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            Ingresar();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Ingresar();
            }
        }

        private void Ingresar()
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password;

            using (var db = new PolirubroDbContext())
            {
                if (!db.Usuarios.Any())
                {
                    string hash = BCrypt.Net.BCrypt.HashPassword("Admin123!");

                    db.Usuarios.Add(new Usuario
                    {
                        Email = "admin@admin.com",
                        PasswordHash = hash,
                        EsAdmin = true,
                        Activo = true,
                        Nombre = "Administrador"
                    });
                db.SaveChanges();
                
                 MessageBox.Show("Usuario admin creado");
                 return;
            }
        }

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Completá email y contraseña.");
                return;
            }

            using (var db = new PolirubroDbContext())
            {
                var usuario = db.Usuarios.FirstOrDefault(u => u.Email == email && u.Activo);

                if (usuario == null)
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.");
                    return;
                }

                bool passwordValida = BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash);
                
                if (!passwordValida)
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.");
                    return;

                }
                Sesion.UsuarioActual = usuario;
            }

            DialogResult = true;
            Close();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}