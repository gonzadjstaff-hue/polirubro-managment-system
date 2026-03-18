using System;
using System.Collections.Generic;
using System.Text;

namespace Polirubro.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool EsAdmin { get; set; }
        public bool Activo { get; set; }
        public string Nombre { get; set; }
    }
}