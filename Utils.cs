using System;

class Utils
{
    static Random RandomGen = new Random();

    public static void SetRandomSeed(int pSeed)
    {
        RandomGen = new Random(pSeed); // Permet de générer les même nombres aléatoires
    }
    public static int GetInt(int pMin, int pMax)
    {
        return RandomGen.Next(pMin, pMax + 1);
    }
}