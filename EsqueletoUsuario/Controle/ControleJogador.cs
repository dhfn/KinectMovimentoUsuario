using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsqueletoUsuario.Auxiliar
{
    public class ControleJogador
    {
        public BitArray controle;
        private StatusJogador status;
        public String strStatus;

        public ControleJogador()
        {
            controle = new BitArray(4);
            status = new StatusJogador();
        }

        public void Andar()
        {
            if(!controle[(int)Controles.PULAR] && !controle[(int)Controles.AGACHAR])
            {
                Console.WriteLine("Andando");
                strStatus = "Andando";
            }
        }

        public void Agachar()
        {
            if (!controle[(int)Controles.PULAR])
            {
                controle[(int)Controles.AGACHAR] = true;
                Console.WriteLine("Agachado");
                strStatus = "Agachando";
            }
        }

        public void Pular()
        {
            controle[(int)Controles.PULAR] = true;
            Console.WriteLine("Pulo");
            strStatus = "Pulando";
        }

        private void EnviarControle()
        {
            //enviar this.controle para função de envio pela rede;
        }
    }
}
