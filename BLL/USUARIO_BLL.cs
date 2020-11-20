using DATABASE;
using ENTIDADE;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class USUARIO_BLL
    {
        private USUARIO_DB usuario_db = new USUARIO_DB();

        public void InserirUsuario(USUARIO usuario)
        {
            if (string.IsNullOrEmpty(usuario.NOME))
            {
                throw new Exception("Informe o nome do usuário");
            }

            if (string.IsNullOrEmpty(usuario.SENHA))
            {
                throw new Exception("Informe a senha do usuário");
            }

            if (string.IsNullOrEmpty(usuario.EMAIL))
            {
                throw new Exception("Informe o email do usuário");
            }

            if (string.IsNullOrEmpty(usuario.CPF))
            {
                throw new Exception("Informe o CPF do usuário");
            }

            usuario_db.InserirUsuarioBancoDados(usuario);
        }

        public USUARIO Login(USUARIO usuario)
        {
            if(string.IsNullOrEmpty(usuario.SENHA))
            {
                throw new Exception("Informe a senha");
            }

            if (string.IsNullOrEmpty(usuario.EMAIL))
            {
                throw new Exception("Informe a e-mail");
            }

            USUARIO usuarioLogin = usuario_db.SelectLoginBancoDados(usuario);

            if(string.IsNullOrEmpty(usuarioLogin.NOME) || string.IsNullOrEmpty(usuarioLogin.EMAIL) || usuarioLogin.ID == 0)
            {
                throw new Exception("Email e/ou Senha incorreto");
            }

            return usuarioLogin;
        }
    }
}
