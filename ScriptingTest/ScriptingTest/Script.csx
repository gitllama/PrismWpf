int i = 1;
for (int y = 0; y < P.Height; y++)
{
    for (int x = 0; x < P.Width; x++)
    {
        P[x, y] += i;
    }
}
