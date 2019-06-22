using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Propellant
{
    public PropellantType type;
    public float ratio;
}

public enum PropellantType
{
    Fuel,
    Air
}

