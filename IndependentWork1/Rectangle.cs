namespace IndependentWork1;

public class Rectangle
{
    private double _width;
    private double _height;

    public double Width => _width;
    public double Height => _height;

    public Rectangle(double width, double height)
    {
        _width = width > 0 ? width : 1.0;
        _height = height > 0 ? height : 1.0;
    }

    public double CalculateArea()
    {
        return _width * _height;
    }

    public double CalculatePerimeter()
    {
        return 2 * (_width + _height);
    }
}