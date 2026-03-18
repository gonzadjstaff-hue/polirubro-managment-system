using Polirubro.Entidades;

namespace PolirubroWPF
{
    public static class Sesion
    {
        public static Usuario UsuarioActual { get; set; }
        public static bool HayUsuarioLogueado => UsuarioActual != null;
        public static void CerrarSesion()
        {
            UsuarioActual = null;
        }
    }
}
