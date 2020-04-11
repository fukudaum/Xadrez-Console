using System;
using System.Collections.Generic;
using System.Text;

namespace tabuleiro
{
    class Peca
    {
        public Cor Cor { get; protected set; }
        public Posicao Posicao { get; set; }
        public int QtdMovimentos { get; protected set; }

        public Tabuleiro Tab { get; protected set; }

        public Peca()
        {
                
        }

        public Peca(Cor cor, Tabuleiro tab)
        {
            Cor = cor;
            Tab = tab;
            QtdMovimentos = 0;
            Posicao = null;
        }

        public void IncrementarMovimentos()
        {
            QtdMovimentos++;
        }
        public override string ToString()
        {
            return "K";
        }
    }
}
