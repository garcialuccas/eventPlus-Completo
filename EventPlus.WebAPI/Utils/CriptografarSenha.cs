using BCrypt.Net;

namespace EventPlus.WebAPI.Utils
{
    public static class CriptografarSenha
    {

        private const int WorkFactor = 12;


        public static string Criptografarsenha(string senhaPura)
        {
            if (string.IsNullOrWhiteSpace(senhaPura))
                throw new ArgumentException("Senha não pode ser vazia.", nameof(senhaPura));

                return BCrypt.Net.BCrypt.HashPassword(senhaPura, workFactor: WorkFactor);
        }


        public static bool VerificarSenha(string senhapura, string senhahash) 
        {
            if (string.IsNullOrEmpty(senhapura) || string.IsNullOrEmpty(senhahash)) return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(senhapura, senhahash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
                throw;
            }
        
        }

    }
}
