using System;
using System.Collections.Generic;
using System.Text;

namespace tabuleiro
{
    class Tabuleiro
    {
        public int Colunas { get; set; }
        public int Linhas { get; set; }
        private Peca[,] _pecas;

        public Tabuleiro(int linhas, int colunas)
        {
            Colunas = colunas;
            Linhas = linhas;
            _pecas = new Peca[linhas, colunas];
        }

        public bool posicaoValida(Posicao pos)
        {
            if (pos.Linha < 0 || pos.Linha >= Linhas || pos.Coluna < 0 || pos.Coluna >= Colunas)
                return false;

            return true;
        }

        public void validarPosicao(Posicao pos)
        {
            if (!posicaoValida(pos))
            {
                throw new TabuleiroException("Posicao Invalida!");
            }
        }

        public Peca peca(int linha, int coluna)
        {
            return _pecas[linha, coluna];
        }
        public Peca peca(Posicao pos)
        {
             return _pecas[pos.Linha, pos.Coluna];
        }

        public bool existePeca(Posicao pos)
        {
            validarPosicao(pos);
            return peca(pos) != null;
        }

        public void colocarPeca(Peca peca, Posicao pos)
        {
            if (existePeca(pos))
            {
                throw new TabuleiroException("Ja existe peca nessa posicao!");
            }
            _pecas[pos.Linha, pos.Coluna] = peca;
            peca.Posicao = pos;
        }

        public Peca retirarPeca(Posicao pos)
        {
            if (peca(pos) == null)
                return null;
            Peca aux = peca(pos);
            aux.Posicao = null;
            _pecas[pos.Linha, pos.Coluna] = null;
            return aux;
        }


    }
}
