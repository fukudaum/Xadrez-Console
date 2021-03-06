﻿using System;
using System.Collections.Generic;
using System.Text;
using tabuleiro;

namespace Xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool partidaTerminada { get; private set; }
        public bool xeque { get; private set; }
        public Peca vulneravelEnPassant;
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            partidaTerminada = false;
            xeque = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            ColocarPecas();
            vulneravelEnPassant = null;
        }

        public void ValidarPosicaoOrigem(Posicao pos)
        {
            if (tab.peca(pos) == null)
                throw new TabuleiroException("Nao existe peca na posicao de origem escolhida!");
            if (jogadorAtual != tab.peca(pos).Cor)
                throw new TabuleiroException("A peca de origem nao e sua!");
            if (!tab.peca(pos).ExisteMovimentosPossiveis())
                throw new TabuleiroException("Nao existe movimentos possiveis para essa peca!");

        }

        public void ValidarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).MovimentoPossivel(destino))
                throw new TabuleiroException("Posicao de destino invalida!");

        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.IncrementarMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
            // #jogadaespecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.retirarPeca(origemT);
                T.IncrementarMovimentos();
                tab.colocarPeca(T, destinoT);
            }
            // #jogadaespecial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.retirarPeca(origemT);
                T.IncrementarMovimentos();
                tab.colocarPeca(T, destinoT);
            }

            //#jogadaespecial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    pecaCapturada = tab.retirarPeca(posP);
                    capturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.DecrementarMovimentos();
            if (pecaCapturada != null)
            {
                tab.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);

            }
            tab.colocarPeca(p, origem);

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.retirarPeca(destinoT);
                T.DecrementarMovimentos();
                tab.colocarPeca(T, origemT);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.retirarPeca(destinoT);
                T.DecrementarMovimentos();
                tab.colocarPeca(T, origemT);
            }

            //#jogadaespecial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == vulneravelEnPassant)
                {
                    Peca peao = tab.retirarPeca(destino);
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    tab.colocarPeca(peao, posP);
                }
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);
            if (estaEmCheque(jogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Voce nao pode se colocar em cheque!");
            }

            Peca p = tab.peca(destino);

            //#jogadaespecial promocao
            if (p is Peao)
            {
                if ((p.Cor == Cor.Branca && destino.Linha == 0) || p.Cor == Cor.Preta && destino.Linha == 7)
                {
                    p = tab.retirarPeca(destino);
                    pecas.Remove(p);
                    Peca dama = new Rainha(p.Cor, tab);
                    tab.colocarPeca(dama, destino);
                    pecas.Add(dama);
                }
            }
            if (estaEmCheque(adversaria(jogadorAtual)))
            {
                xeque = true;
            }
            else
                xeque = false;

            if (estaEmChequeMate(adversaria(jogadorAtual)))
            {
                partidaTerminada = true;
            }
            else
            {
                turno++;
                MudaJogador();
            }

            //#jogadaespecial En Passant

            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
                vulneravelEnPassant = p;
            else
                vulneravelEnPassant = null;
        }

        public void MudaJogador()
        {
            if (jogadorAtual == Cor.Branca)
                jogadorAtual = Cor.Preta;
            else
                jogadorAtual = Cor.Branca;

        }

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        private Cor adversaria(Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            else
                return Cor.Branca;
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca x in pecasEmJogo(cor))
                if (x is Rei)
                    return x;
            return null;
        }

        public bool estaEmCheque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null)
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro!");

            foreach (Peca x in pecasEmJogo(adversaria(cor)))
            {
                bool[,] matriz = x.MovimentosPossiveis();
                if (matriz[R.Posicao.Linha, R.Posicao.Coluna])
                    return true;
            }
            return false;
        }

        public bool estaEmChequeMate(Cor cor)
        {
            if (!estaEmCheque(cor))
                return false;
            foreach (Peca x in pecasEmJogo(cor))
            {
                bool[,] matriz = x.MovimentosPossiveis();
                for (int i = 0; i < tab.Linhas; i++)
                {
                    for (int j = 0; j < tab.Colunas; j++)
                    {
                        if (matriz[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = estaEmCheque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca p)
        {
            tab.colocarPeca(p, new PosicaoXadrez(coluna, linha).ToPosicao());
            pecas.Add(p);
        }
        private void ColocarPecas()
        {

            ColocarNovaPeca('a', 8, new Torre(Cor.Preta, tab));
            ColocarNovaPeca('b', 8, new Cavalo(Cor.Preta, tab));
            ColocarNovaPeca('c', 8, new Bispo(Cor.Preta, tab));
            ColocarNovaPeca('d', 8, new Rei(Cor.Preta, tab, this));
            ColocarNovaPeca('e', 8, new Rainha(Cor.Preta, tab));
            ColocarNovaPeca('f', 8, new Bispo(Cor.Preta, tab));
            ColocarNovaPeca('g', 8, new Cavalo(Cor.Preta, tab));
            ColocarNovaPeca('h', 8, new Torre(Cor.Preta, tab));

            ColocarNovaPeca('a', 7, new Peao(Cor.Preta, tab, this));
            ColocarNovaPeca('b', 7, new Peao(Cor.Preta, tab, this));
            ColocarNovaPeca('c', 7, new Peao(Cor.Preta, tab, this));
            ColocarNovaPeca('d', 7, new Peao(Cor.Preta, tab, this));
            ColocarNovaPeca('e', 7, new Peao(Cor.Preta, tab, this));
            ColocarNovaPeca('f', 7, new Peao(Cor.Preta, tab, this));
            ColocarNovaPeca('g', 7, new Peao(Cor.Preta, tab, this));
            ColocarNovaPeca('h', 7, new Peao(Cor.Preta, tab, this));

            ColocarNovaPeca('a', 1, new Torre(Cor.Branca, tab));
            ColocarNovaPeca('b', 1, new Cavalo(Cor.Branca, tab));
            ColocarNovaPeca('c', 1, new Bispo(Cor.Branca, tab));
            ColocarNovaPeca('d', 1, new Rei(Cor.Branca, tab, this));
            ColocarNovaPeca('e', 1, new Rainha(Cor.Branca, tab));
            ColocarNovaPeca('f', 1, new Bispo(Cor.Branca, tab));
            ColocarNovaPeca('g', 1, new Cavalo(Cor.Branca, tab));
            ColocarNovaPeca('h', 1, new Torre(Cor.Branca, tab));

            ColocarNovaPeca('a', 2, new Peao(Cor.Branca, tab, this));
            ColocarNovaPeca('b', 2, new Peao(Cor.Branca, tab, this));
            ColocarNovaPeca('c', 2, new Peao(Cor.Branca, tab, this));
            ColocarNovaPeca('d', 2, new Peao(Cor.Branca, tab, this));
            ColocarNovaPeca('e', 2, new Peao(Cor.Branca, tab, this));
            ColocarNovaPeca('f', 2, new Peao(Cor.Branca, tab, this));
            ColocarNovaPeca('g', 2, new Peao(Cor.Branca, tab, this));
            ColocarNovaPeca('h', 2, new Peao(Cor.Branca, tab, this));
        }

    }
}
