using System;
using System.Collections.Generic;
using System.Text;
using tabuleiro;
using Xadrez;

namespace Xadrez_Console
{
    class Tela
    {
        public static void ImprimirPartida(PartidaDeXadrez partida)
        {
            Tela.ImprimirTabuleiro(partida.tab);
            Console.WriteLine();
            ImprimirPecasCapturadas(partida);
            Console.WriteLine();
            Console.WriteLine("Turno: " + partida.turno);
            Console.WriteLine("Aguardando jogada: " + partida.jogadorAtual);
            if(partida.xeque)
                Console.WriteLine("XEQUE!");
        }

        public static void ImprimirPecasCapturadas(PartidaDeXadrez partida)
        {
            ConsoleColor aux = Console.ForegroundColor;
            Console.WriteLine("Pecas capturadas:");
            Console.Write("Brancas: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            ImprimirConjunto(partida.pecasCapturadas(Cor.Branca));
            Console.ForegroundColor = aux;
            Console.WriteLine();
            Console.Write("Pretas: ");
            Console.ForegroundColor = ConsoleColor.Red;
            ImprimirConjunto(partida.pecasCapturadas(Cor.Preta));
            Console.WriteLine();
            Console.ForegroundColor = aux;
        }

        public static void ImprimirConjunto(HashSet<Peca> pecasCapturadas)
        {
            Console.Write("[");
            foreach(Peca x in pecasCapturadas)
            {
                Console.Write(x + " ");
            }
            Console.Write("]");
        }
        public static void ImprimirPeca(Peca peca)
        {
            if(peca == null)
            {
                Console.Write("-");
            }
            else
            {
                if(peca.Cor == Cor.Branca)
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }


            }
            Console.Write(" ");
        }
        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            for(int i = 0; i < tab.Linhas; i++)
            {
                Console.Write(8 - i + " ");
                for(int j = 0; j < tab.Colunas; j++)
                {
                    ImprimirPeca(tab.peca(i, j));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }

        public static void ImprimirTabuleiro(Tabuleiro tab, bool[,] posicoesPossiveis)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;
            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (posicoesPossiveis[i, j])
                        Console.BackgroundColor = fundoAlterado;
                    else
                        Console.BackgroundColor = fundoOriginal;
                    ImprimirPeca(tab.peca(i, j));
                    Console.BackgroundColor = fundoOriginal;
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
            Console.BackgroundColor = fundoOriginal;
        }

        public static PosicaoXadrez lerPosicaoXadrez()
        {
            string s = Console.ReadLine();
            char coluna = s[0];
            int linha = int.Parse(s[1] + "");
            return new PosicaoXadrez(coluna, linha);
        }
    }
}
