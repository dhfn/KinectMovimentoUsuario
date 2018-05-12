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

        private int contMudancaDirecao;
        private int contRepouso;
        private int maxFramesMudaDirecao;
        private int maxFramesRepouso;

        public ControleJogador()
        {
            controle = new BitArray(4);
            status = new StatusJogador();

            contMudancaDirecao = 0;
            contRepouso = 0;
            maxFramesMudaDirecao = 20;
            maxFramesRepouso = 20;
        }

        public void AndarFrente()
        {
            if(!controle[(int)Controles.PULAR] && !controle[(int)Controles.AGACHAR])
            {
                Console.WriteLine("Andando para frente");
                strStatus = "Andando para frente";
            }
        }

        public void AndarTras()
        {
            if (!controle[(int)Controles.PULAR] && !controle[(int)Controles.AGACHAR])
            {
                Console.WriteLine("Andando para trás");
                strStatus = "Andando para trás";
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
