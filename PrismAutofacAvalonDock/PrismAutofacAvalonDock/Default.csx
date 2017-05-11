
var a = Average(raw);


public Pixel<float> Filter(Pixel<float> src)
{
    src
        .DivSelf(100)
        .StaggerL();
    return src;
}
public double Average(Pixel<float> src)
{
    return src.Cut().Average();
}
